using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using L_.Models;
using SoERP.Models;
using System.Data;
using System.Web.Security;
using SoERP.Controllers;

namespace SoERP.Controllers
{
    public class PersonnelController : Controller
    {
        //
        // GET: /Personnel/
        //实例化上下文类
        ErpDBEntities db = new ErpDBEntities();
        LazySingleton lazysingleton = LazySingleton.GetInstance();
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
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
            if (salList.Count() != 0 || calList.Count() != 0)
            {
                Session["pd"] = 1;
            }
            else
            {
                Session["pd"] = 0;
            }
            int id = int.Parse(Session["uid"].ToString());
            Users s = (from item in db.Users where item.Usersid == id select item).FirstOrDefault();
            ViewBag.imgs = s.Image;
            Dermal der = (from item in db.Dermal where item.Usersid == id select item).FirstOrDefault();
            ViewBag.lname = der.Dermal_lname;
            ViewBag.tname = der.Dermal_tname;
            ViewBag.gname = der.Dermal_gname;  
            return View();
        }

        /// /// <summary> 首页的内容页
        /// 首页的内容页
        /// </summary>
        /// <returns></returns>//进入公告视图
        [Authorize]
        public ActionResult IndexConten()
        {
            ViewBag.sal = DAL.DALcs.selsal() + "万";
            ViewBag.pur = DAL.DALcs.selpur() + "万";
            ViewBag.ord = DAL.DALcs.selord() + "个";
            ViewBag.emp = DAL.DALcs.selemp() + "人";
            ViewBag.mar = DAL.DALcs.selmar();
            ViewBag.apr = DAL.DALcs.selapr();
            ViewBag.may = DAL.DALcs.selmay();
            ViewBag.jun = DAL.DALcs.seljun();
            ViewBag.ram = DAL.DALcs.selram();
            ViewBag.rpa = DAL.DALcs.selrpa();
            ViewBag.yam = DAL.DALcs.selyam();
            ViewBag.nuj = DAL.DALcs.selnuj();
            ViewBag.fis = DAL.DALcs.fis();
            ViewBag.fname = DAL.DALcs.fisname();
            ViewBag.sec = DAL.DALcs.sec();
            ViewBag.sname = DAL.DALcs.secname();
            ViewBag.thi = DAL.DALcs.thi();
            ViewBag.tname = DAL.DALcs.thiname();
            return View();
        }
        /**************************************登录模块********************************************/
        #region 登录
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string uname, string upwd)
        {
            //2021-11-15 wenly 新增登录try
            try
            {
                uname = Request["username"];
                upwd = Request["userpwd"];
                Users s = (from item in db.Users where item.Usersname == uname select item).FirstOrDefault();
                //if()
                var id = s.Usersid;
                List<Users> model = db.Users.Where(a => a.Usersname == uname && a.Users_password == upwd).ToList();
                if (model.Count() == 0)
                {
                    ViewBag.info = "用户名或密码错误！";
                    return View("Login");
                }
                else
                {
                    System.Web.HttpContext.Current.Session["usrName"] = uname; //将用户名放入session中
                    System.Web.HttpContext.Current.Session["uid"] = id; //将用户ID放入session中
                    System.Web.HttpContext.Current.Session["post"] = db.Users.Find(id).Post;
                    System.Web.HttpContext.Current.Session["department"] = db.Users.Find(id).Department;
                    FormsAuthentication.SetAuthCookie(uname, false);
                    return Content("<script>window.location.href='/Personnel/Index'</script>");
                }
            }
            catch (Exception)
            {
                ViewBag.info = "用户名或密码错误！";
                return View("Login");
            }           
        }
        #endregion
        /**************************************企业公告模块********************************************/
        #region 企业公告
        /// <summary> 详情
        /// 详情
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCenten(int id)
        {
            Announcements model = db.Announcements.Single(a => a.Annountid == id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 企业管理-企业公告
        /// 企业管理-企业公告
        /// </summary>
        /// <returns></returns>        
        [Authorize]
        public ActionResult BusinessManagementCorporateAnnouncements()
        {
            List<Announcements> ann = db.Announcements.OrderByDescending(a => a.Annountid).ToList();
            ViewBag.ann = ann;
            return View();
        }
        /// <summary> 搜索指定项
        /// 搜索指定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Selbus(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Announcements> model = db.Announcements.OrderByDescending(a => a.Annountid).Where(a => a.Announttitle.Contains(name)).ToList();
            ViewBag.ann = model;
            return View("BusinessManagementCorporateAnnouncements");//此视图为模糊查询所在的页面
        }
        #endregion
        /**************************************人事管理模块********************************************/
        #region 人事管理
        /// <summary> 企业管理-人事管理
        /// 企业管理-人事管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult BusinessManagementPersonnelManagement()
        {
            List<Employees> emp = db.Employees.ToList();
            var empList = emp
                .Select(e => new { e.EmployeesId, e.Name, e.Gender, e.Age, e.Department1.Departmentname, e.Department1.DepartmentReadname, e.Phone, e.Email, e.Position, e.Nationals,e.Domicile, e.Address, e.IdNumber, e.Workingtime, e.Photo })
                .OrderByDescending(e => e.EmployeesId)
                .Select(item => Tuple.Create(
                    item.EmployeesId,
                    item.Name,
                    item.Gender,
                    item.Age,
                    item.Departmentname,
                    item.Phone,
                    item.Email,
                    item.Position
                    ))
                .ToList();
            ViewBag.emp = empList;
            List<Department> dep = db.Department.ToList();
            ViewBag.dep = dep;
            return View();
        }
        /// <summary> 添加员工
        /// 添加员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult insemp(Employees emp, HttpPostedFileBase file)
        {
            emp.Name = Request["empname"];
            emp.Domicile = Request["emph"];
            emp.Age = int.Parse(Request["empage"]);
            emp.Address = Request["empadd"];
            emp.IdNumber = Request["empidc"];
            emp.Position = Request["empppro"];
            emp.Gender = Request["empsex"];
            emp.Nationals = Request["empmz"];
            emp.Department = int.Parse(Request["emppm"]);
            emp.Phone = Request["empphe"];
            emp.Workingtime = DateTime.Parse(Request["example-datepicker"]);
            emp.Email = Request["empeml"];
            if (file != null)
            {
                emp.Photo = "/images/" + Request["empimg"];
                file.SaveAs(Server.MapPath("~/images/") + file.FileName);
            }
            db.Employees.Add(emp);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("BusinessManagementPersonnelManagement");
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        /// <summary> 编辑员工
        /// 编辑员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updemp(HttpPostedFileBase file)
        {
            Employees emp = db.Employees.Find(int.Parse(Request["empidup"]));
            emp.Name = Request["empname"];
            emp.Domicile = Request["emph"];
            emp.Age = int.Parse(Request["empage"]);
            emp.Address = Request["empadd"];
            emp.IdNumber = Request["empidc"];
            emp.Position = Request["empppro"];
            emp.Gender = Request["empsex"];
            emp.Nationals = Request["empmz"];
            emp.Department = int.Parse(Request["emppm"]);
            emp.Phone = Request["empphe"];
            emp.Workingtime = DateTime.Parse(Request["example-datepicker"]);
            emp.Email = Request["empeml"];
            if (file != null)
            {
                emp.Photo = "/images/" + Request["empimg"];
                file.SaveAs(Server.MapPath("~/images/") + file.FileName);
            }
            db.Entry<Employees>(emp).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("BusinessManagementPersonnelManagement");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary> 查询员工详情
        /// 查询员工详情
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public ActionResult selupdid(int empid)
        {
            Employees model = db.Employees.Single(a => a.EmployeesId == empid);
            var data = new
            {
                EmployeesId = model.EmployeesId,
                Name = model.Name,
                Domicile = model.Domicile,
                Age = model.Age,
                Address = model.Address,
                IdNumber = model.IdNumber,
                Position = model.Position,
                Gender = model.Gender,
                Nationals = model.Nationals,
                Department = model.Department,
                Phone = model.Phone,
                Workingtime = model.Workingtime,
                Email = model.Email,
                Photo = model.Photo
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 删除员工
        ///删除员工
        /// </summary>
        /// <returns></returns>
        public ActionResult delemp(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                Employees emp = db.Employees.Find(emp_id);
                db.Employees.Remove(emp);
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 搜索指定项
        /// 搜索指定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Selempsup(string name)
        {
            name = Request["tj"];
            List<Employees> emp = db.Employees.ToList();
            var empList = emp
                .Select(e => new { e.EmployeesId, e.Name, e.Gender, e.Age, e.Department1.Departmentname, e.Department1.DepartmentReadname, e.Phone, e.Email, e.Position, e.Nationals, e.Domicile, e.Address, e.IdNumber, e.Workingtime, e.Photo })
                .Where(e=>e.Name.Contains(name))
                .OrderByDescending(e => e.EmployeesId)
                .Select(item => Tuple.Create(
                    item.EmployeesId,
                    item.Name,
                    item.Gender,
                    item.Age,
                    item.Departmentname,
                    item.Phone,
                    item.Email,
                    item.Position
                    ))
                .ToList();
            ViewBag.emp = empList;
            List<Department> dep = db.Department.ToList();
            ViewBag.dep = dep;
            return View("BusinessManagementPersonnelManagement");
        }
        #endregion
        /**************************************薪资管理模块********************************************/
        #region 薪资管理
        /// <summary> 企业管理-薪资管理
        /// 企业管理-薪资管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult BusinessManagementSalaryManagement()
        {
            List<Salary> sal = db.Salary.ToList();
            var salList = sal
                .Select(e => new { e.SalaryId, e.Employees.Department1.Departmentname, e.Employees.Name, e.Base_salary, e.Total_amount, e.Issuing_date, e.Thetax_amount })
                .OrderByDescending(e => e.SalaryId)
                .Select(item => Tuple.Create(
                    item.SalaryId,
                    item.Departmentname,
                    item.Name,
                    item.Base_salary,
                    item.Total_amount,
                    item.Issuing_date,
                    item.Thetax_amount))
                .ToList();
            ViewBag.sal = salList;
            List<Employees> dep = db.Employees.ToList();
            ViewBag.dep = dep;
            return View();
        }
        /// <summary> 添加员工薪资
        /// 添加员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult inssal(Salary emp)
        {
            emp.EmployeesId = int.Parse(Request["salemppm"]);
            emp.Salary_stardate = DateTime.Parse(Request["saldate"]);
            emp.Bankname = Request["salBankname"];
            emp.Base_salary = int.Parse(Request["salBas"]);
            emp.Lunch_subsidies = int.Parse(Request["salLun"]);
            emp.Housing_subsidies = int.Parse(Request["salHous"]);
            emp.Taken = int.Parse(Request["salTak"]);
            emp.Additional_benefits = int.Parse(Request["salAdd"]);
            emp.The_fines = int.Parse(Request["salThe"]);
            emp.Thetax_amount = int.Parse(Request["salTot"]);
            emp.Issuing_date = DateTime.Parse(Request["salsdate"]);
            emp.Salary_account = Request["salSal"];
            emp.Total_amount = emp.Base_salary + emp.Lunch_subsidies + emp.Housing_subsidies + emp.Taken + emp.Additional_benefits - emp.Thetax_amount - emp.The_fines;
            db.Salary.Add(emp);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("BusinessManagementSalaryManagement");               
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        /// <summary> 编辑员工薪资
        /// 编辑员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updsal()
        {
            Salary emp = db.Salary.Find(int.Parse(Request["empidup"]));
            emp.EmployeesId = int.Parse(Request["ssalemppm"]);
            emp.Salary_stardate = DateTime.Parse(Request["ssaldate"]);
            emp.Bankname = Request["ssalBankname"];
            emp.Base_salary = int.Parse(Request["ssalBas"]);
            emp.Lunch_subsidies = int.Parse(Request["ssalLun"]);
            emp.Housing_subsidies = int.Parse(Request["ssalHous"]);
            emp.Taken = int.Parse(Request["ssalTak"]);
            emp.Additional_benefits = int.Parse(Request["ssalAdd"]);
            emp.The_fines = int.Parse(Request["ssalThe"]);
            emp.Thetax_amount = int.Parse(Request["ssalThet"]);
            emp.Issuing_date = DateTime.Parse(Request["ssalsdate"]);
            emp.Salary_account = Request["ssalSal"];
            emp.Total_amount = emp.Base_salary + emp.Lunch_subsidies + emp.Housing_subsidies + emp.Taken + emp.Additional_benefits - emp.Thetax_amount - emp.The_fines;
            db.Entry<Salary>(emp).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
            return RedirectToAction("BusinessManagementSalaryManagement");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary> 查询员工薪资详情
        /// 查询员工详情
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public ActionResult selsalid(int empid)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            Salary model = db.Salary.Single(a => a.SalaryId == empid);
            var data = new
            {
                SalaryId = model.SalaryId,
                EmployeesId = model.EmployeesId,
                Base_salary = model.Base_salary,
                Lunch_subsidies = model.Lunch_subsidies,
                Taken = model.Taken,
                The_fines = model.The_fines,
                Housing_subsidies = model.Housing_subsidies,
                Additional_benefits = model.Additional_benefits,
                Thetax_amount = model.Thetax_amount,
                Total_amount = model.Total_amount,
                Issuing_date = model.Issuing_date,
                Bankname = model.Bankname,
                Salary_account = model.Salary_account,
                Salary_stardate = model.Salary_stardate,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 删除员工薪资
        ///删除员工
        /// </summary>
        /// <returns></returns>
        public ActionResult delsal(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                Salary emp = db.Salary.Find(emp_id);
                db.Salary.Remove(emp);
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 搜索指定项
        /// 搜索指定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Selsalsup(string name)
        {
            name = Request["tj"];
            List<Salary> sal = db.Salary.ToList();
            var salList = sal
                .Select(e => new { e.SalaryId, e.Employees.Department1.Departmentname, e.Employees.Name, e.Base_salary, e.Total_amount, e.Issuing_date, e.Thetax_amount })
                .Where(e => e.Name.Contains(name))
                .OrderByDescending(e => e.SalaryId)
                .Select(item => Tuple.Create(
                    item.SalaryId,
                    item.Departmentname,
                    item.Name,
                    item.Base_salary,
                    item.Total_amount,
                    item.Issuing_date,
                    item.Thetax_amount))
                .ToList();
            ViewBag.sal = salList;
            List<Employees> dep = db.Employees.ToList();
            ViewBag.dep = dep;
            return View("BusinessManagementSalaryManagement");
        }
        #endregion
        /// <summary>
        /// 客户管理-客户资料
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerInfo()
        {
            //查询客户资料
            var Customerlist = db.Customer
                .OrderByDescending(p=>p.CustomerId)
                .ToList();
            ViewBag.Customer = Customerlist;
            return View();
        }
        /// <summary>
        /// 模糊查询客户资料
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerInfoLike(string name)
        {
            name = Request["info"];//前端input传过来的值
            List<Customer> model = db.Customer.OrderByDescending(a => a.CustomerId).Where(a => a.Customername.Contains(name)).ToList();
            ViewBag.Customer = model;
            return View("CustomerManagerInfo");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据id查询客户资料
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerInfoById()
        {
            //获取传输过来的商品id
            var Customerid = Request["Customer"];
            var Customerinfo = db.Customer.Find(int.Parse(Customerid));
            //返回数据
            var obj = new
            {
                CustomerId = Customerinfo.CustomerId,
                Customername = Customerinfo.Customername,
                Zip_code = Customerinfo.Zip_code,
                Phone = Customerinfo.Phone,
                Address = Customerinfo.Address,
                Fax = Customerinfo.Fax,
                Contact = Customerinfo.Contact,
                Concat_phone = Customerinfo.Concat_phone,
                Email = Customerinfo.Email,
                Bank = Customerinfo.Bank,
                BankId = Customerinfo.BankId
            };
            return Json(obj, JsonRequestBehavior.AllowGet); 
        }
        /// <summary>
        /// 修改客户资料信息
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerInfoUpdate(Customer customer)
        {
            //查询客户资料信息
            var Customer = db.Customer.Find(customer.CustomerId);
            Customer.Customername = customer.Customername;
            Customer.Zip_code = customer.Zip_code;
            Customer.Phone = customer.Phone;
            Customer.Address = customer.Address;
            Customer.Fax = customer.Fax;
            Customer.Contact = customer.Contact;
            Customer.Concat_phone = customer.Concat_phone;
            Customer.Email = customer.Email;
            Customer.Bank = customer.Bank;
            Customer.BankId = customer.BankId;
            //保存到数据库
            db.SaveChanges();
            return RedirectToAction("CustomerManagerInfo");
        }
        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerInfoDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表订单表信息
                var ordermanager = db.OrderManager
                    .Where(g => g.CustomerID == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (ordermanager == null)
                {

                }
                else
                {
                    Customer emp = db.Customer.Find(emp_id);
                    db.Customer.Remove(emp);
                }
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 客户管理-添加客户资料
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerCustomerInsertInfo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CustomerManagerCustomerInsertInfo(Customer customer)
        {
            //保存到数据库
            db.Customer.Add(customer);
            //保存到数据库
            db.SaveChanges();
            return RedirectToAction("CustomerManagerInfo");
        }
        /// <summary> 
        /// 客户管理-订单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerOrder()
        {
            //查询订单信息
            var ordermanager = db.OrderManager.ToList();
            ViewBag.ordermanager = ordermanager;
            //查询客户信息
            var customer = db.Customer.ToList();
            ViewBag.customer = customer;
            //查询交货方式
            var transport = db.Transport.ToList();
            ViewBag.trapsorp = transport;
            return View();
        }
        /// <summary>
        /// 模糊查询订单信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<OrderManager> model = db.OrderManager.OrderByDescending(a => a.Orderid).Where(a => a.Customer.Customername.Contains(name)).ToList();
            ViewBag.ordermanager = model;
            //查询客户信息
            var customer = db.Customer.ToList();
            ViewBag.customer = customer;
            //查询交货方式
            var transport = db.Transport.ToList();
            ViewBag.trapsorp = transport;
            return View("CustomerManagerOrder");//此视图为模糊查询所在的页面
        }

        /// <summary>
        /// 根据订单id查询订单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderByid()
        {
            //获取id
            var orderid = Request["ids"];
            //查询
            var orderinfo = db.OrderManager.Find(int.Parse(orderid));
            var obj = new
            {
                ordersid=orderinfo.Orderid,
                ordernumber=orderinfo.Ordercount,
                //user=orderinfo.Users.u
                hander=orderinfo.Handlers,
                customernamre=orderinfo.CustomerID,
                type=orderinfo.Transport,
                money=orderinfo.Order_price,
                price=orderinfo.Products.Price
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改订单信息
        /// </summary>
        /// <param name="odemanger"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderDelete(OrderManager odemanger)
        {
            //查询对象
            var orderinfo = db.OrderManager.Find(odemanger.Orderid);
            //修改信息
            orderinfo.Orderid = odemanger.Orderid;
            orderinfo.Ordercount=odemanger.Ordercount;
            orderinfo.Usersid = odemanger.Usersid;
            orderinfo.Handlers = odemanger.Handlers;
            orderinfo.CustomerID = odemanger.CustomerID;
            orderinfo.Transport = odemanger.Transport;
            orderinfo.Order_price = odemanger.Order_price;
            //保存数据库
            db.SaveChanges();
            return RedirectToAction("CustomerManagerOrder");
        }
        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderDeletess(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表计划订单表信息
                var planorder = db.PlanOrder
                    .Where(g => g.Orderid == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (planorder == null)
                {

                }
                else
                {
                    OrderManager emp = db.OrderManager.Find(emp_id);
                    db.OrderManager.Remove(emp);
                }
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 客户管理-添加订单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderInsert()
        {
            //查询产品信息
            var Product = db.Products.ToList();
            ViewBag.Product = Product;
            //查询订单信息
            var ordermanagerinfo = db.OrderManager.ToList();
            ViewBag.Ordermanager = ordermanagerinfo;
            //查询客户信息
            var customer = db.Customer.ToList();
            ViewBag.customer = customer;
            //查询交货方式
            var transport = db.Transport.ToList();
            ViewBag.trapsorp = transport;
            return View();
        }
        /// <summary>
        /// 根据产品id查询产品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductByid()
        {
            //获取产品id
            var Proid = Request["Productid"];
            //查询产品信息
            var produinfo = db.Products.Find(int.Parse(Proid));
            var obj = new
            {
                productid=produinfo.Productsid,
                proce=produinfo.Price,
                proguige=produinfo.Specifications,
                prowenhao=produinfo.Titanic,
                protype=produinfo.Lnven.Invenname,
                address=produinfo.Address,
                unit=produinfo.Unit,
                suppliername=produinfo.SupplierId
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="ordermanager"></param>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderInsertAdd(OrderManager ordermanager)
        {
            //保存到对象
            db.OrderManager.Add(ordermanager);
            //保存到数据库
            db.SaveChanges();
            return RedirectToAction("CustomerManagerOrderInsert");
        }
        /// <summary>
        /// 客户管理-订单跟踪
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerManagerOrderTracking()
        {
            //查询订单信息
            var order = db.OrderManager
                .OrderByDescending(p => p.Orderid);
            ViewBag.order = order;
            return View();
        }
        /// <summary>
        /// 根据订单id查询订单相关信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectByOrderid()
        {
            //获取编号
            var order = Request["ords"];
            //查询订单信息
            OrderManager ordinfo = db.OrderManager.Find(int.Parse(order));
            //查询产品表信息
            Products products = db.Products.Find(ordinfo.ProductID);
            //查询销售单信息
            var sale = (from p in db.Sales
                       where p.Orderid == ordinfo.Orderid
                       select new{
                           SalesId=p.SalesId,
                           Customername=p.Customer.Customername,
                           Contact=p.Customer.Contact,
                           Sales_number=p.Sales_number,
                           Amount_payable=p.Amount_payable,
                           Salse_state=p.Salse_state,
                           saledate=p.Sales_date,
                           Real_pay=p.Real_pay
                       }).ToList();
            int SalesId=0;
            string customername="";
            string contant="";
            int salenumber=0;
            double amoutmoney=0;
            string salestate="";
            double realpay = 0;
            DateTime saledate=DateTime.Parse("1999-1-1");
            foreach (var item in sale)
            {
                SalesId = int.Parse((item.SalesId).ToString());
                customername = item.Customername;
                contant = item.Contact;
                salenumber = int.Parse((item.Sales_number).ToString());
                amoutmoney = double.Parse((item.Amount_payable).ToString());
                salestate = item.Salse_state;
                saledate = DateTime.Parse((item.saledate).ToString());
                realpay = double.Parse((item.Real_pay).ToString());
            }
            //查询销售结款单信息
            var salebro = (from s in db.Sales_Brrowing
                          where s.SalesId == SalesId
                          select new
                          {
                              Sales_brrowid=s.Sales_brrowid,
                              Sales_brrowiddate=s.Sales_brrowiddate,
                          }).ToList();
            int salebroid = 0;
            DateTime salebrodate = DateTime.Parse("1999-1-1");
            foreach (var item in salebro)
            {
                salebroid = int.Parse((item.Sales_brrowid).ToString());
                salebrodate = DateTime.Parse((item.Sales_brrowiddate).ToString());
            }
            var obj = new
            {
                //订单信息
                orderid=ordinfo.Orderid,
                cutomername=ordinfo.Customer.Customername,
                people=ordinfo.Customer.Contact,
                jiaohuodate=(ordinfo.Ordertime).ToString(),
                ordernumber=ordinfo.Ordercount,
                orderprice=ordinfo.Order_price,
                orderdate=(ordinfo.Ordertime).ToString(),
                //产品信息
                productname=products.Productsname,
                suppliername=products.SupplierId,
                address=products.Address,
                guige=products.Specifications,
                price=products.Price,
                productype=products.Lnven.Invenname,
                unit=products.Unit,
                //销售结款单信息
                salebroid = salebroid,
                customername = customername,
                customer = contant,
                saleprice = amoutmoney,
                yudingprice = realpay,
                jiekuanprice = amoutmoney,
                jiekuandate = salebrodate.ToString(),
                //销售单信息
                saleid = SalesId,
                customername1 = customername,
                customerpeople = contant,
                salenumber = salenumber,
                salemoney = amoutmoney,
                saledate =saledate.ToString(),
                salestate = salestate
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /**************************************用户管理模块********************************************/
        #region 用户管理
        /// <summary> 系统管理-用户管理
        /// 系统管理-用户管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SystemManagementUserManagement()
        {
            List<Users> user = db.Users.ToList();
            var userList = user
                .Select(e => new
                {
                    Usersid = e.Usersid,
                    Usersname = e.Usersname,
                    UserReadname = e.UserReadname,
                    Start_time = e.Start_time,
                    Gender = e.Gender,
                    Departmentname = e.Department1.Departmentname
                })
                .Where(e => e.Usersid != 1)
                .OrderByDescending(e => e.Usersid)
                .Select(item => Tuple.Create(item.Usersid,
                    item.Usersname,
                    item.UserReadname,
                    item.Departmentname,
                    item.Gender,
                    item.Start_time))
                .ToList();
            ViewBag.user = userList;
            List<Department> dep = db.Department.ToList();
            ViewBag.dep = dep;
            return View();
        }
        /// <summary> 添加用户
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult insUser(Users user, Dermal der)
        {
            //Users ss = (from item in db.Users where item.Usersname == Request["uname"].ToString() select item).FirstOrDefault();
            string sname= Request["uname"].ToString();
            Users ss = db.Users.Where(a => a.Usersname == sname).FirstOrDefault();
            if (ss != null)
            {
                return Content("<script>alert('此用户名已存在！');window.location.href='/Personnel/SystemManagementUserManagement';</script>");
            }
            else
            {
                user.Usersname = Request["uname"];
                user.Users_password = Request["upwd"];
                user.UserReadname = Request["reuname"];
                user.Gender = Request["selsex"];
                user.Department = int.Parse(Request["emppm"]);
                user.Start_time = DateTime.Now;
                db.Users.Add(user);
                if (db.SaveChanges() > 0)
                {
                    Users s = (from item in db.Users where item.Usersname == user.Usersname select item).FirstOrDefault();
                    var id = s.Usersid;
                    der.Usersid = id;
                    der.Dermal_lname = "rgb(255, 255, 255)";
                    der.Dermal_gname = "rgb(255, 255, 255)";
                    der.Dermal_tname = "rgb(255, 255, 255)";
                    db.Dermal.Add(der);
                    db.SaveChanges();
                    return RedirectToAction("SystemManagementUserManagement");
                }
                else
                {
                    return Content("<script>alert('新增失败')</script>");
                }
                
            }
        }
        /// <summary> 编辑用户信息
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updUser()
        {
            Users emp = db.Users.Find(int.Parse(Request["empidup"]));
            emp.Usersname = Request["usname"];
            emp.UserReadname = Request["usrename"];
            emp.Users_password = Request["uspwd"];
            emp.Department = int.Parse(Request["usemppm"]);
            emp.Gender = Request["usselsex"];
            db.Entry<Users>(emp).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("SystemManagementUserManagement");                
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        /// <summary> 查询用户详情
        /// 查询员工详情
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public ActionResult seluseid(int empid)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            Users model = db.Users.Single(a => a.Usersid == empid);
            var data = new
            {
                Usersid = model.Usersid,
                Usersname = model.Usersname,
                UserReadname = model.UserReadname,
                Users_password = model.Users_password,
                Gender = model.Gender,
                Department = model.Department,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 删除用户
        ///删除员工
        /// </summary>
        /// <returns></returns>
        public ActionResult deluser(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                Users emp = db.Users.Find(emp_id);
                db.Users.Remove(emp);
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 搜索指定项
        /// 搜索指定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Selusersup(string name)
        {
            name = Request["tj"];
            List<Users> user = db.Users.ToList();
            var userList = user
                .Select(e => new
                {
                    Usersid = e.Usersid,
                    Usersname = e.Usersname,
                    UserReadname = e.UserReadname,
                    Start_time = e.Start_time,
                    Gender = e.Gender,
                    Departmentname = e.Department1.Departmentname
                })
                .Where(e => e.Usersid != 1&&e.Usersname.Contains(name))
                .OrderByDescending(e => e.Usersid)
                .Select(item => Tuple.Create(item.Usersid,
                    item.Usersname,
                    item.UserReadname,
                    item.Departmentname,
                    item.Gender,
                    item.Start_time))
                .ToList();
            ViewBag.user = userList;
            List<Department> dep = db.Department.ToList();
            ViewBag.dep = dep;
            return View("SystemManagementUserManagement");
        }
        #endregion

        /**************************************部门管理模块********************************************/
        #region 部门管理
        /// <summary> 系统管理-部门管理
        /// 系统管理-部门管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SystemManagementDivisionalManagement()
        {
            List<Department> dep = db.Department.OrderByDescending(a => a.DepartmentId).ToList();
            ViewBag.dep = dep;
            return View();
        }
        /// <summary> 添加部门
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult insDep(Department user)
        {
            user.Departmentname = Request["depname"];
            user.DepartmentReadname = Request["depsname"];
            db.Department.Add(user);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("SystemManagementDivisionalManagement");                
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        /// <summary> 搜索指定项
        /// 搜索指定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Seldepsup(string name)
        {
            name = Request["tj"];
            List<Department> model = db.Department.Where(a => a.Departmentname.Contains(name)).OrderByDescending(a => a.DepartmentId).ToList();
            ViewBag.dep = model;
            return View("SystemManagementDivisionalManagement");
        }
        /// <summary> 删除部门
        ///删除员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult deldep(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                Department emp = db.Department.Find(emp_id);
                db.Department.Remove(emp);
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 编辑部门信息
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult upddep()
        {
            Department emp = db.Department.Find(int.Parse(Request["depid"]));
            emp.Departmentname = Request["sdepname"];
            emp.DepartmentReadname = Request["sdeprename"];
            db.Entry<Department>(emp).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("SystemManagementDivisionalManagement");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary> 查询部门详情
        /// 查询员工详情
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public ActionResult seldepid(int empid)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            Department model = db.Department.Single(a => a.DepartmentId == empid);
            var data = new
            {
                DepartmentId = model.DepartmentId,
                Departmentname = model.Departmentname,
                DepartmentReadname = model.DepartmentReadname
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /**************************************公告管理模块********************************************/
        #region 公告管理
        /// <summary> 系统管理-公告管理
        /// 系统管理-公告管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SystemManagementAnnouncementOfTheManagement()
        {
            List<Announcements> ann = db.Announcements.OrderByDescending(a => a.Annountid).ToList();
            ViewBag.ann = ann;
            return View();
        }
        /// <summary> 删除公告
        ///删除员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult delann(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                Announcements emp = db.Announcements.Find(emp_id);
                db.Announcements.Remove(emp);
            }
            int num = db.SaveChanges();
            string result = "";
            if (num > 0)
            {
                result = "success";
            }
            else
            {
                result = "failure";
            }
            var obj = new
            {
                result = result
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 编辑公告信息
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updann()
        {
            Announcements emp = db.Announcements.Find(int.Parse(Request["depid"]));
            emp.Announttitle = Request["stitle"];
            emp.Annountext = Request["scontent"];
            db.Entry<Announcements>(emp).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("SystemManagementAnnouncementOfTheManagement");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary> 添加公告
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult insann(Announcements user)
        {
            user.Announttitle = Request["title"];
            user.Annountpeople = Request["uname"];
            user.Annountext = Request["content"];
            user.Annountime = DateTime.Parse(Request["anntime"]);
            db.Announcements.Add(user);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("SystemManagementAnnouncementOfTheManagement");
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        /// <summary> 详情
        /// 详情
        /// </summary>
        /// <returns></returns>
        public ActionResult selannid(int empid)
        {
            Announcements model = db.Announcements.Single(a => a.Annountid == empid);
            var data = new
            {
                Annountid = model.Annountid,
                Annountime = model.Annountime,
                Announttitle = model.Announttitle,
                Annountpeople = model.Annountpeople,
                Annountext = model.Annountext
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary> 搜索指定项
        /// 搜索指定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult selannbus(string name)
        {
            name = Request["tj"];
            List<Announcements> model = db.Announcements.Where(a => a.Announttitle.Contains(name)).OrderByDescending(a => a.Annountid).ToList();
            List<Announcements> model1 = db.Announcements.Where(a => a.Annountpeople.Contains(name)).OrderByDescending(a => a.Annountid).ToList();
            List<Announcements> model2 = db.Announcements.Where(a => a.Annountext.Contains(name)).OrderByDescending(a => a.Annountid).ToList();
            if (model.Count() > 0)
            {
                ViewBag.ann = model;
            }
            else if (model1.Count() > 0)
            {
                ViewBag.ann = model1;
            }
            else if (model2.Count() > 0)
            {
                ViewBag.ann = model2;
            }
            return View("SystemManagementAnnouncementOfTheManagement");
        }
        #endregion
        #region 请假申请
        public ActionResult Leave()
        {
            int userid = int.Parse(Session["uid"].ToString());
            //请假单信息
            List<Leave> leave = db.Leave.OrderByDescending(a => a.Leave_id).Where(a => a.Userid == userid).ToList();
            ViewBag.leave = leave;
            //部门信息
            var department = db.Department.ToList();
            ViewBag.department = department;
            return View();
        }
        /// <summary>
        /// 模糊查询请假单信息
        /// </summary>
        /// <param name="leaveid"></param>
        /// <returns></returns>
        public ActionResult BasicLeaveLike(string names)
        {
            names = Request["names"];//前端input传过来的值
            List<Leave> model = db.Leave.OrderByDescending(a => a.Leave_id).Where(a => a.Name.Contains(names)).ToList();
            ViewBag.leave = model;
            //部门信息
            var department = db.Department.ToList();
            ViewBag.department = department;
            return View("Leave");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 查询请假单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectLeaveById()
        {
            var leaveid = Request["LeaveID"];
            var LeaveInfo = db.Leave.Find(int.Parse(leaveid));           
            //返回数据
            var obj = new
            {
                Leave_id = LeaveInfo.Leave_id,
                Name = LeaveInfo.Name,
                DepartmentId = LeaveInfo.DepartmentId,
                Applytime = LeaveInfo.Applytime.ToString("yyyy-MM-dd"),
                Cause = LeaveInfo.Cause,
                Start_time = LeaveInfo.Start_time.ToString("yyyy-MM-dd"),
                End_time = LeaveInfo.End_time.ToString("yyyy-MM-dd"),
                Total_time = LeaveInfo.Total_time,
                Leave_Type = LeaveInfo.Leave_Type,
                Progress = LeaveInfo.Progress

            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加请假单
        /// </summary>
        /// <param name="ileave"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertLeave(Leave ileave)
        {
            Users users = db.Users.Find(int.Parse(Session["uid"].ToString()));
            if (users.Post == 5)
            {
                ileave.Progress = 1;
            }
            db.Leave.Add(ileave);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("Leave");
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        /// <summary>
        /// 修改请假单
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateLeave()
        {
            Leave ileave = db.Leave.Find(int.Parse(Request["Leave_id"]));
            ileave.Name = Request["Name"];
            ileave.Applytime = DateTime.Parse(Request["Applytime"]);
            ileave.Cause = Request["Cause"];
            ileave.DepartmentId = int.Parse(Request["DepartmentId"]);
            ileave.End_time = DateTime.Parse(Request["End_time"]);
            ileave.Leave_Type = Request["Leave_Type"];
            ileave.Progress = int.Parse(Request["Progress"]);
            ileave.Start_time = DateTime.Parse(Request["Start_time"]);
            ileave.Total_time = int.Parse(Request["Total_time"]);
            ileave.Userid = int.Parse(Request["Userid"]);
            db.Entry<Leave>(ileave).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("Leave");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary>
        /// 删除请假信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ActionResult DeleteLeave(List<int?> id)
        //{
        //    foreach (var emp_id in id)
        //    {
        //        Users emp = db.Users.Find(emp_id);
        //        db.Users.Remove(emp);
        //    }
        //    int num = db.SaveChanges();
        //    string result = "";
        //    if (num > 0)
        //    {
        //        result = "success";
        //    }
        //    else
        //    {
        //        result = "failure";
        //    }
        //    var obj = new
        //    {
        //        result = result
        //    };
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region 调班/调休
        public ActionResult Shif()
        {
            int userid = int.Parse(Session["uid"].ToString());
            //请假单信息
            List<Shif> shif = db.Shif.OrderByDescending(a => a.Shif_id).Where(a => a.Userid== userid).ToList();
            ViewBag.shif = shif;
            //部门信息
            var department = db.Department.ToList();
            ViewBag.department = department;
            return View();
        }
        /// <summary>
        /// 添加调班申请
        /// </summary>
        /// <param name="ishif"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertShif(Shif ishif)
        {
            Users users = db.Users.Find(int.Parse(Session["uid"].ToString()));
            if (users.Post == 5)
            {
                ishif.Progress = 1;
            }
            db.Shif.Add(ishif);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("Shif");
            }
            else
            {
                return Content("<script>alert('新增失败')</script>");
            }
        }
        [HttpPost]
        public ActionResult UpdateShif()
        {
            Shif ishif = db.Shif.Find(int.Parse(Request["Shif_id"]));
            ishif.Name = Request["Name"];
            ishif.Applay_time = DateTime.Parse(Request["Applay_time"]);
            ishif.Cause = Request["Cause"];
            ishif.DepartmentId = int.Parse( Request["DepartmentId"]);
            ishif.Leave_Type = Request["Leave_Type"];
            ishif.NEnd_time = DateTime.Parse( Request["NEnd_time"]);
            ishif.NStart_time = DateTime.Parse( Request["NStart_time"]);
            ishif.NTotal = int.Parse( Request["NTotal"]);
            ishif.Progress = int.Parse( Request["Progress"]);
            ishif.Userid = int.Parse( Request["Userid"]);
            ishif.YEnd_time = DateTime.Parse(Request["YEnd_time"]);
            ishif.YStart_time = DateTime.Parse(Request["YStart_time"]);
            ishif.YTotal = int.Parse(Request["YTotal"]);
            db.Entry<Shif>(ishif).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("Shif");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary>
        /// 编辑调班信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectShifById()
        {
            var shifid = Request["ShifId"];
            var ShifInfo = db.Shif.Find(int.Parse(shifid));
            //返回数据
            var obj = new
            {
                Shif_id = ShifInfo.Shif_id,
                Applay_time = ShifInfo.Applay_time.ToString("yyyy-MM-dd"),
                Cause = ShifInfo.Cause,
                DepartmentId = ShifInfo.DepartmentId,
                Leave_Type = ShifInfo.Leave_Type,
                Name = ShifInfo.Name,
                NEnd_time = ShifInfo.NEnd_time.ToString("yyyy-MM-dd"),
                NStart_time = ShifInfo.NStart_time.ToString("yyyy-MM-dd"),
                NTotal = ShifInfo.NTotal,
                YEnd_time = ShifInfo.YEnd_time.ToString("yyyy-MM-dd"),
                YStart_time = ShifInfo.YStart_time.ToString("yyyy-MM-dd"),
                YTotal = ShifInfo.YTotal,
                Progress = ShifInfo.Progress

            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public ActionResult BasicShifLike(string names)
        {
            names = Request["names"];//前端input传过来的值
            List<Shif> model = db.Shif.OrderByDescending(a => a.Shif_id).Where(a => a.Name.Contains(names)).ToList();
            ViewBag.shif = model;
            //部门信息
            var department = db.Department.ToList();
            ViewBag.department = department;
            return View("Shif");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 调休/调班审批 信息
        /// </summary>
        /// <returns></returns>
        public ActionResult examainShif()
        {
            Users users = db.Users.Find(int.Parse(Session["uid"].ToString()));
            int depid = int.Parse(users.Department.ToString());
            int userid = int.Parse(users.Usersid.ToString());
            //判断等级权限
            if (users.Post>1 && users.Post <= 5)
            {
                //部门经理，只能审批本部门的
                //List<Shif> shifList = db.Shif.Find(departmentId).
                List <Shif> shifList = db.Shif.OrderByDescending(a => a.Shif_id).Where(a => a.DepartmentId.Equals(depid) && a.Progress==0).ToList();
                ViewBag.examain = shifList;
            }
            else
            {
                //总经理,审批所有部门的
                List<Shif> shifList = db.Shif.OrderByDescending(a => a.Shif_id).Where(a => a.Progress ==1).ToList();
                ViewBag.examain = shifList;
            }
            var department = db.Department.ToList();
            ViewBag.department = department;
            return View();
            //根据账号查询出其所在的部门
            //通过部门查询出该部门的审批单
        }
        /// <summary>
        /// 审批操作
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateexamainShif()
        {
            Users users = db.Users.Find(int.Parse(Session["uid"].ToString()));
            Shif ishif = db.Shif.Find(int.Parse(Request["Shif_id"]));
            if(users.Post > 1 && users.Post <= 5)
            {
                ishif.Progress = 1;
            }
            else
            {
                ishif.Progress = 2;
            }
            
            db.Entry<Shif>(ishif).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("examainShif");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary>
        /// 请假审批 信息
        /// </summary>
        /// <returns></returns>
        public ActionResult examainLeave()
        {
            Users users = db.Users.Find(int.Parse(Session["uid"].ToString()));
            int depid = int.Parse(users.Department.ToString());
            int userid = int.Parse(users.Usersid.ToString());
            //判断等级权限
            if (users.Post > 1 && users.Post <= 5)
            {
                //部门经理，只能审批本部门的
                //List<Shif> shifList = db.Shif.Find(departmentId).
                List<Leave> LeaveList = db.Leave.OrderByDescending(a => a.Leave_id).Where(a => a.DepartmentId.Equals(depid) && a.Progress == 0).ToList();
                ViewBag.examainleave = LeaveList;
            }
            else
            {
                //总经理,审批所有部门的
                List<Leave> LeaveList = db.Leave.OrderByDescending(a => a.Leave_id).Where(a => a.Progress == 1).ToList();
                ViewBag.examainleave = LeaveList;
            }
            var department = db.Department.ToList();
            ViewBag.department = department;
            return View();
        }
        /// <summary>
        /// 审批操作
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateexamainLeave()
        {
            Users users = db.Users.Find(int.Parse(Session["uid"].ToString()));
            Leave ileave = db.Leave.Find(int.Parse(Request["Leave_id"]));
            if (users.Post > 1 && users.Post <= 5)
            {
                ileave.Progress = 1;
            }
            else
            {
                ileave.Progress = 2;
            }

            db.Entry<Leave>(ileave).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("examainLeave");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        #endregion
    }
}
