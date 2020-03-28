using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Wrappers
{
    public class GetMediaRequest
    {
        public string UserId { get; set; }
        public string MediaId { get; set; }

        public static implicit operator ContentDispositionHeaderValue(GetMediaRequest v)
        {
            throw new NotImplementedException();
        }
    }
}
