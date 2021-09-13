using System;
using System.Collections.Generic;
using System.Linq;

using Dapper;
using Dapper.Contrib.Extensions;
using ReportApp.Model;
using System.Data.SQLite;
using System.IO;

namespace ReportApp.Service
{
    public class DAOService
    {
        static string directory = AppDomain.CurrentDomain.BaseDirectory;
        static string dbPath = Path.Combine(directory, "data.db");
        string cnStr = "data source=" + dbPath;

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="orders"></param>
        public int AddOrder(Order order)
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                // 判斷是否存在
                int count = cn.Query<int>(@"SELECT COUNT(*) FROM OrderTable WHERE Number=@Number", new { Number = order.Number }).Single();
                if (count == 0)
                {
                    cn.Insert(order);
                }
                return count;

            }
        }
        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orders"></param>
        public void DeleteOrder(List<Order> orders)
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                foreach (var order in orders)
                {
                    // 刪除order
                    cn.Query<Order>(@"DELETE FROM OrderTable Where Number=@Number", new { Number = order.Number });
                }
            }
        }
        /// <summary>
        /// 取得全部訂單(根據第一個字元排序)
        /// </summary>
        public List<Order> GetOrder()
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                return cn.Query<Order>(@"SELECT * FROM OrderTable order by SUBSTR(Number, 1, 1)").ToList();
            }
        }
        /// <summary>
        /// 取得相似訂單流水號
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int GetSimilarOrder(Order order)
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                return cn.Query<int>(@"SELECT COUNT(*) FROM OrderTable WHERE Number LIKE @Number", new { Number = order.Number, Lenth = order.Number.Length }).Single();
            }
        }
        /// <summary>
        /// 取得相同訂單流水號
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int GetSameOrder(Order order)
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                return cn.Query<int>(@"SELECT COUNT(*) FROM OrderTable WHERE Number=@Number", new { Number = order.Number }).Single();
            }
        }

    }
}
