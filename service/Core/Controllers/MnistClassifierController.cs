using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core.Controllers
{
    public class MnistClassifierController : Controller
    {
        private ILogger<MnistClassifierController> _logger;
        private IImageClassifierService _service;

        public MnistClassifierController(ILogger<MnistClassifierController> logger, IImageClassifierService service)
        {
            _logger = logger;
            _service = service;
        }

        [Route("MnistClassifier")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("MnistClassifier/recognize")]
        public async Task<IActionResult> Recognize([FromBody] ImageClassificationPostRequest postRequest)
        {
            string label;
            Image image;
            var imageData = Convert.FromBase64String(postRequest.Base64EncodedImage);
            using (var stream = new MemoryStream(imageData, false))
            {
                try
                {
                    _logger.LogInformation("Received bytes: " + imageData.Length);
                    image = Image.FromStream(stream);
                }
                catch (Exception)
                {
                    return new JsonResult(new {Error = "Couldn't create image."});
                }
            }

            try
            {
                label = await _service.Classify(postRequest.ClassifierType, image);
            }
            catch (UnknownClassifierTypeException)
            {
                throw new RpcException(new Status(Grpc.Core.StatusCode.InvalidArgument,
                    $"Unknown classifier type '{postRequest.ClassifierType}'"));
            }

            return new JsonResult(new {Label = label});
        }
    }
}