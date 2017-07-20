using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tweety.Models.Twitter
{
    internal class CRCResponseToken
    {
        [JsonProperty("response_token")]
        public string Token { get; set; }
    }
}
