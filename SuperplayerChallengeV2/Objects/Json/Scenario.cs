using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace SuperplayerChallenge.Objects
{
    public class Scenario
    {
        [JsonProperty("suspects")]
        public string[] Suspects { get; set; }

        [JsonProperty("locals")]
        public string[] Locals { get; set; }

        [JsonProperty("guns")]
        public string[] Guns { get; set; }
    }
}
