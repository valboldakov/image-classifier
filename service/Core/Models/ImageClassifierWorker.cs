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
        private Thread _thread;
        private bool _isCanceled;

        public void StopWorker()
        {
            _thread.Join();
        }

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

        public Task<int> StartWorker(CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<int>();
            _thread = new Thread(() =>
            {
                try
                {
                    _classifier.LoadModel();
                }
                catch (ModelNotLoadedException exception)
                {
                    taskCompletionSource.SetException(exception);
                    return;
                }

                taskCompletionSource.SetResult(0);
                while (!_isCanceled)
                {
                    IImageClassificationTask classificationTask;
                    try
                    {
                        classificationTask = _queue.Receive(cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _isCanceled = true;
                        return;
                    }

                    string label;
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
            _thread.Start();
            return taskCompletionSource.Task;
        }
    }
}