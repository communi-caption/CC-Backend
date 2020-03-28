using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaptionBackend.Controllers
{
    [Route("/")]
    public class UserController : Controller
    {
        private Guid guidPin;

        [HttpGet("pin")]
        public ActionResult GeneratePin()
        {
            guidPin = Guid.NewGuid();
            return Content(guidPin.ToString());
        }
    }
}