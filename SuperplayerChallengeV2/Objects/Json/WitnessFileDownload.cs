using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace SuperplayerChallenge.Objects
{
    public class WitnessFileDownload
    {

        [JsonProperty("responses")]
        public List<ResponseFile> Scenario { get; set; }

    }
}
