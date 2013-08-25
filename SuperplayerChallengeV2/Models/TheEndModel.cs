using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperplayerChallenge.Objects;
namespace Models
{
    public class TheEndModel
    {
        public List<Killer> Killers { get; set; }
        public Detective Detective { get; set; }
    }
}