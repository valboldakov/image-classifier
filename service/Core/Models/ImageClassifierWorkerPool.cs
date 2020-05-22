using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ImageClassifierWorkerPool : IImageClassifierWorkersPool
    {
        private int _roundRobinCounter;
        private readonly List<IImageClassifierWorker> _workers;

        public ImageClassifierWorkerPool(IImageClassifierFactory factory, ImageClassifierConfiguration configuration,
            int workersAmount)
        {
            _roundRobinCounter = 0;
            _workers = new List<IImageClassifierWorker>();
            for (var i = 0; i < workersAmount; ++i)
            {
                _workers.Add(new ImageClassifierWorker(factory.GetClassifier(configuration)));
            }
        }

        public Task<string> Classify(Image image)
        {
            _roundRobinCounter += 1;
            return _workers[_roundRobinCounter % _workers.Count].Classify(image);
        }

        public Task StartWorkers(CancellationToken cancellationToken)
        {
            foreach (var worker in _workers)
            {
                worker.StartWorker(cancellationToken);
            }

            return Task.CompletedTask;
        }

        public Task StopWorkers()
        {
            foreach (var worker in _workers)
            {
                worker.Thread.Join();
            }

            return Task.CompletedTask;
        }
    }
}