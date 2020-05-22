using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Test
{
    public class ImageClassifierServiceTest
    {
        [Fact]
        public async void ImageClassifierServiceSmokeTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("TestData/appsettings.json", optional: false, reloadOnChange: true);
            var factory = new ImageClassifierFactory();
            using var service = new ImageClassifierService(new NullLogger<ImageClassifierService>(), factory,
                configuration.Build());
            await service.StartAsync(default);
            var results = new List<Task<string>>();
            var image = Image.FromFile("TestData/seven.png");
            for (var i = 0; i < 25; i++)
            {
                results.Add(service.Classify("MNISTClassifier", image));
            }

            var labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            foreach (var taskResult in results)
            {
                Assert.Contains(await taskResult, labels);
            }

            await service.StopAsync(default);
        }

        [Fact]
        public async Task ThrowsIncorrectTypeExceptionTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("TestData/appsettings.json", optional: false, reloadOnChange: true);
            var factory = new ImageClassifierFactory();
            using var service = new ImageClassifierService(new NullLogger<ImageClassifierService>(), factory,
                configuration.Build());
            await service.StartAsync(default);
            var image = Image.FromFile("TestData/seven.png");
            await Assert.ThrowsAsync<UnknownClassifierTypeException>(() => service.Classify("Incorrect", image));
            await service.StopAsync(default);
        }
    }
}