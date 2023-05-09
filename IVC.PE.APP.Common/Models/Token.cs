using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Common.Models
{

    public class Token
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("access_Token")]
        public string Access_Token { get; set; }
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
        [JsonProperty("expiration")]
        public string Expiration { get; set; }
    }
}
