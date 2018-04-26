using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BillboardsMVC5.Models;
using System.IO;
using System.Runtime.Versioning;
using Microsoft.Ajax.Utilities;

namespace BillboardsMVC5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private BillboardContext _db = new BillboardContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EditBillboard()
        {
            IEnumerable<Billboard> billboards = _db.Billboards;
            ViewBag.Billboards = billboards;
            return View();
        }

        public ActionResult EditUser()
        {
            var managerRoleId = _db.Roles.First(r => r.Name == "Manager").Id;
            var userRoles = _db.UserRoles.ToList();
            var users = _db.Users.ToList();

            foreach (var u in users)
                if (userRoles.FindAll(ur => ur.UserId == u.Id)
                    .Exists(ur => ur.RoleId == managerRoleId))
                    u.IsManager = true;
                else
                    u.IsManager = false;
            ViewBag.Users = _db.Users.ToList();
            return View("EditUser");
        }

        [HttpPost]
        public ActionResult ChangeBillboard(HttpPostedFileBase imgFile, string desc, string tenant,
            string lat, string lng, string height, string width, string cost, string id, string type)
        {
            try
            {
                switch (type)
                {
                    case "delete":
                    {

                        Billboard billboard = _db.Billboards.First(b => b.Id == id);
                        _db.Billboards.Remove(billboard);
                        _db.SaveChanges();
                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Resources"), billboard.Id + ".png")))
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Resources"), billboard.Id + ".png"));
                    }
                        break;

                    case "create":
                    {

                        Billboard billboard = new Billboard
                        {
                            Description = desc,
                            TenantId = tenant,
                            Cost = Convert.ToInt32(cost),
                            Height = Convert.ToDouble(height),
                            Width = Convert.ToDouble(width),
                            Lat = (float) Convert.ToDouble(lat),
                            Long = (float) Convert.ToDouble(lng)
                        };

                        _db.Billboards.Add(billboard);
                        _db.SaveChanges();

                        if (imgFile != null)
                        {
                            imgFile.SaveAs(Path.Combine(Server.MapPath("~/Resources"), billboard.Id + ".png"));
                            billboard.ImageName = billboard.Id + ".png";
                        }

                        _db.SaveChanges();


                    }
                        break;

                    case "save":
                    {
                        var rents = _db.Rents.ToList();
                        var billboard = _db.Billboards.First(b => b.Id == id);
                        billboard.Description = desc;
                        billboard.TenantId = tenant;
                        billboard.Cost = Convert.ToInt32(cost);
                        billboard.Height = Convert.ToDouble(height);
                        billboard.Width = Convert.ToDouble(width);
                        billboard.Lat = (float) Convert.ToDouble(lat);
                        billboard.Long = (float) Convert.ToDouble(lng);

                        if (imgFile != null)
                        {
                            imgFile.SaveAs(Path.Combine(Server.MapPath("~/Resources"), billboard.Id + ".png"));
                            billboard.ImageName = billboard.Id + ".png";
                        }

                        if (tenant.IsNullOrWhiteSpace())
                        {
                            var rent = rents.FirstOrDefault(r => r.BillboardId == id);
                            if (rent != null)
                                _db.Rents.Remove(rent);
                        }

                        _db.SaveChanges();

                    }
                        break;
                }
            }
            catch (Exception e)
            {
                return View("Error");
            }

            ViewBag.Billboards = _db.Billboards;
            return View("EditBillboard");
        }

        [HttpPost]
        public ActionResult ChangeUser(string userId, string status)
        {
            try
            {
                var managerRoleId = _db.Roles.First(r => r.Name == "Manager").Id;
                var userRoles = _db.UserRoles.ToList();

                var stat = Convert.ToBoolean(status);
                if (stat)
                {
                    var ur = userRoles.First(u => u.UserId == userId);
                    _db.UserRoles.Remove(ur);
                }
                else
                {
                    var ur = new UserRole {UserId = userId, RoleId = managerRoleId};
                    _db.UserRoles.Add(ur);
                }

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return View("Error");
            }

            return EditUser();
        }
    }
}