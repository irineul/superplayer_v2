using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace SuperplayerChallenge.Objects
{
    public class ResponseFileDownload
    {
        [JsonProperty("suspects")]
        public List<String> Suspects { get; set; }

        [JsonProperty("guns")]
        public List<String> Guns { get; set; }

        [JsonProperty("locals")]
        public List<String> Locals { get; set; }
    }
}
