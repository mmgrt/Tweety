using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tweety.Models.Twitter
{

    public class TwitterError
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }

        public override string ToString()
        {
            return string.Join("\n", Errors.Select(err => $"Error Code: {err.Code}, Error Message: {err.Message}"));
        }
    }

    public class Error
    {

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
