using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.apicenter.www.Controllers
{
    public class HomeController : ApiBaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}