using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Models
{
    public class ImageClassifierService : IDisposable, IImageClassifierService
    {
        private readonly ILogger<ImageClassifierService> _logger;
        private readonly Dictionary<string, ImageClassifierWorkerPool> _namedWorkerPools;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ImageClassifierService(ILogger<ImageClassifierService> logger, IImageClassifierFactory factory,
            IConfiguration configuration)
        {
            var serviceConfiguration = configuration.GetSection("ImageClassifierService")
                .Get<ImageClassifierServiceConfiguration>();
            _cancellationTokenSource = new CancellationTokenSource();
            _logger = logger;
            _namedWorkerPools = new Dictionary<string, ImageClassifierWorkerPool>();
            foreach (var classifierConfiguration in serviceConfiguration.ImageClassifierConfigurations)
            {
                try
                {
                    _namedWorkerPools.Add(classifierConfiguration.ClassifierType,
                        new ImageClassifierWorkerPool(factory, classifierConfiguration,
                            serviceConfiguration.WorkersPerModel));
                }
                catch (UnknownClassifierTypeException e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting ImageClassifierService's pools.");
            var tasks = new List<Task>();
            foreach (var namedWorkerPool in _namedWorkerPools)
            {
                tasks.Add(namedWorkerPool.Value.StartWorkers(_cancellationTokenSource.Token));
            }

            try
            {
                await Task.WhenAll(tasks.ToArray());
            }
            catch (ModelNotLoadedException exception)
            {
                _logger.LogError(exception.Message);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            var tasks = new List<Task>();
            foreach (var namedWorkerPool in _namedWorkerPools)
            {
                tasks.Add(namedWorkerPool.Value.StopWorkers());
            }

            return Task.WhenAll(tasks.ToArray());
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public Task<string> Classify(string classifierType, Image image)
        {
            if (_namedWorkerPools.ContainsKey(classifierType))
            {
                return _namedWorkerPools[classifierType].Classify(image);
            }

            throw new UnknownClassifierTypeException($"Model with type {classifierType} wasn't initialized.");
        }
    }
}