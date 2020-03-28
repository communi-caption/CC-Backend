using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommunicaptionBackend {

    public static class ActionResults {

        public static IActionResult Json(string json, int statusCode = 200) {
            return new ContentResult {
                Content = json,
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }

        public static IActionResult Json<T>(T obj, int statusCode = 200) {
            return new ContentResult {
                Content = JsonConvert.SerializeObject(obj),
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }
    }
}
