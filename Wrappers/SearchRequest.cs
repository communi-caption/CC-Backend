using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Wrappers
{
    public class SearchRequest
    {
        public string keyword { get; set; }
        public bool spellCheck { get; set; }
    }
}
