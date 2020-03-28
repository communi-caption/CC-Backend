using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommunicaptionBackend.Core;
using CommunicaptionBackend.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaptionBackend.Controllers
{
    [Route("/")]
    public class UserController : Controller
    {
        private Guid guidPin;
        private MessageProcessor messageProcessor;

        public UserController(MessageProcessor messageProcessor)
        {
            this.messageProcessor = messageProcessor;
        }

        [HttpGet("pin")]
        public IActionResult GeneratePin()
        {
            guidPin = Guid.NewGuid();
            return Content(guidPin.ToString());
        }

        [HttpGet("media/{userId}/{mediaId}")]
        public HttpResponseMessage GetMedia(string userId, string mediaId)
        {
            var stream = new MemoryStream();

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.GetBuffer())
            };
            result.Content.Headers.ContentDisposition = new GetMediaRequest
            {
                UserId = userId,
                MediaId = mediaId
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        [HttpPost("pushMessage/{userId}")]
        public IActionResult PushMessage(string userId, Message message)
        {
            messageProcessor.PushMessage(userId, message);

            return ActionResults.Json(new
            {
                message = "Pushed Message."
            });
        }
    }
}