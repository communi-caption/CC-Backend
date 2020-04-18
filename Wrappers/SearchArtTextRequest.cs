using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Wrappers
{
    public class SearchArtTextRequest
    {
        public string Text { get; set; }
        public string ArtId { get; set; }
    }
}
