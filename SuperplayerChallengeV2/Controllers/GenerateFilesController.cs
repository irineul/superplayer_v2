using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Text;
using SuperplayerChallenge.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace BootstrapMvcSample.Controllers
{
    public class GenerateFilesController : BootstrapBaseController
    {
        #region Views
        [HttpGet]
        public ActionResult Scenario()
        {
            this.TempData["ScenarioGridItems"] = null;
            return View();
        }

        [HttpGet]
        public ActionResult Witness()
        {
            this.TempData["ScenarioGridItems"] = null;
            this.TempData["WitnessGridItems"] = null;
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        #endregion

        #region Scenario
        [HttpPost]
        public string AddScenarioItem(string item, string dataType)
        {
            ResponseFileDownload data = new ResponseFileDownload();

            /* Get current grid */
            if (this.TempData["ScenarioGridItems"] != null)
            {
                data = (ResponseFileDownload)this.TempData["ScenarioGridItems"];
            }
            else
            {
                data.Guns = new List<string>();
                data.Locals = new List<string>();
                data.Suspects = new List<string>();
            }

            if (item != null)
            {
                return ScenarioGridGenerator(item, dataType, data);
            }
            else
            {
                return "";
            }

        }

        [HttpPost]
        public string DeleteScenarioItem(string idItem, string dataType)
        {
            ResponseFileDownload data = new ResponseFileDownload();

            /* Get current grid */
            if (this.TempData["ScenarioGridItems"] != null)
            {
                data = (ResponseFileDownload)this.TempData["ScenarioGridItems"];

                /* Remove suspect */
                if (dataType.Equals("S"))
                {
                    data.Suspects.Remove(idItem);
                }

                /* Remove local */
                else if (dataType.Equals("L"))
                {
                    data.Locals.Remove(idItem);
                }

                /* Remove gun */
                else if (dataType.Equals("G"))
                {
                    data.Guns.Remove(idItem);
                }
                return ScenarioGridGenerator("", dataType, data);
            }
            else
            {
                return "";
            }


        }

        private string ScenarioGridGenerator(string item, string dataType, ResponseFileDownload data)
        {

            StringBuilder sb = new StringBuilder();

            /* Generate table header */
            sb.AppendLine("<table class=\"table table-hover\">");
            sb.AppendLine(" <thead>");
            sb.AppendLine("   <tr>");
            sb.AppendLine("     <th>#</th>");
            sb.AppendLine("     <th>Nome</th>");
            sb.AppendLine("     <th>Excluir</th>");
            sb.AppendLine("   </tr>");
            sb.AppendLine(" </thead>");
            sb.AppendLine(" <tbody>");



            /* Suspect */
            if (dataType.Equals("S"))
            {
                if (item != "")
                    data.Suspects.Add(item);

                /* Render Suspects */
                for (int i = 0; i < data.Suspects.Count(); i++)
                {
                    sb.AppendLine("     <tr>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(i.ToString());
                    sb.AppendLine("         </td>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(data.Suspects[i]);
                    sb.AppendLine("         </td>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(string.Concat("<a href=\"javascript:DeleteScenarioItem('", data.Suspects[i].ToString(), "','", "S", "');\"><img src=\"" + Url.Content("~/Content/img/glyphicons_197_remove.png") + "\"></a>"));
                    sb.AppendLine("         </td>");

                }
            }
            /* Local */
            if (dataType.Equals("L"))
            {
                if (item != "")
                    data.Locals.Add(item);

                /* Render Locals */
                for (int i = 0; i < data.Locals.Count(); i++)
                {
                    sb.AppendLine("     <tr>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(i.ToString());
                    sb.AppendLine("         </td>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(data.Locals[i]);
                    sb.AppendLine("         </td>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(string.Concat("<a href=\"javascript:DeleteScenarioItem('", data.Locals[i].ToString(), "','", "L", "');\"><img src=\"" + Url.Content("~/Content/img/glyphicons_197_remove.png") + "\"></a>"));
                    sb.AppendLine("         </td>");

                }
            }
            /* Suspect */
            if (dataType.Equals("G"))
            {
                if (item != "")
                    data.Guns.Add(item);

                /* Render Guns */
                for (int i = 0; i < data.Guns.Count(); i++)
                {
                    sb.AppendLine("     <tr>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(i.ToString());
                    sb.AppendLine("         </td>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(data.Guns[i]);
                    sb.AppendLine("         </td>");
                    sb.AppendLine("         <td>");
                    sb.AppendLine(string.Concat("<a href=\"javascript:DeleteScenarioItem('", data.Guns[i].ToString(), "','", "G", "');\"><img src=\"" + Url.Content("~/Content/img/glyphicons_197_remove.png") + "\"></a>"));
                    sb.AppendLine("         </td>");

                }
            }

            this.TempData["ScenarioGridItems"] = data;


            sb.AppendLine(" </tbody>");
            sb.AppendLine("</tbody>");

            return sb.ToString();
        }

        [HttpPost]
        public ActionResult GenerateScenarioJsonFile()
        {
            /* Get the scenario model to save into a file */
            ResponseFileDownload fileToDownload = new ResponseFileDownload();

            /* Get current grid */
            if (this.TempData["ScenarioGridItems"] != null)
            {
                fileToDownload = (ResponseFileDownload)this.TempData["ScenarioGridItems"];
            }
            
            this.TempData["ScenarioGridItems"] = fileToDownload;

            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(@"c:\temp\data"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, fileToDownload);
                }
            }
            catch (Exception e)
            {
                Error("Ocorreu um erro ao gerar o arquivo: " + e.Message);
                return View("Scenario");
            }
            Success("Arquivo de cenário gerado com sucesso na pasta C:\\TEMP!");
            return View("Scenario");
        }
        #endregion

        #region Witness
        [HttpPost]
        public string AddWitnessItem(string suspect, string local, string gun)
        {
            if (suspect != null && local != null && gun != null)
            {
                WitnessFileDownload response = new WitnessFileDownload();
                /* Get current grid */
                if (this.TempData["WitnessGridItems"] != null)
                {
                    response = (WitnessFileDownload)this.TempData["WitnessGridItems"];
                }
                else
                {
                    response.Scenario = new List<ResponseFile>();
                }

                return WitnessGridGenerator(suspect, local, gun, response);
            }
            else
            {
                return "";
            }

        }

        [HttpPost]
        public string DeleteWitnessItem(string idItem)
        {
            WitnessFileDownload response = new WitnessFileDownload();

            /* Get current grid */
            if (this.TempData["WitnessGridItems"] != null && idItem != null)
            {
                response = (WitnessFileDownload)this.TempData["WitnessGridItems"];

                response.Scenario.RemoveAt(int.Parse(idItem));
                return WitnessGridGenerator(string.Empty, string.Empty, string.Empty, response);
            }
            else
            {
                return "";
            }


        }

        private string WitnessGridGenerator(string suspect, string local, string gun, WitnessFileDownload response)
        {

            StringBuilder sb = new StringBuilder();

            /* Generate table header */
            sb.AppendLine("<table class=\"table table-hover\">");
            sb.AppendLine(" <thead>");
            sb.AppendLine("   <tr>");
            sb.AppendLine("     <th>#</th>");
            sb.AppendLine("     <th>Suspeito</th>");
            sb.AppendLine("     <th>Local</th>");
            sb.AppendLine("     <th>Arma</th>");
            sb.AppendLine("     <th>Excluir</th>");
            sb.AppendLine("   </tr>");
            sb.AppendLine(" </thead>");
            sb.AppendLine(" <tbody>");



            if (suspect != string.Empty && local != string.Empty && gun != string.Empty)
            {
                response.Scenario.Add(new ResponseFile() { Suspect = suspect, Local = local, Gun = gun });
            }

            for (int i = 0; i < response.Scenario.Count(); i++)
            {
                sb.AppendLine("     <tr>");
                sb.AppendLine("         <td>");
                sb.AppendLine(i.ToString());
                sb.AppendLine("         </td>");
                sb.AppendLine("         <td>");
                sb.AppendLine(response.Scenario[i].Suspect);
                sb.AppendLine("         </td>");
                sb.AppendLine("         <td>");
                sb.AppendLine(response.Scenario[i].Local);
                sb.AppendLine("         </td>");
                sb.AppendLine("         <td>");
                sb.AppendLine(response.Scenario[i].Gun);
                sb.AppendLine("         </td>");
                sb.AppendLine("         <td>");
                sb.AppendLine(string.Concat("<a href=\"javascript:DeleteWitnessItem('", i, "','", "S", "');\"><img src=\"" + Url.Content("~/Content/img/glyphicons_197_remove.png") + "\"></a>"));
                sb.AppendLine("         </td>");


            }

            this.TempData["WitnessGridItems"] = response;

            sb.AppendLine(" </tbody>");
            sb.AppendLine("</tbody>");

            return sb.ToString();
        }

        [HttpPost]
        public ActionResult GenerateWitnessJsonFile()
        {
            /* Get the scenario model to save into a file */
            WitnessFileDownload fileToDownload = new WitnessFileDownload();

            /* Get current grid */
            if (this.TempData["WitnessGridItems"] != null)
            {
                fileToDownload = (WitnessFileDownload)this.TempData["WitnessGridItems"];
            }
            this.TempData["WitnessGridItems"] = fileToDownload;

            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(@"c:\temp\response"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, fileToDownload);
                }
            }
            catch (Exception e)
            {
                Error("Ocorreu um erro ao gerar o arquivo: " + e.Message);
                return View("Scenario");
            }

            Success("Arquivo de testemunhas gerado com sucesso na pasta C:\\TEMP!");
            return View("Witness");
        }
        #endregion
    }
}
