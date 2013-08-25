using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace SuperplayerChallenge.Objects
{
    public class ResponseFile
    {
        [JsonProperty("suspect")]
        public string Suspect { get; set; }

        [JsonProperty("gun")]
        public string Gun { get; set; }

        [JsonProperty("local")]
        public string Local { get; set; }
    }
}
