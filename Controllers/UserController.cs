using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommunicaptionBackend.Contexts;
using CommunicaptionBackend.Core;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaptionBackend.Controllers
{
    [Route("/")]
    public class UserController : ControllerBase
    {
        private Guid guidPin;
        private MessageProcessor messageProcessor;
        private Context userContext;

        public UserController(MessageProcessor messageProcessor, Context userContext)
        {
            this.messageProcessor = messageProcessor;
            this.userContext = userContext;
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

        [HttpPost("disconnectDevice/{userId}")]
        public IActionResult DisconnectDevice(string userId)
        {
            //For the given userId, the match record in the database is removed.

            return ActionResults.Json(new
            {
                message = "Device Disconnected."
            });
        }

        [HttpGet("checkPairing/{pin}")]
        public HttpResponseMessage CheckForPairing(string pin)
        {
            string userId = null;

            //The userId value of the record matching the pin on database is returned.

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(userId)
            };
            return result;
        }

        [HttpPost("connectWithoutHololens")]
        public IActionResult ConnectWithoutHololens()
        {
            User user = new User();
            userContext.Users.Add(user);
            userContext.SaveChanges();
        }

    }
}