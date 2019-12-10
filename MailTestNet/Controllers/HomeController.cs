using Bsixmail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;

namespace MailTestNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mailer = new BsixMailer();
            var _Base = "https://yourdomain.com/controller/action"; // or ur webform service;
            mailer.BuildBody(new
            {
                Message = "hello from bsix aproval",
                UrlApprove = string.Format("{0}/{1}/{2}", _Base, "Approve", "yes"),
                UrlReject = string.Format("{0}/{1}/{2}", _Base, "Approve", "no")
            }, "default.txt") // <--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .Send();

            // example
            mailer.BuildBody(new
            {
                Message = "hello from bsix"
            }, "default.txt")  // <--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .SendBackground(); // <--- background task email
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}