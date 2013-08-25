using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SuperplayerChallenge.Objects
{
    public class Killer
    {
        public Gun Gun { get; set; }
        public Local Local { get; set; }
        public Suspect Suspect { get; set; }

    }
}
