using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Xunit;

namespace Test
{
    public class ImageClassifierWorkerPoolTest
    {
        [Fact]
        public async void ImageClassifierWorkerPoolSmokeTest()
        {
            var configuration = new ImageClassifierConfiguration
            {
                ClassifierType = "MNISTClassifier",
                ImageHeight = 28, ImageWidth = 28,
                Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"},
                PathToModel = "TestData/traced_model.pt"
            };
            var factory = new ImageClassifierFactory();
            var pool = new ImageClassifierWorkerPool(factory, configuration, 5);
            using var source = new CancellationTokenSource();
            await pool.StartWorkers(source.Token);
            var image = Image.FromFile("TestData/seven.png");
            var results = new List<Task<string>>();
            for (var i = 0; i < 100; i++)
            {
                results.Add(pool.Classify(image));
            }

            var labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            foreach (var taskResult in results)
            {
                Assert.Contains(await taskResult, labels);
            }

            source.Cancel();
            await pool.StopWorkers();
        }
    }
}