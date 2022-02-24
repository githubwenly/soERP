using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using L_.Models;
using SoERP.Models;
using System.Dynamic;//引用

namespace SoERP.Controllers
{
    public class ManagementController : Controller
    {
        //
        //实例化上下文类
        ErpDBEntities db = new ErpDBEntities();
        //定义查询采购单信息方法
        //-------------------------
        private void PurchasList()
        {
            var purchasorder = (from p in db.Purchase_order
                                join g in db.Goods on
                                    p.Goodsid equals g.Goodsid
                                select new
                                {
                                    purchasid = p.Purchase_orderid,
                                    goodsname = g.Goodsname,
                                    purchasnumber = p.Procurement_number,
                                    purchardata = p.Purchase_date,
                                    purcharstae = p.State
                                })
                                .OrderByDescending(p=>p.purchasid)
                                .ToList();
            //dynamic动态绑定
            //使用ExpandoObject需要引用using System.Dynamic;
            //使用dynamic和ExpandoObject，实现数据转义后一体化的输出，包括增加任意多的字段信息。
            List<dynamic> purchas = new List<dynamic>();
            foreach (var data in purchasorder.ToList())
            {
                dynamic dyobject = new ExpandoObject();
                dyobject.purchasid = data.purchasid;
                dyobject.goodsname = data.goodsname;
                dyobject.purchasnumber = data.purchasnumber;
                dyobject.purchardata = data.purchardata;
                dyobject.purcharstae = data.purcharstae;
                purchas.Add(dyobject);
            }
            ViewBag.purchas = purchas;
           
        }
        //-----------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        //定义根据状态查询采购单信息的方法
        private void PurchasList(string state)
        {
            var purchasorder = (from p in db.Purchase_order
                                join g in db.Goods on
                                    p.Goodsid equals g.Goodsid
                                select new
                                {
                                    purchasid = p.Purchase_orderid,
                                    goodsname = g.Goodsname,
                                    purchasnumber = p.Procurement_number,
                                    purchardata = p.Purchase_date,
                                    purcharstae = p.State
                                })
                                
                                .Where(p=>p.purcharstae==state)
                                .ToList();
            //dynamic动态绑定
            //使用ExpandoObject需要引用using System.Dynamic;
            //使用dynamic和ExpandoObject，实现数据转义后一体化的输出，包括增加任意多的字段信息。
            List<dynamic> purchas = new List<dynamic>();
            foreach (var data in purchasorder.ToList())
            {
                dynamic dyobject = new ExpandoObject();
                dyobject.purchasid = data.purchasid;
                dyobject.goodsname = data.goodsname;
                dyobject.purchasnumber = data.purchasnumber;
                dyobject.purchardata = data.purchardata;
                dyobject.purcharstae = data.purcharstae;
                purchas.Add(dyobject);
            }
            ViewBag.purchas = purchas;
        }
       
        //----------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        // GET: /Management/
        /// <summary>
        /// 生产管理-计划管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagerPlanManager()
        {
            //查询生产计划单信息
            var planOrder = db.PlanOrder
                .OrderByDescending(p=>p.PlanOrderid);
            ViewBag.planorder = planOrder;
            //查询订单信息
            var ordermanager = db.OrderManager;
            ViewBag.ordermanager = ordermanager;
            return View();
        }
        /// <summary>
        /// 模糊查询计划管理
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerPlanManagerLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<PlanOrder> model = db.PlanOrder.OrderByDescending(a => a.PlanOrderid).Where(a => a.Product_name.Contains(name)).ToList();
            ViewBag.planorder = model;
            //查询订单信息
            var ordermanager = db.OrderManager;
            ViewBag.ordermanager = ordermanager;
            return View("ProductionManagerPlanManager");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据选择的订单id查询订单产品名
        /// </summary>
        /// <returns></returns>
        public ActionResult Productgoodsbyid()
        {
            //获取传输的值
            var id = Request["ids"];
            //查询
            var ordermanager = db.OrderManager.Find(int.Parse(id));
            var obj = new
            {
                productname=ordermanager.Products.Productsname
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加计划管理
        /// </summary>
        /// <param name="planorder"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerPlanManagerAdd(PlanOrder planorder)
        {
            //保存到对象
            db.PlanOrder.Add(planorder);
            //保存到数据库
            db.SaveChanges();
            return RedirectToAction("ProductionManagerPlanManager");
        }
        /// <summary>
        /// 根据计划订单编号查询计划订单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagerPlanManagerByid()
        {
            //获取传递值
            var planorderid = Request["planorderid"];
            //查询
            var planorderinfo = db.PlanOrder.Find(int.Parse(planorderid));
            var obj = new
            {
                PlanOrderid=planorderinfo.PlanOrderid,
                Product_name=planorderinfo.Product_name,
                Production=planorderinfo.Production,
                Product_time=planorderinfo.Product_time.ToString(),
                Handlers=planorderinfo.Handlers,
                Orderid=planorderinfo.Orderid,
                Usersid=planorderinfo.Users.Usersname,
                Tips=planorderinfo.Tips,
                planorderid=planorderinfo.PlanOrderid
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改计划管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagerPlanManagerUpdate(PlanOrder planod)
        {
            var planinf = db.PlanOrder.Find(planod.PlanOrderid);
            planinf.Product_time = planod.Product_time;
            planinf.Handlers = planod.Handlers;
            planinf.Usersid = planod.Usersid;
            planinf.Production = planod.Production;
            planinf.Tips = planod.Tips;
            //保存
            db.SaveChanges();
            return RedirectToAction("ProductionManagerPlanManager");
        }
        /// <summary>
        /// 删除计划订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerPlanManagerDelete (List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表产品生产表信息
                var product = db.Product
                    .Where(g => g.PlanOrderid == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (product == null)
                {

                }
                else
                {
                PlanOrder emp = db.PlanOrder.Find(emp_id);
                db.PlanOrder.Remove(emp);
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
        /// 生产管理-领料管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagerAcquisitionManager()
        {
            //查询领料单信息
            var materal = db.Material
                .OrderByDescending(m=>m.Materialid);
            ViewBag.materal = materal;
            //查询部门信息
            var department = db.Department;
            ViewBag.deparment = department;
            //查询材料信息
            var Goods = db.Goods;
            ViewBag.Goodse = Goods;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerAcquisitionManagerLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Material> model = db.Material.OrderByDescending(a => a.Materialid).Where(a => a.Goods.Goodsname.Contains(name)).ToList();
            ViewBag.materal = model;
            //查询部门信息
            var department = db.Department;
            ViewBag.deparment = department;
            //查询材料信息
            var Goods = db.Goods;
            ViewBag.Goodse = Goods;
            return View("ProductionManagerAcquisitionManager");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 添加领料单
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerAcquisitionManagerAdd(Material me)
        {
            //判断是否有选择部门和商品
            if (me.Goodsid == 0 || me.Departmentid == 0)
            {
                return Content("<script>alert('未选择部门和商品信息');window.location.href='/Management/ProductionManagerAcquisitionManager';</script>");
            }
            else
            {
                //保存到对象
                db.Material.Add(me);

                //保存数据库
                db.SaveChanges();
                return RedirectToAction("ProductionManagerAcquisitionManager");
            }
        }
        /// <summary>
        /// 根据id查询领料单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagerAcquisitionManagerByid()
        {
            //获取编号
            var ids = Request["ids"];
            //据编号查询
            var mateinfo = db.Material.Find(int.Parse(ids));
            var obj = new
            {
                Materialid=mateinfo.Materialid,
                Goodsid1=mateinfo.Goodsid,
                Materialcount1=mateinfo.Materialcount,
                Material_time2=mateinfo.Material_time.ToString(),
                Material_use1=mateinfo.Material_use,
                Usersid1=mateinfo.Users.Usersname,
                Material_member1=mateinfo.Material_member,
                Departmentid1=mateinfo.Departmentid,
                Tips1=mateinfo.Tips
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改领料单信息
        /// </summary>
        /// <param name="mateil"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerAcquisitionManagerUpdate(Material mateil)
        {
            //实例化领料单
            var mateinfo = db.Material.Find(mateil.Materialid);
            //修改信息
            mateinfo.Materialid = mateil.Materialid;
            mateinfo.Goodsid = mateil.Goodsid;
            mateinfo.Materialcount = mateil.Materialcount;
            mateinfo.Material_time = mateil.Material_time;
            mateinfo.Material_use = mateil.Material_use;
            mateinfo.Usersid = mateil.Usersid;
            mateinfo.Material_member = mateil.Material_member;
            mateinfo.Departmentid = mateil.Departmentid;
            mateinfo.Tips = mateil.Tips;
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("ProductionManagerAcquisitionManager");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }

        }
        /// <summary>
        /// 删除领料单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductionManagerAcquisitionManagerDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表产品出库表信息
                //var product = db.Product
                //    .Where(g => g.ma == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goods == null)
                //{

                //}
                //else
                //{
                Material emp = db.Material.Find(emp_id);
                db.Material.Remove(emp);
                //}
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
        /// 生产管理-产品生产
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagegrProductsProduction()
        {
            //查询产品生产单信息
            var ProductInfo = db.Product
                .OrderByDescending(p=>p.Productid);
            ViewBag.Product = ProductInfo;
            //查询产品计划单
            var Planorder = db.PlanOrder;
            ViewBag.Planorder = Planorder;
            //查询部门信息
            var department = db.Department;
            ViewBag.depart = department;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ProductionManagegrProductsProductionLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Product> model = db.Product.OrderByDescending(a => a.Productid).Where(a => a.Product_name.Contains(name)).ToList();
            ViewBag.Product = model;
            //查询产品计划单
            var Planorder = db.PlanOrder;
            ViewBag.Planorder = Planorder;
            //查询部门信息
            var department = db.Department;
            ViewBag.depart = department;
            return View("ProductionManagegrProductsProduction");//此视图为模糊查询所在的页面
        }
        //
        /// <summary>
        /// 根据选择的计划编号查询对应的产品名称
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectProductname()
        {
            //获取id
            var ids = Request["ids"];
            //查询产品
            var productinfo = db.PlanOrder.Find(int.Parse(ids));
            var obj = new
            {
                productname=productinfo.Product_name
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加产品生产
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagegrProductsProductionAdd(Product profuct)
        {
            //保存到对象
            db.Product.Add(profuct);
            //保存数据库
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("ProductionManagegrProductsProduction");
            }
            else
            {
                return Content("<script>alert('添加失败')</script>");
            }
        }
        /// <summary>
        /// 根据生产id查询生产信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductionManagegrProductsProductionByID()
        {
            //获取id
            var productid = Request["productid"];
            //查询信息
            var producinfo = db.Product.Find(int.Parse(productid));
            var obj = new
            {
                Productid=producinfo.Productid,
                Productcount=producinfo.Productcount,
                Departmentid=producinfo.Departmentid,
                Product_time=producinfo.Product_time.ToString(),
                Product_name=producinfo.Product_name,
                Product_pihao=producinfo.Product_pihao,
                Usersid=producinfo.Users.Usersname,
                Tips=producinfo.Tips
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改产品生产单信息
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ActionResult ProductionManagegrProductsProductionUpdate(Product product)
        {
            //查询对象
            var productinfo = db.Product.Find(product.Productid);
            productinfo.Productid = product.Productid;
            productinfo.Productcount = product.Productcount;
            productinfo.Departmentid = product.Departmentid;
            productinfo.Product_time = product.Product_time;
            productinfo.Product_name = product.Product_name;
            productinfo.Product_pihao = product.Product_pihao;
            productinfo.Usersid = product.Usersid;
            productinfo.Tips = product.Tips;
            //保存数据库
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("/Management/ProductionManagegrProductsProduction");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
         /// <summary>
        /// 采购管理-添加采购单
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerAddPurchaseOrder()
        {
            //查询采购单信息
            PurchasList();
            //查询商品
            var goods = db.Goods.ToList();
            ViewBag.Goods = goods;
            //查询商品信息
            return View();
        }      
        
        /// <summary>
        /// 根据id查询商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectGoodsinfo()
        {
            //获取id
            var Goodsid = Request["goods"];
            //查询信息
            var Goodsinfo = db.Goods.Find(int.Parse(Goodsid));
            //供应商和类别都存在时
            if (Goodsinfo.SupplierId != null && Goodsinfo.Categoryid != null)
            {
                var obj = new
                {
                    goodsid = Goodsinfo.Goodsid,
                    address = Goodsinfo.Origin,
                    unit = Goodsinfo.Unit,
                    speacis = Goodsinfo.Specifications,
                    tianct = Goodsinfo.Titanic,
                    price = Goodsinfo.Price,
                    lvenname = Goodsinfo.Lnven.Invenname,
                    suppliername = Goodsinfo.Supplier.Supplier_name
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
                //供应商不存在，类别存在
            else if(Goodsinfo.SupplierId==null&&Goodsinfo.Categoryid!=null)
            {
                var obj = new
                {
                    goodsid = Goodsinfo.Goodsid,
                    address = Goodsinfo.Origin,
                    unit = Goodsinfo.Unit,
                    speacis = Goodsinfo.Specifications,
                    tianct = Goodsinfo.Titanic,
                    price = Goodsinfo.Price,
                    lvenname = Goodsinfo.Lnven.Invenname,
                    suppliername = ""
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            //供应商存在，类别不存在
            else if (Goodsinfo.SupplierId != null && Goodsinfo.Categoryid == null)
            {
                var obj = new
                {
                    goodsid = Goodsinfo.Goodsid,
                    address = Goodsinfo.Origin,
                    unit = Goodsinfo.Unit,
                    speacis = Goodsinfo.Specifications,
                    tianct = Goodsinfo.Titanic,
                    price = Goodsinfo.Price,
                    lvenname = "",
                    suppliername = Goodsinfo.Supplier.Supplier_name
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
                //供应商和类别都不存在
            else
            {
                var obj = new
                {
                    goodsid = Goodsinfo.Goodsid,
                    address = Goodsinfo.Origin,
                    unit = Goodsinfo.Unit,
                    speacis = Goodsinfo.Specifications,
                    tianct = Goodsinfo.Titanic,
                    price = Goodsinfo.Price,
                    lvenname = "",
                    suppliername = ""
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 添加采购单信息
        /// </summary>
        /// <param name="purchasorder"></param>
        /// <returns></returns>
        public ActionResult PurchasingManagerAddPurchaseOrderAdd(Purchase_order purchasorder)
        {
            //保存到对象
            db.Purchase_order.Add(purchasorder);
            //保存到数据库
            db.SaveChanges();
            return RedirectToAction("PurchasingManagerAddPurchaseOrder");
        }

        /// <summary>
        /// 采购管理-采购单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManager()
        {
            //查询采购单信息
            var purchas = db.Purchase_order
                .OrderByDescending(p => p.Purchase_orderid);
            ViewBag.purchas = purchas;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManagerLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_order> model = db.Purchase_order.OrderByDescending(a => a.Purchase_orderid).Where(a => a.Goods.Goodsname.Contains(name)).ToList();
            ViewBag.purchas = model;
            return View("PurchasingManagerPurchaseOrderManager");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 采购付款单查询
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManagerbrrowing()
        {
            //获取采购单信息
            var purchasid = Request["Purchasid"];
            //查询采购单信息
            var purchsinfo = db.Purchase_order.Find(int.Parse(purchasid));
            var obj = new
            {
                purchasid=purchsinfo.Purchase_orderid,
                goodsname=purchsinfo.Goods.Goodsname,
                purchaasnumber=purchsinfo.Procurement_number,
                amountPrice=purchsinfo.Amount_payable,
                readprice=purchsinfo.Real_pay,
                unpidprice=purchsinfo.Unpaid_amount,
                purchasPrice=purchsinfo.Purchase_money,
                Price=purchsinfo.Goods.Price

            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加采购结款单信息
        /// </summary>
        /// <param name="Pb"></param>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManagerAdd(Purchase_brrowing Pb)
        {
            //保存采购单信息
            db.Purchase_brrowing.Add(Pb);
            //保存到数据库
            if (db.SaveChanges() > 0)
            {
                //获取采购单编号
                var purchasid = Pb.Purchaseid;
                var purchasinfo = db.Purchase_order.Find(purchasid);
                purchasinfo.State = "待入库";
                //保存数据库
                db.SaveChanges();
            }
           
            return RedirectToAction("PurchasingManagerPurchaseOrderManager");
        }
        /// <summary>
        /// 查询采购退货单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManagerReurnByid()
        {
            //获取采购单编号
            var purchasid = Request["Purchasid"];
            //查询采购单信息
            var purchasinfo = db.Purchase_order.Find(int.Parse(purchasid));
            var obj = new
            {
                purchasid=purchasinfo.Purchase_orderid,
                goodsname=purchasinfo.Goods.Goodsname,
                price=purchasinfo.Goods.Price
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加采购退货单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManagerReturn(Purchase_returns PR)
        {
            //获取采购信息
            var purchainfo = db.Purchase_order.Find(PR.Purchase_orderid);
            var number=purchainfo.Procurement_number;
            //判断退货数量是否超过采购数量
            if (int.Parse(PR.Purchase_returnsnumber) > number)
            {
                //大于时则无法保存
                return Content("<script>alert('退货数量有误，不能大于采购数量')</script>");
            }
            else
            {
                //小于，保存采购退货单信息
                db.Purchase_returns.Add(PR);
                //保存数据库
                if (db.SaveChanges() > 0)
                {
                    //退货成功
                    purchainfo.State = "待入库";
                    purchainfo.Procurement_number = purchainfo.Procurement_number - int.Parse(PR.Purchase_returnsnumber);
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("PurchasingManagerPurchaseOrderManager");
        }
        /// <summary>
        /// 删除采购单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerPurchaseOrderManagerDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表商品质检单信息
                var zhijian = db.Good_Quality
                    .Where(g => g.Purchase_orderid == emp_id).ToList();
                //如果质检表中存在此采购单信息
                if (zhijian == null)
                {

                }
                else
                {
                    //查询商品结款表
                    var jiankuan = db.Purchase_brrowing
                        .Where(p => p.Purchaseid == emp_id).ToList();
                    //如果商品结款单存在此采购单信息
                    if (jiankuan == null)
                    {
                    }
                    else
                    {
                        //查询采购入库单
                        var Ruku = db.Purchase_Put
                            .Where(p => p.Purchaseid == emp_id).ToList();
                        //如果入库单存在此采购单信息
                        if (Ruku == null)
                        {
                        }
                        else
                        {
                            //查询退货单信息
                            var Tuihuo = db.Purchase_returns
                                .Where(p => p.Purchase_orderid == emp_id).ToList();
                            //如果采购退货单存在此信息
                            if (Tuihuo == null)
                            {
                            }
                            else
                            {
                                Purchase_order emp = db.Purchase_order.Find(emp_id);
                                db.Purchase_order.Remove(emp);
                            }
                        }
                    }
                    
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
        /// 采购管理-采购单统计
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingManagerProcurementStatistics()
        {
            return View();
        }
         /// <summary>
        /// 企业库存管理-库存管理
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManager()
        {
            //查询库存表信息
            var GoodsKuCun = db.Goods_inventory
                .OrderByDescending(p=>p.InventoryId)
                .ToList();
            ViewBag.KuCun = GoodsKuCun;
            return View();
        }
        /// <summary>
        /// 库存模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Goods_inventory> model = db.Goods_inventory.OrderByDescending(a => a.Invenid).Where(a => a.Name.Contains(name)).ToList();
            ViewBag.KuCun = model;
            return View("MaterialInventoryManager");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据库存id查询库存信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerByID()
        {
            //获取库存id
            var InventoryId = Request["Kuid"];
            //查询库存信息
            var GoodsInventory = db.Goods_inventory.Find(int.Parse(InventoryId));
            //判断是商品还是产品
            if (GoodsInventory.Inventory_type == "商品")
            {
                //本商品入库信息是否被删除
                if (GoodsInventory.Invenid == null)
                {
                    var obj = new
                    {
                        goodsname = GoodsInventory.Name,
                        address = "",
                        tintice = "",
                        goodsjiechen = "",
                        guige = "",
                        Suppliername = GoodsInventory.Suppliername,
                        unit = GoodsInventory.Unit,
                        packing = "",
                        leiType = "",
                        price = GoodsInventory.Price,
                        pihao = "",
                        kunumber = GoodsInventory.Inventory
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var obj = new
                    {
                        goodsname = GoodsInventory.Name,
                        address = GoodsInventory.Purchase_Put.Purchase_order.Goods.Origin,
                        tintice = GoodsInventory.Purchase_Put.Purchase_order.Goods.Titanic,
                        goodsjiechen = GoodsInventory.Purchase_Put.Purchase_order.Goods.Goodsjieche,
                        guige = GoodsInventory.Purchase_Put.Purchase_order.Goods.Specifications,
                        Suppliername = GoodsInventory.Suppliername,
                        unit = GoodsInventory.Unit,
                        packing = GoodsInventory.Purchase_Put.Purchase_order.Goods.Packaging,
                        leiType = GoodsInventory.Purchase_Put.Purchase_order.Goods.Lnven.Invenname,
                        price = GoodsInventory.Price,
                        pihao = GoodsInventory.Purchase_Put.Purchase_order.Goods.Batch_number,
                        kunumber = GoodsInventory.Inventory
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //本产品入库信息是否被删除
                if (GoodsInventory.ProduPutid == null)
                {
                    var obj = new
                    {
                        goodsname = GoodsInventory.Name,
                        address = "",
                        tintice = "",
                        goodsjiechen = "",
                        guige = "",
                        Suppliername = GoodsInventory.Suppliername,
                        unit = GoodsInventory.Unit,
                        packing = "",
                        leiType = "",
                        price = GoodsInventory.Price,
                        pihao = "",
                        kunumber = GoodsInventory.Inventory
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var obj = new
                    {
                        goodsname = GoodsInventory.Name,
                        address = GoodsInventory.Product_Put.Product.PlanOrder.OrderManager.Products.Address,
                        tintice = GoodsInventory.Product_Put.Product.PlanOrder.OrderManager.Products.Titanic,
                        goodsjiechen = GoodsInventory.Product_Put.Product.PlanOrder.OrderManager.Products.Productsjieche,
                        guige = GoodsInventory.Product_Put.Product.PlanOrder.OrderManager.Products.Specifications,
                        Suppliername = GoodsInventory.Suppliername,
                        unit = GoodsInventory.Unit,
                        packing = GoodsInventory.Product_Put.Product.PlanOrder.OrderManager.Products.Packaging,
                        leiType = GoodsInventory.Product_Put.Product.PlanOrder.OrderManager.Products.Lnven.Invenname,
                        price = GoodsInventory.Price,
                        pihao = GoodsInventory.Product_Put.Product.Product_pihao,
                        kunumber = GoodsInventory.Inventory
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            
            
        }
        /// <summary>
        /// 删除库存信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表产品出库表信息
                //var goods = db.Goods_Delivery
                //    .Where(g => g.Goodsid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goods == null)
                //{

                //}
                //else
                //{
                Goods_inventory emp = db.Goods_inventory.Find(emp_id);
                db.Goods_inventory.Remove(emp);
                //}
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
        /// 企业库存管理-添加商品入库单
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialReceipt()
        {
            //查询采购单信息
            //PurchasList("待入库");
            var purcharorder = db.Purchase_order
                .Where(g=>g.State=="待入库")
                .OrderByDescending(p => p.Purchase_orderid);
            ViewBag.purchas = purcharorder;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialReceiptLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_order> model = db.Purchase_order.OrderByDescending(a => a.Purchase_orderid).Where(a => a.Goods.Goodsname.Contains(name)).ToList();
            ViewBag.purchas = model;
            return View("MaterialInventoryManagerAddMaterialReceipt");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 查询采购单商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialReceiptByID()
        {
            //获取采购单信息
            var purchasid = Request["Purchasid"];
            //查询信息
            var purchasinfo = db.Purchase_order.Find(int.Parse(purchasid));
            var obj = new
            {
                purchasid=purchasinfo.Purchase_orderid,
                putnumber=purchasinfo.Procurement_number,
                guige=purchasinfo.Goods.Specifications,
                goodsname=purchasinfo.Goods.Goodsname,
                amountPrice=purchasinfo.Amount_payable,
                Price=purchasinfo.Goods.Price,
                suppliername=purchasinfo.Goods.Supplier.Supplier_name,
                Upint=purchasinfo.Unpaid_amount,
                lvenname=purchasinfo.Goods.Lnven.Invenname,
                address=purchasinfo.Goods.Origin,
                readPrice=purchasinfo.Real_pay,
                goodsid=purchasinfo.Goods.Goodsid,
                number=purchasinfo.Procurement_number
                
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加商品入库单
        /// </summary>
        /// <param name="PPut"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialReceiptAdd(Purchase_Put PPut)
        {
            //保存到数据库
            db.Purchase_Put.Add(PPut);
            //保存到数据库
            if(db.SaveChanges()>0){
                //查询采购单信息
                var purchasinfo = db.Purchase_order.Find(PPut.Purchaseid);
                purchasinfo.State = "已入库";
                //入库成功将数据再保存到库存表中
                if (db.SaveChanges()>0)
                {
                    //查询本次操作的商品入库id
                    Purchase_Put Putid = db.Purchase_Put.Single(a => a.Purchaseid==PPut.Purchaseid);
                    //实例化库存表对象
                    Goods_inventory GooIn = new Goods_inventory();
                    //赋值对象
                    GooIn.Invenid = Putid.Putid;//入库id
                    GooIn.Name = Putid.Purchase_order.Goods.Goodsname;//商品名
                    GooIn.Inventory_type = "商品";
                    GooIn.Inventory = Putid.Putnumber;
                    GooIn.Price = Putid.Purchase_order.Goods.Price;
                    GooIn.Unit = Putid.Purchase_order.Goods.Unit;
                    GooIn.Suppliername = Putid.Purchase_order.Goods.Supplier.Supplier_name;
                    //保存到库存表对象
                    db.Goods_inventory.Add(GooIn);
                    //保存到数据库
                    db.SaveChanges();

                }
            }
            return RedirectToAction("MateriaInventoryManagerMaterialReceiptManage");
        }
        /// <summary>
        /// 企业库存管理-商品入库单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult MateriaInventoryManagerMaterialReceiptManage()
        {
            //查询入库单信息
            var purchasPut = db.Purchase_Put
                .OrderByDescending(p=>p.Putid)
                .ToList();
            ViewBag.PurchasPut = purchasPut;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult MateriaInventoryManagerMaterialReceiptManageLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_Put> model = db.Purchase_Put.OrderByDescending(a => a.Putid).Where(a => a.Purchase_order.Goods.Goodsname.Contains(name)).ToList();
            ViewBag.PurchasPut = model;
            return View("MateriaInventoryManagerMaterialReceiptManage");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据入库单id查询入库单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MateriaInventoryManagerMaterialReceiptManageById()
        {
            //获取入库单id
            var putid = Request["Purputid"];
            //查询入库单信息
            var PurOutinfo = db.Purchase_Put.Find(int.Parse(putid));
            var obj = new
            {
                purchasid=PurOutinfo.Purchaseid,
                purputid=PurOutinfo.Putid,
                goodsname=PurOutinfo.Purchase_order.Goods.Goodsname,
                putdate=PurOutinfo.Put_date,
                putnumber=PurOutinfo.Putnumber,
                amountprice=PurOutinfo.Purchase_order.Amount_payable,
                readprice=PurOutinfo.Purchase_order.Real_pay,
                upientPrice=PurOutinfo.Purchase_order.Unpaid_amount,
                hander=PurOutinfo.Handlers,
                userid=PurOutinfo.Users.Usersname,
                note=PurOutinfo.Note
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除商品入库单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MateriaInventoryManagerMaterialReceiptManageDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表库存信息
                var goodsinven = db.Goods_inventory
                    .Where(g => g.Invenid == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (goodsinven == null)
                {

                }
                else
                {
                    Purchase_Put emp = db.Purchase_Put.Find(emp_id);
                    db.Purchase_Put.Remove(emp);
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
        ///企业库存管理-添加产品入库单
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductManagerReceipt()
        {
            //查询待入库的的生产单信息
            var products = db.Product
                .Where(p => p.State == "待入库")
                .OrderByDescending(o => o.Productid)
                .ToList();
            ViewBag.Products = products;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Product> model = db.Product.OrderByDescending(a => a.Productid).Where(a => a.Product_name.Contains(name)&&a.State=="待入库").ToList();
            ViewBag.Products = model;
            return View("ProductManagerReceipt");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据生产编号查询生产信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptByid()
        {
            //获取传输的生产编号
            var productod = Request["proids"];
            //查询
            var proinfo = db.Product.Find(int.Parse(productod));
            var obj = new
            {
                productid=proinfo.Productid,
                productname=proinfo.Product_name,
                department=proinfo.Department.Departmentname,
                putnumber=proinfo.Productcount
            };
            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 产品入库
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptAdd(Product_Put pb)
        {
            //保存对象
            db.Product_Put.Add(pb);
            //保存数据库,如果保存入库表成功
            if (db.SaveChanges() > 0)
            {
                //修改生产单的状态
                var product = db.Product.Find(pb.Productid);
                product.State = "已入库";
                //保存数据库
                if (db.SaveChanges() > 0)
                {
                    //查询本次入库产品生产单信息
                    var pro = db.Product.Find(pb.Productid);
                    //查询入库单信息
                    Product_Put put = db.Product_Put.Find(pb.Productid);
                    //实例化库存信息
                    Goods_inventory gin = new Goods_inventory();
                    //查询本次入库的产品库存表是否已存在
                    var ginv = db.Goods_inventory
                        .Where(g => g.Name == pb.Product.Product_name);
                    if (ginv.Count() > 0)
                    {
                        Goods_inventory gi = (from item in db.Goods_inventory where item.Name == pb.Product.Product_name select item).FirstOrDefault();
                        //增加库存数量
                        gi.Inventory += pb.Putnumber;
                        db.SaveChanges();
                    }
                    else
                    {
                        //查询入库单信息
                        Product_Put put1 = db.Product_Put.Find(pb.Productid);
                        //新增库存信息
                        //编辑库存信息
                        gin.Name = pro.Product_name;
                        gin.Price = pro.PlanOrder.OrderManager.Products.Price;
                        gin.Unit = pro.PlanOrder.OrderManager.Products.Specifications;
                        gin.Suppliername = pro.PlanOrder.OrderManager.Products.SupplierId;
                        gin.ProduPutid = put1.Putid;
                        gin.Inventory = put1.Putnumber;
                        gin.Inventory_type = "产品";
                        //保存对象
                        db.Goods_inventory.Add(gin);
                        //保存数据库
                        db.SaveChanges();
                    }

                }
                }
                
            else
            {
                //保存失败
                return Content("<script>alert('添加失败')</script>");

            }
            //跳转到产品入库库存管理
            return RedirectToAction("ProductManagerReceiptManage");
        }
        /// <summary>
        /// 企业库存管理-产品入库库存管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptManage()
        {
            //查询产品入库单信息
            var productput = db.Product_Put
                .OrderByDescending(p => p.Putid);
            ViewBag.proPut = productput;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptManageLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Product_Put> model = db.Product_Put.OrderByDescending(a => a.Putid).Where(a => a.Product.Product_name.Contains(name)).ToList();
            ViewBag.proPut = model;
            return View("ProductManagerReceiptManage");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据id查询入库单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptManageByid()
        {
            //获取入库id
            var putid = Request["putid"];
            //查询
            var proput = db.Product_Put.Find(int.Parse(putid));
            var obj = new
            {
                Putid=proput.Putid,
                Productid=proput.Productid,
                productname=proput.Product.Product_name,
                putdate=proput.Put_date.ToString(),
                hander=proput.Handlers,
                putnumber=proput.Putnumber,
                user=proput.Usersid,
                note=proput.Note
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除产品入库单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductManagerReceiptManageDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表库存信息
                var goodsinven = db.Goods_inventory
                    .Where(g => g.Invenid == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (goodsinven == null)
                {

                }
                else
                {
                    Product_Put emp = db.Product_Put.Find(emp_id);
                    db.Product_Put.Remove(emp);
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
        /// 商品库存管理-添加出库单
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialDeliveryList()
        {
            //查询库存产品信息
            var pDeliver = db.Goods_inventory
                .Where(p => p.Inventory_type == "产品")
                .OrderByDescending(i => i.InventoryId);
            ViewBag.Delivery = pDeliver;
            //查询采购单信息
            var sale = db.Sales;
            ViewBag.sale = sale;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialDeliveryListLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Goods_inventory> model = db.Goods_inventory.OrderByDescending(a => a.Invenid).Where(a => a.Name.Contains(name)&&a.Inventory_type=="产品").ToList();
            ViewBag.Delivery = model;
            //查询采购单信息
            var sale = db.Sales;
            ViewBag.sale = sale;
            return View("MaterialInventoryManagerAddMaterialDeliveryList");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据id查询库存产品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialDeliveryListByid()
        {
            //获取传输的库存编号
            var invid = Request["invid"];
            //查询
            var goodinven = db.Goods_inventory.Find(int.Parse(invid));
            var obj = new
            {
                invids = invid,
                productname=goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Productsname,
                address = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Address,
                price = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Price,
                unit = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Unit,
                guige = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Specifications,
                packing = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Packaging,
                pihao = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Titanic,
                productype = goodinven.Product_Put.Product.PlanOrder.OrderManager.Products.Lnven.Invenname,
                inennunber=goodinven.Inventory
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询销售单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectSaleByid()
        {
            //获取值
            var saleid = Request["num"];
            //查询
            var sale = db.Sales.Find(int.Parse(saleid));
            var obj = new
            {
                salecount=sale.Sales_number
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加产品出库信息
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerAddMaterialDeliveryListAdd(Goods_Delivery gd)
        {
            //实例库存信息
            Goods_inventory gi = db.Goods_inventory.Find(gd.GoodsInvenid);
            //判断出库数量是否大于库存数量
            if (gd.Outbound_number > gi.Inventory)
            {
                return Content("<script>alert('出库数量不能大于库存数量');window.location.href='/Management/MaterialInventoryManagerMaterialDeliveryListManagemen';</script>");
            }
            else
            {
                //保存到对象
                db.Goods_Delivery.Add(gd);
                //保存到数据库
                if (db.SaveChanges() > 0)
                {
                    //计算数量
                    gi.Inventory -= gd.Outbound_number;
                    db.SaveChanges();
                    return RedirectToAction("MaterialInventoryManagerMaterialDeliveryListManagemen");
                }
                else
                {
                    return Content("<script>alert('出库失败')</script>");
                }
            }
           
        }
        /// <summary>
        /// 企业库存管理-出库单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerMaterialDeliveryListManagemen()
        {
            //查询出库单信息
            var productout = db.Goods_Delivery
                .OrderByDescending(p => p.DeliveryId);
            ViewBag.pduout = productout;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerMaterialDeliveryListManagemenLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Goods_Delivery> model = db.Goods_Delivery.OrderByDescending(a => a.DeliveryId).Where(a => a.productname.Contains(name)).ToList();
            ViewBag.pduout = model;
            return View("MaterialInventoryManagerMaterialDeliveryListManagemen");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据出库单id查询出库单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerMaterialDeliveryListManagemenByid()
        {
            //获取值
            var deliverid = Request["pid"];
            //查询
            var prod = db.Goods_Delivery.Find(int.Parse(deliverid));
            var obj = new
            {
                DeliveryId=prod.DeliveryId,
                productname=prod.productname,
                Delivery_date=prod.Delivery_date.ToString(),
                Outbound_number=prod.Outbound_number,
                GoodsInvenid=prod.GoodsInvenid,
                Handlers=prod.Handlers,
                Usersid=prod.Usersid,
                Tips=prod.Tips
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除出库单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MaterialInventoryManagerMaterialDeliveryListManagemenDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表库存信息
                //var goodsinven = db.Goods_inventory
                //    .Where(g => g.Invenid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goodsinven == null)
                //{

                //}
                //else
                //{
                Goods_Delivery emp = db.Goods_Delivery.Find(emp_id);
                db.Goods_Delivery.Remove(emp);
                //}
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
        /// 往来管理-销售结款单
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentManagementSalesStatement()
        {
            //查询结款单
            var godsalebro = db.Sales_Brrowing
                .OrderByDescending(s => s.SalesId);
            ViewBag.salebro = godsalebro;
            //查询销售单
            var sale = db.Sales
                .Where(s => s.Salse_state != "已结款");
            ViewBag.sale = sale;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementSalesStatementLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Sales_Brrowing> model = db.Sales_Brrowing.OrderByDescending(a => a.Sales_brrowid).Where(a => a.Sales.Products.Productsname.Contains(name)||a.Sales.Customer.Customername.Contains(name)).ToList();
            ViewBag.salebro = model;
            //查询销售单
            var sale = db.Sales
                .Where(s => s.Salse_state != "已结款");
            ViewBag.sale = sale;
            return View("CurrentManagementSalesStatement");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 查询销售单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectOrderdsfdfsbyid()
        {
            //获取id
            var ords = Request["sid"];
            //查询
            var orderin = db.Sales.Find(int.Parse(ords));
            var obj = new
            {
                salenumber=orderin.Sales_number,
                Amount_payable=orderin.SumPrice,
                Real_pay=orderin.Real_pay,
                uppay=orderin.Unpaid_amount,
                productname=orderin.Products.Productsname,
                salesnumber=orderin.Sales_number,
                price=orderin.Products.Price
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加销售结款单
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementSalesStatementAdd(Sales_Brrowing sb)
        {
            //判断销售编号，不为0时保存
            if (sb.SalesId != 0)
            {
                //保存对象
                db.Sales_Brrowing.Add(sb);
                if (db.SaveChanges()>0)
                {
                    //修改销售单状态
                    Sales se = db.Sales.Find(sb.SalesId);
                    se.Salse_state = "已结款";
                    se.Real_pay = se.SumPrice;
                    db.SaveChanges();
                    return RedirectToAction("CurrentManagementSalesStatement");
                }
                else
                {
                    return Content("<script>alert('保存销售单失败')</script>");
                }
                
            }
            else
            {
                return Content("<script>alert('添加失败，请选择销售信息')</script>");
            }
            
            
        }
        /// <summary>
        /// 删除结款单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SelectOrderdsfdfsbyidDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表库存信息
                //var goodsinven = db.Goods_inventory
                //    .Where(g => g.Invenid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goodsinven == null)
                //{

                //}
                //else
                //{
                Sales_Brrowing emp = db.Sales_Brrowing.Find(emp_id);
                db.Sales_Brrowing.Remove(emp);
                //}
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
        /// 往来管理-销售退货单
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentManagementSalesInvoice()
        {
            //查询退货单
            var salereturn = db.Purchase_Return.OrderByDescending(p=>p.Purchase_returnid);
            ViewBag.salereturn = salereturn;
            //查询销售产品
            var sale = db.Sales;
            ViewBag.sale = db.Sales;
                
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementSalesInvoiceLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_Return> model = db.Purchase_Return.OrderByDescending(a => a.Purchase_returnid).Where(a => a.Sales.Products.Productsname.Contains(name)||a.Sales.Customer.Customername.Contains(name)).ToList();
            ViewBag.salereturn = model;
            //查询销售产品
            var sale = db.Sales;
            ViewBag.sale = db.Sales;
            return View("CurrentManagementSalesInvoice");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 添加销售退货单
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementSalesInvoiceAdd(Purchase_Return pr)
        {
            //判断销售编号，不为0时保存
            if (pr.Purchaseid != 0)
            {
                //保存对象
                db.Purchase_Return.Add(pr);
                if (db.SaveChanges() > 0)
                {
                    //销售金额进行对应变化
                    //var saleinfo=db.
                    return RedirectToAction("CurrentManagementSalesInvoice");
                }
                else
                {
                    return Content("<script>alert('添加失败')</script>");
                }
            }
            else
            {
                return Content("<script>alert('添加失败，请选择销售信息')</script>");
            }
        }
       /// <summary>
       /// 删除销售退货单
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult CurrentManagementSalesInvoiceDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表库存信息
                //var goodsinven = db.Goods_inventory
                //    .Where(g => g.Invenid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goodsinven == null)
                //{

                //}
                //else
                //{
                Purchase_Return emp = db.Purchase_Return.Find(emp_id);
                db.Purchase_Return.Remove(emp);
                //}
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
        /// 往来管理-采购结款单
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseStatement()
        {
            //查询采购结款单信息
            var purchasinfo = db.Purchase_brrowing
                .OrderByDescending(p => p.Brrowingid);
            ViewBag.brrowing = purchasinfo;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseStatementLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_brrowing> model = db.Purchase_brrowing.OrderByDescending(a => a.Brrowingid).Where(a => a.Product_name.Contains(name)).ToList();
            ViewBag.brrowing = model;
            return View("CurrentManagementPurchaseStatement");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据采购结款单id查询采购结款单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseStatementBYid()
        {
            //获取结款单id
            var goodQuery = Request["browid"];
            //查询
            var goodsbrow = db.Purchase_brrowing.Find(int.Parse(goodQuery));
            var obj = new
            {
                Brrowingid=goodsbrow.Brrowingid,
                Brrowingdate=goodsbrow.Brrowingdate.ToString(),
                Purchaseid=goodsbrow.Purchaseid,
                Product_name=goodsbrow.Product_name,
                Purchase_returnsnumber=goodsbrow.Purchase_returnsnumber,
                Purchase_returnsprice = goodsbrow.Amount_payable,
                Price=goodsbrow.Price,
                Amount_payable=goodsbrow.Amount_payable,
                Real_pay=goodsbrow.Real_pay,
                Unpaid_amount=goodsbrow.Unpaid_amount,
                Handlers=goodsbrow.Handlers,
                Usersid=goodsbrow.Usersid,
                Tips=goodsbrow.Tips
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改采购结款单信息
        /// </summary>
        /// <param name="pb"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseStatementUpdate(Purchase_brrowing pb)
        {
           //查找对象
            var pub = db.Purchase_brrowing.Find(pb.Brrowingid);
            //修改对象
            pub.Brrowingid = pb.Brrowingid;
            pub.Amount_payable = pb.Amount_payable;
            pub.Brrowingdate = pb.Brrowingdate;
            pub.Handlers = pb.Handlers;
            pub.Price = pb.Price;
            pub.Product_name = pb.Product_name;
            pub.Purchase_returnsnumber = pb.Purchase_returnsnumber;
            pub.Purchase_returnsprice = pb.Purchase_returnsprice;
            pub.Purchaseid = pb.Purchaseid;
            pub.Real_pay = pb.Real_pay;
            pub.Tips = pb.Tips;
            pub.Unpaid_amount = pb.Unpaid_amount;
            pub.Usersid = pb.Usersid;
            //保存对象
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("CurrentManagementPurchaseStatement");
            }
            else
            {
                return Content("<script>alert('修改失败')</script>");
            }
        }
        /// <summary>
        /// 删除采购结款单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseStatementDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表商品信息
                //var goods = db.Goods
                //    .Where(g => g.Goodsid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goods == null)
                //{

                //}
                //else
                //{
                Purchase_brrowing emp = db.Purchase_brrowing.Find(emp_id);
                db.Purchase_brrowing.Remove(emp);
                //}
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
        /// 往来管理-采购退货单
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseReturns()
        {
            //查询采购退货单信息
            var PurReturn = db.Purchase_returns
                .OrderByDescending(p => p.Purchase_returnsid);
            ViewBag.pureturn = PurReturn;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseReturnsLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_returns> model = db.Purchase_returns.OrderByDescending(a => a.Purchase_returnsid).Where(a => a.Product_name.Contains(name)).ToList();
            ViewBag.pureturn = model;
            return View("CurrentManagementPurchaseReturns");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 删除采购退货单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CurrentManagementPurchaseReturnsDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表商品信息
                //var goods = db.Goods
                //    .Where(g => g.Goodsid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goods == null)
                //{

                //}
                //else
                //{
                Purchase_brrowing emp = db.Purchase_brrowing.Find(emp_id);
                db.Purchase_brrowing.Remove(emp);
                //}
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
        /// 销售管理-销售单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesManagementSalesOrderManagement()
        {
            //查询销售单信息
            var Sale = db.Sales
                .OrderByDescending(s => s.SalesId);
            ViewBag.sales = Sale;
            //查询订单信息
            var orders = db.OrderManager;
            ViewBag.order = orders;
            return View();
        }
        
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult SalesManagementSalesOrderManagementLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Sales> model = db.Sales.OrderByDescending(a => a.SalesId).Where(a => a.Products.Productsname.Contains(name)).ToList();
            ViewBag.sales = model;
            //查询订单信息
            var orders = db.OrderManager;
            ViewBag.order = orders;
            return View("SalesManagementSalesOrderManagement");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据订单编号查询订单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ordermangerByid()
        {
            //获取选择的订单编号
            var orderid = Request["orderid"];
            //查询
            var orderinfo = db.OrderManager.Find(int.Parse(orderid));
            //定义
            var obj = new
            {
                productsname=orderinfo.Products.Productsname,
                customername=orderinfo.Customer.Customername,
                Customersid=orderinfo.CustomerID,
                Sales_number=orderinfo.Ordercount,
                SumPrice=orderinfo.Order_price,
                Amount_payable=orderinfo.Order_price,
                ProductsId=orderinfo.Products.Productsid,
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加销售单
        /// </summary>
        /// <param name="sales"></param>
        /// <returns></returns>
        public ActionResult SalesManagementSalesOrderManagementADD(Sales sales)
        {
            //保存数据库
            db.Sales.Add(sales);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("SalesManagementSalesOrderManagement");
            }
            else
            {
                return Content("<script>alert('添加成功')</script>");
            }
        }
        /// <summary>
        /// 根据id查询销售单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectSalesByid()
        {
            //获取传输的值
            var salid = Request["sid"];
            //查询
            var saleinfo = db.Sales.Find(int.Parse(salid));
            var obj = new
            {
                SalesId=saleinfo.SalesId,
                Sales_date1=saleinfo.Sales_date.ToString(),
                productname=saleinfo.Products.Productsname,
                customername2=saleinfo.Customer.Customername,
                salecount=saleinfo.Sales_number,
                price=saleinfo.Products.Price,
                readonlyprice=saleinfo.Real_pay,
                orderid=saleinfo.ProductsId,
                sumprice=saleinfo.SumPrice,
                amoutprice=saleinfo.Amount_payable,
                upprice=saleinfo.Unpaid_amount,
                handle=saleinfo.Handlers,
                userid=saleinfo.Users.Usersname
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除销售单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SalesManagementSalesOrderManagementDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表销售结款单信息
                var salebro = db.Sales_Brrowing
                    .Where(g => g.SalesId == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (salebro == null)
                {

                }
                else
                {
                    //查询销售退货单信息
                    var saleout = db.Sales_delivery
                        .Where(s => s.Salesid == emp_id).ToList();
                    if (saleout == null)
                    {
                    }
                    else
                    {
                        Sales emp = db.Sales.Find(emp_id);
                        db.Sales.Remove(emp);
                    }
               
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
        /// 销售管理-销售订单
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesManagementPendingOrder()
        {
            return View();
        }
         /// <summary>
        /// 销售管理-销售统计
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesManagementSalesStatistics()
        {
            return View();
        }
        /// <summary>
        /// 质量管理-待检材料商品
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementMaterialsToBeInspected()
        {
            //查询采购单信息
            //PurchasList("待质检");
            var purchas = db.Purchase_order
                .Where(p => p.State == "待质检")
                .OrderByDescending(g => g.Purchase_orderid);
            ViewBag.purchas = purchas;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult QualityManagementMaterialsToBeInspectedLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Purchase_order> model = db.Purchase_order.OrderByDescending(a => a.Purchase_orderid).Where(a => a.Goods.Goodsname.Contains(name)&&a.State=="待质检").ToList();
            ViewBag.purchas = model;
            return View("QualityManagementMaterialsToBeInspected");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 据id查询采购单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementMaterialsToBeInspectedBYid()
        {
            //获取采购单id
            var purchasid = Request["Purchasid"];
            //查询数据
            var purchasinfo = db.Purchase_order.Find(int.Parse(purchasid));
            var onj = new
            {
                purchasid=purchasinfo.Purchase_orderid,
                goodsname=purchasinfo.Goods.Goodsname
            };
            return Json(onj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加商品质检单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementMaterialsToBeInspectedAdd(Good_Quality gQ)
        {
            //保存到对象
            db.Good_Quality.Add(gQ);
            //保存到商品质检表
            if (db.SaveChanges() > 0)
            {
                //查询本id对应采购单信息
                var purchasinfo = db.Purchase_order.Find(gQ.Purchase_orderid);
                //判断质检结果，0为合格，1为不合格
                if (gQ.Quality_state == "0")
                {
                    //合格进行下一步，付款
                    purchasinfo.State = "待付款";
                }
                else
                {
                    //不合格状态为，待退货
                    purchasinfo.State = "待退货";
                }
                //保存采购单信息
                db.SaveChanges();
            }
            return RedirectToAction("QualityManagementMaterialsToBeInspected");
        }
        /// <summary>
        /// 质量管理-材料商品质检单
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementMaterialInspectionList()
        {
            //查询质检商品信息
            var purcharQuery = db.Good_Quality
                .OrderByDescending(p => p.QualityId);
            ViewBag.GoodsQuery = purcharQuery;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult QualityManagementMaterialInspectionListLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Good_Quality> model = db.Good_Quality.OrderByDescending(a => a.QualityId).Where(a => a.Goodsname.Contains(name)).ToList();
            ViewBag.GoodsQuery = model;
            return View("QualityManagementMaterialInspectionList");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 删除材料商品质检单
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryManagementDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表商品信息
                //var goods = db.Goods
                //    .Where(g => g.Goodsid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goods == null)
                //{

                //}
                //else
                //{
                Good_Quality emp = db.Good_Quality.Find(emp_id);
                db.Good_Quality.Remove(emp);
                //}
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
        /// 质量管理-待检产品
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementWaitingForTheProduct()
        {
            //查询待检产品信息
            var productinfo = db.Product
                .Where(p => p.State == "待质检")
                .OrderByDescending(o=>o.Productid)
                .ToList();
            ViewBag.productinfo = productinfo;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult QualityManagementWaitingForTheProductLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Product> model = db.Product.OrderByDescending(a => a.Productid).Where(a => a.Product_name.Contains(name)&&a.State=="待质检").ToList();
            ViewBag.productinfo = model;
            return View("QualityManagementWaitingForTheProduct");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 根据id查询产品生产信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementWaitingForTheProductBYid()
        {
            //获取传输过来的id
            var productid = Request["proid"];
            //查询产品生产单
            var Product = db.Product.Find(int.Parse(productid));
            var obj = new
            {
                productid=Product.Productid,
                productname=Product.Product_name
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加产品质检单
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>
        public ActionResult QualityManagementWaitingForTheProductAdd(Product_Quality productQuery)
        {
            //保存到对象
            db.Product_Quality.Add(productQuery);
            //保存到数据库
            if (db.SaveChanges() > 0)
            {
                //查询生产单信息
                var productinfo = db.Product.Find(productQuery.Product_orderid);
                //判断质检结果
                if (productQuery.Quality_end == 1)
                {
                    //为不合格时修改入库数量
                    productinfo.Productcount -= productQuery.Quality_number;
                }
                productinfo.State = "待入库";
                db.SaveChanges();

                return RedirectToAction("QualityManagementWaitingForTheProduct");
            }
            else
            {
                return Content("<script>alert('添加失败')</script>");
            }
        }
        /// <summary>
        /// 质量管理-产品质检单
        /// </summary>
        /// <returns></returns>
        public ActionResult QualityManagementProductQualityCheckList()
        {
            //查询质检单信息
            var proQuery = db.Product_Quality
                .OrderByDescending(p => p.QualityId);
            ViewBag.ProQuery = proQuery;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult QualityManagementProductQualityCheckListLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Product_Quality> model = db.Product_Quality.OrderByDescending(a => a.QualityId).Where(a => a.Product_name.Contains(name)).ToList();
            ViewBag.ProQuery = model;
            return View("QualityManagementProductQualityCheckList");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 删除产品质检单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult QualityManagementProductQualityCheckListDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                ////查询从表商品信息
                //var goods = db.Goods
                //    .Where(g => g.Goodsid == emp_id).ToList();
                ////如果商品表中存在此供应商信息
                //if (goods == null)
                //{

                //}
                //else
                //{
                Product_Quality emp = db.Product_Quality.Find(emp_id);
                db.Product_Quality.Remove(emp);
                //}
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
        /// 基础信息-供应商信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementSupplier()
        {
            //查询供应商信息
            List<Supplier> supplier = db.Supplier
                .OrderByDescending(p=>p.SupplierId)
                .ToList();
            ViewBag.Supplierlist = supplier;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementSupplierLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Supplier> model = db.Supplier.OrderByDescending(a => a.SupplierId).Where(a => a.Supplier_name.Contains(name)).ToList();
            ViewBag.Supplierlist = model;
            return View("BasicInformationManagementSupplier");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementSupplierAdd(Supplier supplier)
        {
            //保存数据到对象
            db.Supplier.Add(supplier);
            //保存到数据库
            if (db.SaveChanges() > 0)
            {
               //return Content("<script>alert('添加成功');</script>");
               return RedirectToAction("BasicInformationManagementSupplier");
            }
            else
            {
                return Content("<script>alert('添加失败');</script>");
            }        
        }
        /// <summary>
        /// 根据id查询供应商信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementSupplierByID()
        {
            //获取供应商id
            var Supid = Request["Supid"];
            //根据id查询供应商信息
            var Sup=db.Supplier.Find(int.Parse(Supid));
            //返回数据
            var obj = new
            {
                Supperlier=Sup.SupplierId,
                Supplier_name=Sup.Supplier_name,
                Supplier__Introduction=Sup.Supplier__Introduction,
                Postal_code=Sup.Postal_code,
                Credibility=Sup.Credibility,
                Quality_state=Sup.Quality_state,
                Contact=Sup.Contact,
                Phone=Sup.Contact,
                Tel=Sup.Tel,
                Deposit_bank=Sup.Deposit_bank,
                Bank_number=Sup.Bank_number,
                Fax=Sup.Fax,
                E_mail=Sup.E_mail
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="supper"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementSupplierUpdate(Supplier supper)
        {
            //根据id查询供应商信息
            var Super = db.Supplier.Find(supper.SupplierId);
            Super.SupplierId = supper.SupplierId;
            Super.Supplier_name = supper.Supplier_name;
            Super.Supplier__Introduction = supper.Supplier__Introduction;
            Super.Phone = supper.Phone;
            Super.Postal_code = supper.Postal_code;
            Super.Quality_state = supper.Quality_state;
            Super.Tel = supper.Tel;
            Super.Bank_number = supper.Bank_number;
            Super.Deposit_bank = supper.Deposit_bank;
            Super.Fax = supper.Fax;
            Super.E_mail = supper.E_mail;
            Super.Credibility = supper.Credibility;
            Super.Contact = supper.Contact;
            //保存到数据库
            db.SaveChanges();
            return RedirectToAction("BasicInformationManagementSupplier");
        }
        /// <summary>// 删除供应商
        /// 删除供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult delemp(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表商品信息
                var goods = db.Goods
                    .Where(g => g.Goodsid == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (goods == null)
                {

                }
                else
                {
                    Supplier emp = db.Supplier.Find(emp_id);
                    db.Supplier.Remove(emp);
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
        /// 基础信息-商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementCommodityInformation()
        {
            //查询商品
            var goods = db.Goods
                .OrderByDescending(p=>p.Goodsid)
                .ToList();
            ViewBag.Goods = goods;
            //查询供应商信息
            var supplier = db.Supplier.ToList();
            ViewBag.Supplierlist = supplier;
            //查询商品类别信息
            var Lven = db.Lnven.ToList();
            ViewBag.Lven = Lven;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementCommodityInformationLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Goods> model = db.Goods.OrderByDescending(a => a.Goodsid).Where(a => a.Goodsname.Contains(name)).ToList();
            ViewBag.Goods = model;
            //查询供应商信息
            var supplier = db.Supplier.ToList();
            ViewBag.Supplierlist = supplier;
            //查询商品类别信息
            var Lven = db.Lnven.ToList();
            ViewBag.Lven = Lven;
            return View("BasicInformationManagementCommodityInformation");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementCommodityInformationAdd(Goods goods)
        {
            //判断是否有选择供应商与商品类别
            if (goods.SupplierId != 0 && goods.Categoryid != 0)
            {

                //保存到对象
                db.Goods.Add(goods);
                //保存到数据库
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("BasicInformationManagementCommodityInformation");
                }
                else
                {
                    return Content("<script>alert('添加失败');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('商品信息不完善，请确认填写的信息');</script>");
                return RedirectToAction("BasicInformationManagementCommodityInformation");
            }
        }
        /// <summary>
        /// 根据商品id查询商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementCommodityInformationByID()
        {
            //获取传输过来的商品id
            var Goodsid = Request["GoodsID"];
            var GoodsInfo = db.Goods.Find(int.Parse(Goodsid));
            //返回数据
            var obj = new
            {
                Goodsid=GoodsInfo.Goodsid,
                Goodsname=GoodsInfo.Goodsname,
                Goodsjieche=GoodsInfo.Goodsjieche,
                Unit=GoodsInfo.Unit,
                Price=GoodsInfo.Price,
                Origin=GoodsInfo.Origin,
                Specifications=GoodsInfo.Specifications,
                Packaging=GoodsInfo.Packaging,
                Batch_number=GoodsInfo.Batch_number,
                Titanic=GoodsInfo.Titanic,
                SupplierId=GoodsInfo.SupplierId,
                Categoryid=GoodsInfo.Categoryid,
                Tips=GoodsInfo.Tips
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementCommodityInformationUpdate(Goods goods)
        {
            var Goods = db.Goods.Find(goods.Goodsid);
            Goods.Goodsname = goods.Goodsname;
            Goods.Origin = goods.Origin;
            Goods.Batch_number = goods.Batch_number;
            Goods.Titanic = goods.Titanic;
            Goods.Goodsjieche = goods.Goodsjieche;
            Goods.Specifications = goods.Specifications;
            Goods.Unit = goods.Unit;
            Goods.Packaging = goods.Packaging;
            Goods.SupplierId = goods.SupplierId;
            Goods.Price = goods.Price;
            Goods.Categoryid = goods.Categoryid;
            //保存数据库
            db.SaveChanges();
            return RedirectToAction("BasicInformationManagementCommodityInformation");
        }
        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodsDelete(List<int?> id)
        {
            foreach (var emp_id in id)
            {
                //查询从表商品信息
                var goods = db.Goods
                    .Where(g => g.SupplierId == emp_id).ToList();
                //如果商品表中存在此供应商信息
                if (goods == null)
                {

                }
                else
                {
                    Supplier emp = db.Supplier.Find(emp_id);
                    db.Supplier.Remove(emp);
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
        /// 基础信息-产品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementProductInformation()
        {
            //查询产品信息
            var Productinfo = db.Products
                .OrderByDescending(p=>p.Productsid)
                .ToList();
            ViewBag.Productlist = Productinfo;
            //查询商品类别信息
            var Lven = db.Lnven.ToList();
            ViewBag.Lven = Lven;
            return View();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementProductInformationLike(string name)
        {
            name = Request["tj"];//前端input传过来的值
            List<Products> model = db.Products.OrderByDescending(a => a.Productsid).Where(a => a.Productsname.Contains(name)).ToList();
            ViewBag.Productlist = model;
            //查询商品类别信息
            var Lven = db.Lnven.ToList();
            ViewBag.Lven = Lven;
            return View("BasicInformationManagementProductInformation");//此视图为模糊查询所在的页面
        }
        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementProductInformationAdd(Products product)
        {
            
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("BasicInformationManagementProductInformation");
        }
        /// <summary>
        /// 根据产品id查询产品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInformationManagementProductInformationByID()
        {
            var productid = Request["Product"];
            var Produinfo=db.Products.Find(int.Parse(productid));
            //返回数据
            var obj = new
            {
                Productsid = Produinfo.Productsid,
                Productsname = Produinfo.Productsname,
                Productsjieche = Produinfo.Productsjieche,
                Unit = Produinfo.Unit,
                Address = Produinfo.Address,
                Specifications = Produinfo.Specifications,
                Packaging = Produinfo.Packaging,
                Batch_number = Produinfo.Batch_number,
                Titanic = Produinfo.Titanic,
                Price=Produinfo.Price,
                SupplierId = Produinfo.SupplierId,
                Category = Produinfo.Category,
                Tips = Produinfo.Tips
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public ActionResult BasicInformationManagementProductInformationUpdate(Products products)
        {
            var Productinfo = db.Products.Find(products.Productsid);
            Productinfo.Productsname = products.Productsname;
            Productinfo.Productsjieche = products.Productsjieche;
            Productinfo.Batch_number = products.Batch_number;
            Productinfo.Unit = products.Unit;
            Productinfo.Address = products.Address;
            Productinfo.Specifications = products.Specifications;
            Productinfo.Packaging = products.Packaging;
            Productinfo.Batch_number = products.Batch_number;
            Productinfo.Titanic = products.Titanic;
            Productinfo.Price = products.Price;
            Productinfo.SupplierId = products.SupplierId;
            Productinfo.Category = products.Category;
            //保存数据库
            db.SaveChanges();
            return RedirectToAction("BasicInformationManagementProductInformation");
        }
    }
}
