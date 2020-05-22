using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Core.Exceptions;

namespace Core.Models
{
    public class ImageClassifierWorker : IImageClassifierWorker
    {
        private readonly IImageClassifier _classifier;
        private readonly BufferBlock<IImageClassificationTask> _queue;
        private bool _isCanceled;
        public Thread Thread { get; private set; }

        public ImageClassifierWorker(IImageClassifier imageClassifier)
        {
            _isCanceled = false;
            _classifier = imageClassifier;
            _queue = new BufferBlock<IImageClassificationTask>();
        }

        public Task<string> Classify(Image image)
        {
            if (_isCanceled)
            {
                throw new UsingCanceledWorkerException();
            }

            var taskCompletionSource =
                new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            _queue.Post(new ImageClassificationTask
            {
                Image = image, TaskCompletionSource = taskCompletionSource
            });
            return taskCompletionSource.Task;
        }

        public void StartWorker(CancellationToken cancellationToken)
        {
            Thread = new Thread(() =>
            {
                _classifier.LoadModel();
                while (!_isCanceled)
                {
                    IImageClassificationTask classificationTask;
                    try
                    {
                        classificationTask = _queue.Receive(cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }

                    var label = "";
                    try
                    {
                        label = _classifier.ClassifyImage(classificationTask.Image);
                    }
                    catch (ModelEvaluationFailedException exception)
                    {
                        classificationTask.TaskCompletionSource.SetException(exception);
                        return;
                    }

                    classificationTask.TaskCompletionSource.SetResult(label);
                    _isCanceled = cancellationToken.IsCancellationRequested;
                }
            });
            Thread.Start();
        }
    }
}