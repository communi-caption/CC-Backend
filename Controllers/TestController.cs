using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommunicaptionBackend.Controllers {

    [Route("/")]
    public class TestController : ControllerBase {

        [HttpGet("deneme")]
        public IActionResult Deneme() {
            return ActionResults.Json(new {
                message = "deneme"
            });
        }
    }
}
