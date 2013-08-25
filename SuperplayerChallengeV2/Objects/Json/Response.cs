using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;
using Newtonsoft.Json;

namespace SuperplayerChallenge.Objects
{
    public class Response
    {
        [JsonProperty("responses")]
        public ResponseFile[] Responses;
    }
}
