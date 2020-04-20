using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using CommunicaptionBackend.Api;
using CommunicaptionBackend.Core;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Messages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommunicaptionBackend.Api {

    [Route("/")]
    public class MainController : ControllerBase {
        private readonly MainService mainService;

        public MainController(MainService mainService) {
            this.mainService = mainService;
        }

        [HttpGet("")] //tested
        public IActionResult Index() {
            return Ok("slm nbr?");
        }

        [HttpGet("pin")] // --
        public IActionResult GeneratePin() {
            return ActionResults.Json(new {
                pin = mainService.GeneratePin()
            });
        }

        [HttpGet("clearDb")] // --
        public IActionResult ClearDb() {
            mainService.ClearDb();
            return Ok("ok");
        }

        [HttpPost("setDump")]
        public IActionResult SetDump([FromBody] DumpWrapper dumpWrapper) {
            mainService.SetDump(dumpWrapper);
            return Ok("ok");
        }

        [HttpGet("debugText")] 
        public IActionResult debugText(string fileName) {
            return File(System.IO.File.ReadAllBytes("texts / " + fileName), " .txt");
        }

        [HttpGet("media")] 
        public IActionResult GetMedia(int userId, string mediaId) {
            return File(mainService.GetMediaData(mediaId), "application/octet-stream");
        }

        [HttpPost("getDetails")] //db gelince test et
        public IActionResult GetDetails(int artId)
        {
            var result = mainService.getDetails(artId);
            return ActionResults.Json(result);
        }

        [HttpGet("search")] // ??? 
        public IActionResult Search(string json)
        {
            return ActionResults.Json(mainService.getSearchResult(json));
        }

        [HttpGet("gallery")] // calismiyor
        public IActionResult Gallery(int userId) {
            return ActionResults.Json(mainService.GetGallery(userId));
        }

        [HttpGet("dump")] // calismiyor
        public IActionResult Dump() {
            return ActionResults.Json(mainService.Dump());
        }

        [HttpGet("mediaItems")] 
        public IActionResult GetMediaItems(int userId)
        {
            return ActionResults.Json(mainService.GetMediaItems(userId, true));
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

        [HttpPost("createArt")] 
        public IActionResult CreateArt(int userId, string artTitle) {
            int artId = mainService.CreateArt(userId, artTitle);
            return ActionResults.Json(new {
                artId = artId
            });
        }

        [HttpGet("triggerTrain")] //db gelince test et
        public IActionResult TriggerTrain() {
            mainService.TriggerTrain();
            return Ok("trained");
        }

        [HttpGet("trainDebug")]
        public IActionResult TrainDebug() {
            return Ok(mainService.TrainDebug());
        }
    }
}

public class DumpWrapper {
    public List<MediaEntity> Medias { get; set; }
    public List<SettingsEntity> Settings { get; set; }
    public List<TextEntity> Texts { get; set; }
    public List<ArtEntity> Arts { get; set; }
    public List<string> MediaBase64 { get; set; }
}