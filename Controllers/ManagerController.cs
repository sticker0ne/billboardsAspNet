using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BillboardsMVC5.Models;
using Microsoft.AspNet.Identity;

namespace BillboardsMVC5.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private BillboardContext _db = new BillboardContext();
        // GET: Manager
        public ActionResult Index()
        {
            var users = _db.Users.ToList();
            var applications = _db.Applications.ToList();
            var billboards = _db.Billboards.ToList();
            foreach (var app in applications)
            {
                var mail = users.First(u => u.Id == app.TenantId).Email;
                var desc = billboards.First(b => b.Id == app.BillboardId).Description;
                app.TenantMail = mail;
                app.BillboardDesc = desc;
            }

            ViewBag.Applications = applications;
            return View("Index");
        }

        public ActionResult ChangeApplication(string appId, string tenantId, string billboardId, 
            string status)
        {
            try
            {
                var applications = _db.Applications.ToList();
          
                var stat = Convert.ToBoolean(status);
                if (stat)
                {
                    foreach (var application in applications)
                        if (application.BillboardId == billboardId)
                            _db.Applications.Remove(application);
                    _db.Billboards.First(b => b.Id == billboardId).TenantId = tenantId;

                    var rent = new Rent
                    {
                        BillboardId = billboardId,
                        ManagerId = User.Identity.GetUserId(),
                        TenantId = tenantId,
                        DateStart = DateTime.Today,
                        DateFinish = DateTime.Today.AddMonths(1)
                    };
                    _db.Rents.Add(rent);
                }
                else
                {
                    var app = applications.First(a => a.Id == appId);
                    _db.Applications.Remove(app);
                }

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return View("Error");
            }

            return Index();
        }
    }
}