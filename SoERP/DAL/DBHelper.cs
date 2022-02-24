using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SoERP.DAL
{
    public class DBHelper
    {
        private static string constr = "server = .;database=ERPDB;uid=sa;pwd=123456";
        private static SqlConnection con = new SqlConnection(constr);
        //查询
        public static DataTable get(string sql)
        {
            try
            {
                //如果连接状态为关闭
                if (con.State != ConnectionState.Open)
                {
                    con.Open();//打开数据库连接
                }
                DataTable dt = new DataTable();//实例化DataTable对象dt
                                               //实例化SqlDataAdapter对象
                SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
                //执行sql语句，查询结果填充到dt对象
                adapter.Fill(dt);
                //返回dt
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }

        //新增，修改，删除
        public static bool exe(string sql)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //实例化SqlCommand对象
                SqlCommand command = new SqlCommand(sql, con);
                //执行sql语句，如果影响的行数大于0，数据库操作成功
                return command.ExecuteNonQuery() > 0;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }       
    }
}