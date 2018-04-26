using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BillboardsMVC5.Models;
using Microsoft.AspNet.Identity;

namespace BillboardsMVC5.Controllers
{
    public class HomeController : Controller
    {
        private BillboardContext _db = new BillboardContext();
        public ActionResult Index()
        {
            IEnumerable<Billboard> billboards = _db.Billboards;
            ViewBag.Billboards = billboards;
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


        public ActionResult More(string id)
        {
            try
            {
                ViewBag.Billboard = _db.Billboards.ToList().First(b => b.Id == id);
                ViewBag.TenantId = User.Identity.GetUserId();
                var rents = _db.Rents.ToList();
                var applications = _db.Applications.ToList();
                var rent = rents.FirstOrDefault(r => r.BillboardId == id);
                ViewBag.Status = "free";

                ViewBag.Application = "login";
                if (rent != null)
                {
                    ViewBag.EndDate = rent.DateFinish;
                    ViewBag.Status = "rented";
                    if (rent.TenantId == User.Identity.GetUserId())
                        ViewBag.Status = "owner";
                    ViewBag.Application = "disable";
                }

                if (User.Identity.GetUserId() != null)
                {
                    foreach (var application in applications)
                        if (application.BillboardId == id &&
                            application.TenantId == User.Identity.GetUserId())
                        {
                            ViewBag.Application = "made";
                            break;
                        }

                    if (ViewBag.Application == "login")
                        ViewBag.Application = "enable";
                }

            }
            catch (Exception e)
            {
                return View("Error");
            }

            return View("More");
        }

        [HttpPost]
        public ActionResult MakeApplication(string phoneNumber, string tenantId, string billboardId)
        {
            try
            {
                var application = new Application
                {
                    BillboardId = billboardId,
                    TenantId = tenantId,
                    ContactPhone = phoneNumber,
                    Status = "wait"
                };
                _db.Applications.Add(application);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return View("Error");
            }
            return More(billboardId);
        }

        [Authorize]
        public ActionResult MyBillboards()
        {
           _db.Billboards.ToList();
            var billboards = new List<Billboard>();
            var rents = _db.Rents.ToList();

            foreach (var billboard in _db.Billboards.ToList())
                if (billboard.TenantId == User.Identity.GetUserId())
                    billboards.Add(billboard);

            foreach (var billboard in billboards)
            {
                var rent = rents.FirstOrDefault(r => r.BillboardId == billboard.Id);

                if (rent != null)
                    billboard.EndRent = rent.DateFinish;
            }

            ViewBag.Billboards = billboards;
            ViewBag.Count = billboards.Count;
            return View("MyBillboards");
        }

    }
}