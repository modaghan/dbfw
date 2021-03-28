using MS.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Resources;

namespace MSDAL
{
    public static class Extensions
    {
        /// <summary>
        /// Gets Active entities
        /// </summary>
        /// <typeparam name="T">T Type of entity</typeparam>
        /// <param name="list"><typeparam name="T"/> List</param>
        /// <param name="is_active_prop">Active Property</param>
        /// <returns>Not Deleted <typeparam name="T"/> List</returns>
        public static List<T> Actives<T>(this List<T> list, string is_active_prop = "is_active")
        {
            return list.Where(e => e.GetType().GetProperty(is_active_prop).GetValue(e).ToString().ToLower().Equals("true")).ToList();
        }

        /// <summary>
        /// Get Entity by Id
        /// </summary>
        /// <typeparam name="T">T Type of entity</typeparam>
        /// <param name="list"><typeparam name="T"/> List</param>
        /// <param name="id">Primary Key of <typeparam name="T"/></param>
        /// <returns><typeparam name="T"/></returns>
        public static T GetById<T>(this List<T> list, long? id)
        {
            if (list == null || id == null || id == 0)
                return default(T);
            T obj = list.FirstOrDefault(t => t.GetType().GetProperty("id").GetValue(t).ToString().Equals(id.ToString()));
            return obj;
        }
        public static T GetById<T>(this List<T> list, object foreign, string foreign_key)
        {
            if (list == null || foreign == null || foreign_key == null)
                return default(T);

            // Getting Foreign Key
            Type foreignType = foreign.GetType();
            object foreign_key_value = foreignType.GetProperty(foreign_key).GetValue(foreign);
            if (foreign_key_value == null)
                return default(T);

            T obj = list.FirstOrDefault(t => t.GetType().GetProperty("id").GetValue(t).Equals(foreign_key_value));
            if (obj == null)
                return default(T);
            return obj;
        }
        /// <summary>
        /// Get de
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="return_prop"></param>
        /// <param name="default_return"></param>
        /// <returns>Defaulted String of T Entity</returns>
        public static string GetById<T>(this List<T> list, long? id, string return_prop = null, string default_return = "-")
        {
            if (list == null || id == null || id == 0)
                return default_return;
            T obj = list.FirstOrDefault(t => t.GetType().GetProperty("id").GetValue(t).ToString().Equals(id.ToString()));
            if (obj == null)
                return default_return;
            if (return_prop == null)
                return obj.ToString();
            object value = typeof(T).GetProperty(return_prop).GetValue(obj);
            if (value == null)
                return default_return;
            return value.ToString();
        }
        public static string GetById<T>(this List<T> list, object foreign, string foreign_key, string return_prop = null, string default_return = "-")
        {
            // Handling null attrs
            if (list == null || foreign == null || foreign_key == null)
                return default_return;

            // Getting Foreign Key
            Type foreignType = foreign.GetType();
            object foreign_key_value = foreignType.GetProperty(foreign_key).GetValue(foreign);
            if (foreign_key_value == null)
                return default_return;

            // Getting T
            T obj = list.FirstOrDefault(t => t.GetType().GetProperty("id").GetValue(t).Equals(foreign_key_value));
            if (obj == null)
                return default_return;
            if (return_prop == null)
                return obj.ToString();

            // Getting return value
            object value = typeof(T).GetProperty(return_prop).GetValue(obj);
            if (value == null)
                return default_return;
            return value.ToString();
        }
        public static T GetByForeignId<T>(this List<T> list, string foreign_key, long? id)
        {
            T obj = list.FirstOrDefault(t => t.GetType().GetProperty(foreign_key).GetValue(t).ToString().Equals(id.ToString()));
            return obj;
        }
        public static string GetForeignString<T>(this List<T> list, string foreign_key, object id)
        {
            if (id == null)
                return "-";
            long foreign_id = 0;
            long.TryParse(id.ToString(), out foreign_id);
            T foreign = list.GetById(foreign_id);
            if (foreign == null)
                return "-";
            return foreign.ToString();
        }
        public static List<T> GetMultiByForeignId<T>(this List<T> list, string foreign_key, long? id)
        {
            List<T> obj = list.Where(t => t.GetType().GetProperty(foreign_key).GetValue(t).ToString().Equals(id.ToString())).ToList();
            return obj;
        }

        public static string GetRes(this ResourceManager RM, object key)
        {
            try
            {
                if (key == null)
                    return "-";
                return RM.GetString(key.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "-";
            }
        }
    }
}
