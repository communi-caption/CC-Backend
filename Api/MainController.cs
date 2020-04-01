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
        public IActionResult GetMedia(int userId, string mediaId) {
            return File(mainService.GetMediaData(mediaId), "application/octet-stream");
        }

        [HttpPost("pushMessage/{userId}")]
        public IActionResult PushMessage(int userId, [FromBody] Message message) {
            mainService.PushMessage(message);

            return ActionResults.Json(new {
                message = "Pushed Message."
            });
        }
        [HttpGet("getMessages/{userId}")]
        public IActionResult GetMessages(int userId)
        {
            if(!mainService.CheckUserExists(userId)) {
                return ActionResults.Json(new {
                    error = "No such user!"
                }, 400);
            }

            return ActionResults.Json(new
            {
                messages = JsonConvert.SerializeObject(mainService.GetMessages(userId))
            });
        }
        [HttpPost("disconnectDevice/{userId}")]
        public IActionResult DisconnectDevice(int userId) {
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
            int userId = mainService.ConnectWithHoloLens(pin);

            if (userId != 0) {
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
            int userId = mainService.ConnectWithoutHoloLens();
            return ActionResults.Json(new {
                userId = userId
            });
        }
    }
}
