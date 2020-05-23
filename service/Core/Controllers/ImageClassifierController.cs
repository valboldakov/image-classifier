using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Grpc.Core;
using ImageClassifierGrpc;
using Microsoft.Extensions.Logging;

namespace Core.Controllers
{
    public class ImageClassifierController : ImageClassificationService.ImageClassificationServiceBase
    {
        private readonly ILogger<ImageClassifierController> _logger;
        private readonly IImageClassifierService _classifierService;

        public ImageClassifierController(ILogger<ImageClassifierController> logger, IImageClassifierService service)
        {
            _logger = logger;
            _classifierService = service;
        }

        public override async Task<ImageClassificationResponse> ClassifyImage(ImageClassificationRequest request,
            ServerCallContext context)
        {
            string label;
            Image image;
            using (var stream = new MemoryStream(request.ImageData.ToByteArray(), false))
            {
                try
                {
                    _logger.LogInformation("Received bytes: " + request.ImageData.ToByteArray().Length.ToString());
                    image = Image.FromStream(stream);
                }
                catch (Exception)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument,
                        $"Couldn't create image."));
                }
            }

            try
            {
                label = await _classifierService.Classify(request.ClassifierType, image);
            }
            catch (UnknownClassifierTypeException)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    $"Unknown classifier type '{request.ClassifierType}'"));
            }

            return new ImageClassificationResponse
            {
                Label = label
            };
        }
    }
}