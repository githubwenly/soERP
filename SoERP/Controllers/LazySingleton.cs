using SoERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using SoERP.Models;

namespace SoERP.Controllers
{
    public class LazySingleton
    {
        //单例模式
        private static LazySingleton instance = null;
        //运行时的辅助对象
        private static readonly object syncRoot = new object();
        private LazySingleton() { }

        public static LazySingleton GetInstance()
        {
            //第一重判断，判断实例是否存在，不存在再加锁
            if (instance == null)
            {
                //加锁程序，在同一时刻只能有一个访问
                lock (syncRoot)
                {
                    //第二重判断
                    if (instance == null)
                    {
                        instance = new LazySingleton();
                    }
                }
            }
            return instance;
        }
        //{ { 0, "事假" }, { 1, "病假" }, { 2, "产假" }, { 3, "丧假" }, { 4, "其它" } };
        Dictionary<int, string> leaveType = new Dictionary<int, string>
        {
            [1] = "事假",
            [2] = "病假",
            [3] = "产假",
            [4] = "丧假",
            [5] = "其它"
        };
        public int Post;
    }
}