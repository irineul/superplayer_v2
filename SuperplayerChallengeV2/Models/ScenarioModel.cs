using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperplayerChallenge.Objects;
namespace Models
{
    public class ScenarioModel
    {
        public List<Gun> Guns { get; set; }
        public List<Suspect> Suspects { get; set; }
        public List<Local> Locals { get; set; }
        public List<Witness> Witness { get; set; }

    }
}