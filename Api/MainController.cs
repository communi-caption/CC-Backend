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

        [HttpGet("")]
        public IActionResult Index() {
            return Ok("slm nbr?");
        }

        [HttpGet("pin")]
        public IActionResult GeneratePin() {
            return ActionResults.Json(new {
                pin = mainService.GeneratePin()
            });
        }

        [HttpGet("debugMedia")]
        public IActionResult DebugMedia(string fileName) {
            return File(System.IO.File.ReadAllBytes("medias/" + fileName), "image/jpeg");
        }

        [HttpGet("media")]
        public IActionResult GetMedia(int userId, string mediaId) {
            return File(mainService.GetMediaData(mediaId), "application/octet-stream");
        }

        [HttpGet("search")]
        public IActionResult Search(string json)
        {
            var result = mainService.getSearchResult(json);
            return ActionResults.Json(new
            {
                message = result
            });
        }

        [HttpGet("mediaItems")]
        public IActionResult GetMediaItems(int userId)
        {
            return ActionResults.Json(new
            {
                items = JsonConvert.SerializeObject(mainService.GetMediaItems(userId))
            });
        }

        [HttpPost("pushMessage")]
        public IActionResult PushMessage(int userId, [FromBody] Message message) {
            mainService.PushMessage(message);

            return ActionResults.Json(new {
                message = "Pushed Message."
            });
        }

        [HttpPost("saveMediaMessage")]
        public IActionResult SaveMediaMessage([FromBody] SaveMediaMessage message) {
            mainService.PushMessage(message);

            return ActionResults.Json(new {
                message = "Pushed Message."
            });
        }

        [HttpPost("saveTextMessage")]
        public IActionResult SaveTextMessage([FromBody] SaveTextMessage message) {
            mainService.PushMessage(message);

            return ActionResults.Json(new {
                message = "Pushed Message."
            });
        }

        [HttpPost("saveSettingsMessage")]
        public IActionResult SaveSettings([FromBody] SettingsChangedMessage message) {
            mainService.PushMessage(message);

            return ActionResults.Json(new {
                message = "Pushed Message."
            });
        }

        [HttpGet("getSettings")]
        public IActionResult GetSettings(int userId) {
            return new ContentResult {
                Content = mainService.GetUserSettings(userId),
                ContentType = "application/json",
                StatusCode = 200
            };
        }

        [HttpGet("getMessages")]
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

        [HttpPost("disconnectDevice")]
        public IActionResult DisconnectDevice(int userId) {
            mainService.DisconnectDevice(userId);

            return ActionResults.Json(new {
                message = "Device Disconnected."
            });
        }

        [HttpGet("checkPairing")]
        public IActionResult CheckForPairing(string pin) {
            return ActionResults.Json(new {
                userId = mainService.CheckForPairing(pin)
            });
        }

        [HttpPost("connectWithHololens")]
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
                }, 400);
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