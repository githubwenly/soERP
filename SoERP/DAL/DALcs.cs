using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SoERP.DAL;

namespace SoERP.DAL
{
    public class DALcs
    {
        //查询月收入
        public static string selsal()
        {
            string uname = "";
            string sql = "select sum(SumPrice) sump from Sales where DateDiff(mm,Sales_date,getdate())=0";
            DataTable dt = DBHelper.get(sql);
            //2021-12-18 添加没有月收入时的判断
            foreach (DataRow item in dt.Rows)
            {
                if (item["sump"] == DBNull.Value)
                {
                    uname = "0";
                }
                else
                {
                    double sal = double.Parse(item["sump"].ToString()) * 0.0001;
                    uname = sal.ToString();
                }
            }
            return uname;
        }
        //查询月支出
        public static string selpur()
        {
            string uname = "";
            string sql = "select sum(Amount_payable) suma from Purchase_brrowing where DateDiff(mm,Brrowingdate,getdate())=0";
            DataTable dt = DBHelper.get(sql);
            //2021-12-18 添加没有月支出时的判断
            foreach (DataRow item in dt.Rows)
            {
                if (item["suma"] == DBNull.Value)
                {
                    uname = "0";
                }
                else
                {
                    double sal = double.Parse(item["suma"].ToString()) * 0.0001;
                    uname = sal.ToString();
                }   
            }
            return uname;
        }
        //查询订单总数
        public static string selord()
        {
            string uname = "";
            string sql = "select sum(Ordercount) sumo from OrderManager ";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["sumo"].ToString();
            }
            return uname;
        }
        //查询员工总数
        public static string selemp()
        {
            string uname = "";
            string sql = "select count(EmployeesId) sums from Employees";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["sums"].ToString();
            }
            return uname;
        }
        //查询三月销售总额
        public static string selmar()
        { 
            string uname = "";
            string sql = "select sum(SumPrice) sump from Sales where DateDiff(mm,Sales_date,'2019-06-30')=3";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["sump"].ToString();
            }
            return uname;
        }
        //查询四月销售总额
        public static string selapr()
        {
            string uname = "";
            string sql = "select sum(SumPrice) sump from Sales where DateDiff(mm,Sales_date,'2019-06-30')=2";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["sump"].ToString();
            }
            return uname;
        }
        //查询五月销售总额
        public static string selmay()
        {
            string uname = "";
            string sql = "select sum(SumPrice) sump from Sales where DateDiff(mm,Sales_date,'2019-06-30')=1";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["sump"].ToString();
            }
            return uname;
        }
        //查询六月销售总额
        public static string seljun()
        {
            string uname = "";
            string sql = "select sum(SumPrice) sump from Sales where DateDiff(mm,Sales_date,'2019-06-30')=0";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["sump"].ToString();
            }
            return uname;
        }
        //查询三销售总额
        public static string selram()
        {
            string uname = "";
            string sql = "select sum(Amount_payable) suma from Purchase_brrowing where DateDiff(mm,Brrowingdate,'2019-06-30')=3";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["suma"].ToString();
            }
            return uname;
        }
        //查询四月销售总额
        public static string selrpa()
        {
            string uname = "";
            string sql = "select sum(Amount_payable) suma from Purchase_brrowing where DateDiff(mm,Brrowingdate,'2019-06-30')=2";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["suma"].ToString();
            }
            return uname;
        }
        //查询五月销售总额
        public static string selyam()
        {
            string uname = "";
            string sql = "select sum(Amount_payable) suma from Purchase_brrowing where DateDiff(mm,Brrowingdate,'2019-06-30')=1";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["suma"].ToString();
            }
            return uname;
        }
        //查询六月销售总额
        public static string selnuj()
        {
            string uname = "";
            string sql = "select sum(Amount_payable) suma from Purchase_brrowing where DateDiff(mm,Brrowingdate,'2019-06-30')=0";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["suma"].ToString();
            }
            return uname;
        }
        //销售冠军
        public static string fis()
        {
            string uname = "";
            string sql = "select top 1 Ordercount from OrderManager order by Ordercount desc";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["Ordercount"].ToString();
            }
            return uname;
        }
        public static string fisname()
        {
            string uname = "";
            string sql = "select top 1 Productsname,Ordercount from OrderManager o,Products p where o.ProductID=p.Productsid order by Ordercount desc";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["Productsname"].ToString();
            }
            return uname;
        }
        //销售季军
        public static string sec()
        {
            string uname = "";
            string sql = "select top 1 Ordercount from OrderManager where Ordercount <> (select MAX(Ordercount) from OrderManager) order by Ordercount desc";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["Ordercount"].ToString();
            }
            return uname;
        }
        public static string secname()
        {
            string uname = "";
            string sql = "select top 1 Productsname,Ordercount from OrderManager o,Products p where Ordercount <> (select MAX(Ordercount) from OrderManager) and o.ProductID=p.Productsid order by Ordercount desc";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["Productsname"].ToString();
            }
            return uname;
        }
        //销售殿军
        public static string thi()
        {
            string uname = "";
            string sql = "select top 1 Ordercount from OrderManager where Ordercount not in (select top 2 Ordercount from OrderManager order by Ordercount desc) order by Ordercount desc";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["Ordercount"].ToString();
            }
            return uname;
        }
        public static string thiname()
        {
            string uname = "";
            string sql = "select top 1 Productsname,Ordercount from OrderManager o,Products p where Ordercount not in (select top 2 Ordercount from OrderManager order by Ordercount desc) and o.ProductID=p.Productsid order by Ordercount desc";
            DataTable dt = DBHelper.get(sql);
            foreach (DataRow item in dt.Rows)
            {
                uname = item["Productsname"].ToString();
            }
            return uname;
        }
    }
}