using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using L_.Models;
using SoERP.Models;
using System.Data;
using System.Web.Security;

namespace SoERP.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/
        ErpDBEntities db = new ErpDBEntities();
        public ActionResult Manager()
        {
            int id = int.Parse(Session["uid"].ToString());
            Users s = (from item in db.Users where item.Usersid == id select item).FirstOrDefault();
            ViewBag.uname = s.Usersname;
            ViewBag.imgs = s.Image;
            ViewBag.repwd = s.Users_password;
            ViewBag.uid = id;
            return View();
        }
        [HttpPost]
        public ActionResult update(Users user, HttpPostedFileBase file)
        {
            if (file == null)
            {
                Users emp = db.Users.Find(int.Parse(Request["useid"]));
                if (emp != null)
                {
                    emp.Usersname = Request["uname"];
                    emp.Users_password = Request["usspwd"];
                }
                if (db.SaveChanges() > 0)
                {
                    return Content("<script>parent.window.location.href='/Personnel/Login'</script>");
                }
                else
                {
                    return Content("<script>alert('修改失败！')</script>");
                }
            }
            else
            {
                Users emp = db.Users.Find(int.Parse(Request["useid"]));
                emp.Usersname = Request["uname"];
                emp.Users_password = Request["usspwd"];
                emp.Image = "/images/" + Request["empimg"];
                db.Entry<Users>(emp).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    file.SaveAs(Server.MapPath("~/images/") + file.FileName);
                    return Content("<script>parent.window.location.href='/Personnel/Login'</script>");
                }
                else
                {
                    return Content("<script>alert('修改失败！')</script>");
                }
            }
        }
        public ActionResult Message()
        {
            List<Goods_inventory> sal = db.Goods_inventory.ToList();
            var salList = sal
                .Select(e => new { e.Name, e.Inventory_type, e.Inventory })
                .Where(e => e.Inventory_type == "商品" && e.Inventory < 10)
                .Select(item => Tuple.Create(
                        item.Name,
                        item.Inventory_type))
                .ToList();
            ViewBag.sal = salList;
            List<Goods_inventory> cal = db.Goods_inventory.ToList();
            var calList = cal
                .Select(e => new { e.Name, e.Inventory_type, e.Inventory })
                .Where(e => e.Inventory_type == "产品" && e.Inventory < 10)
                .Select(item => Tuple.Create(
                        item.Name,
                        item.Inventory_type))
                .ToList();
            ViewBag.cal = calList;
            return View();
        }
        [HttpPost]
        public ActionResult Miid(string hlogo, string hleft, string htop)
        {
            if (Session["uid"] != null)
            {
                int id = int.Parse(Session["uid"].ToString());
                if (id != 0)
                {
                    Dermal der = (from item in db.Dermal where item.Usersid == id select item).FirstOrDefault();
                    der.Dermal_tname = htop;
                    der.Dermal_lname = hleft;
                    der.Dermal_gname = hlogo;
                    db.Entry<Dermal>(der).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Session["usrName"] = null;
                Session["uid"] = null;
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
