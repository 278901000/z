using hk.papago.Entity.PaPaGoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using z.AdminCenter.Entity;
using z.Foundation.Data;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            IRepository repository = new NHibernateRepository();

            var adminSystem = repository.First<admin_system>(e => 1 == 1);

            var supplier = repository.First<supplier>(e => 1 == 1);

            var adminUser = repository.First<admin_user>(e => 1 == 1);

            return View();
        }

        public ActionResult Default()
        {
            IRepository repository = new NHibernateRepository();

            var adminSystem = repository.First<admin_system>(e => 1 == 1);

            var supplier = repository.First<supplier>(e => 1 == 1);

            var adminUser = repository.First<admin_user>(e => 1 == 1);
            
            return View();
        }
    }
}