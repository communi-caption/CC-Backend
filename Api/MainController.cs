using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using CommunicaptionBackend.Api;
using CommunicaptionBackend.Core;
using CommunicaptionBackend.Messages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommunicaptionBackend.Api {

    [Route("/")]
    public class MainController : ControllerBase {
        private readonly IMainService mainService;

        public MainController(IMainService mainService) {
            this.mainService = mainService;
        }

        [HttpGet("pin")]
        public IActionResult GeneratePin() {
            return ActionResults.Json(new {
                pin = mainService.GeneratePin()
            });
        }

        [HttpGet("media/{userId}/{mediaId}")]
        public IActionResult GetMedia(string userId, string mediaId) {
            return File(mainService.GetMediaData(mediaId), "application/octet-stream");
        }

        [HttpPost("pushMessage/{userId}")]
        public IActionResult PushMessage(string userId, [FromBody] Message message) {
            mainService.PushMessage(message);

            return ActionResults.Json(new {
                message = "Pushed Message."
            });
        }
        [HttpGet("media/{userId}")]
        public IActionResult GetMessages(string userId)
        {
            if(!mainService.CheckUserExists(userId))
                return ActionResults.Json(new
                {
                    error = "No such user!"
                }, 200);

            return ActionResults.Json(new
            {
                messages = JsonConvert.SerializeObject(mainService.GetMessages())
            });
        }
        [HttpPost("disconnectDevice/{userId}")]
        public IActionResult DisconnectDevice(string userId) {
            mainService.DisconnectDevice(userId);

            return ActionResults.Json(new {
                message = "Device Disconnected."
            });
        }

        [HttpGet("checkPairing/{pin}")]
        public IActionResult CheckForPairing(string pin) {
            return ActionResults.Json(new {
                userId = mainService.CheckForPairing(pin)
            });
        }

        [HttpPost("connectWithHololens/{pin}")]
        public IActionResult ConnectWithHololens(string pin) {
            string userId = mainService.ConnectWithHoloLens(pin);

            if (userId != null) {
                return ActionResults.Json(new {
                    userId = userId
                }, 200);
            }
            else {
                return ActionResults.Json(new {
                    error = "Pin does not match with any device"
                }, 200);
            }
        }

        [HttpPost("connectWithoutHololens")]
        public IActionResult ConnectWithoutHololens() {
            string userId = mainService.ConnectWithoutHoloLens();
            return ActionResults.Json(new {
                userId = userId
            });
        }
    }
}
