using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Xunit;

namespace Test
{
    public class ImageClassifierWorkerTest
    {
        [Fact]
        public async Task SmokeTestWorkers()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var workers = new List<IImageClassifierWorker>();
            for (var i = 0; i < 5; ++i)
            {
                var configuration = new ImageClassifierConfiguration
                {
                    ImageHeight = 28, ImageWidth = 28,
                    Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"},
                    PathToModel = "TestData/traced_model.pt"
                };
                var classifier = new MnistImageClassifier(configuration);
                var worker = new ImageClassifierWorker(classifier);
                await worker.StartWorker(cancellationTokenSource.Token);
                workers.Add(worker);
            }

            var image = Image.FromFile("TestData/seven.png");
            var results = new List<Task<string>>();
            foreach (var worker in workers)
            {
                for (var i = 0; i < 10; i++)
                {
                    results.Add(worker.Classify(image));
                }
            }

            var labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            foreach (var taskResult in results)
            {
                Assert.Contains(await taskResult, labels);
            }

            cancellationTokenSource.Cancel();
            foreach (var worker in workers)
            {
                worker.StopWorker();
            }
        }
    }
}