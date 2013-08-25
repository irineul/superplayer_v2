using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace BootstrapMvcSample.Controllers
{
    public class InstructionsController : BootstrapBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
