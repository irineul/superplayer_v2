using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using SuperplayerChallenge.Objects;
using BootstrapMvcSample.Controllers;
using Models;
using Business;
using System.Text;

namespace SuperplayerChallenge.Controllers
{
    public class ChallengeController : BootstrapBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                // get contents to string
                string str = (new StreamReader(file.InputStream)).ReadToEnd();

                // deserializes string into object
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var d = jss.Deserialize<dynamic>(str);


            }
            Success("Your information was saved!");
            return View();
        }

        [HttpPost]
        public ActionResult Init(HttpPostedFileBase fileData, HttpPostedFileBase fileWitness)
        {
            ScenarioModel scenario = new ScenarioModel();
            List<Witness> witness = new List<Witness>();
            if (fileData != null && fileData.ContentLength > 0)
            {
                scenario = ChallengeSolverBusiness.GetInstance().DeserializeJsonFileScenario(fileData.InputStream);
                this.TempData["Scenario"] = scenario;
            }
            else
            {
                Error("Arquivo de cenário inválido!");
                return View(scenario);
            }

            if (fileWitness != null && fileWitness.ContentLength > 0)
            {
                witness = ChallengeSolverBusiness.GetInstance().DeserializeJsonFileWitness(fileWitness.InputStream, scenario);
                if (witness == null || witness.Count == 0)
                {
                    Error("Arquivo de vitímas inválido!");
                    return View(scenario);
                }
                else
                {
                    scenario.Witness = witness;
                    this.TempData["Witness"] = witness;
                }
            }
            else
            {
                Error("Arquivo de vitímas inválido!");
                return View(scenario);
            }

            Success("Arquivo carregado com sucesso!");
            return View(scenario);
        }

        [HttpPost]
        public ActionResult Finish()
        {
            ScenarioModel scenario = this.TempData["Scenario"] != null ? (ScenarioModel) this.TempData["Scenario"] : null;
            List<Witness> witness = this.TempData["Witness"] != null ? (List<Witness>)this.TempData["Witness"] : null;


            TheEndModel theEnd = ChallengeSolverBusiness.GetInstance().FindWhoIsTheKiller(scenario, witness);

            
            if (theEnd == null || theEnd.Killers.Count == 0)
            {
                Error("Assassino não encontrado!");
            }
            else
            {
                Success("Assassino(s) encontrado(s)!");
            }


            return View(theEnd);
        }
    }
}

