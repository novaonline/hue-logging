using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HueLogging.Web.Controllers
{
    public class FocusOnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}