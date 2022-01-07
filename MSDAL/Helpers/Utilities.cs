using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MS.BLL
{
    public static class Utilities
    {
        public static ConnectionState ConnectionState
        {
            get
            {
                ServerCredentials sc = Credentials.ServerCredentials().Clone();
                sc.ConnectTimeout = 10;
                if (string.IsNullOrEmpty(sc.DataSource) ||
                    string.IsNullOrEmpty(sc.InitialCatalog) ||
                    string.IsNullOrEmpty(sc.UserID) ||
                    string.IsNullOrEmpty(sc.Password))
                    return ConnectionState.Broken;
                return IsConnectionStringValid(sc.ToConnectionString()) == null ? ConnectionState.Open : ConnectionState.Closed;
            }
        }
        public static T Clone<T>(this T source)
        {
            T result = Activator.CreateInstance<T>();
            foreach (PropertyInfo property in source.GetType().GetProperties())
            {
                if (property.GetCustomAttribute(typeof(NotMappedAttribute)) != null || property.SetMethod == null)
                    continue;
                property.SetValue(result, property.GetValue(source));
            }
            return result;
        }
        public static T Copy<T>(this object source)
        {
            T result = Activator.CreateInstance<T>();
            foreach (PropertyInfo property in source.GetType().GetProperties())
            {
                if (property.Name.Equals("id"))
                    property.SetValue(result, 0);
                else
                    property.SetValue(result, property.GetValue(source));
            }
            return result;
        }
        public static bool IsEqual(this object source, object target)
        {
            foreach (PropertyInfo property in source.GetType().GetProperties())
            {
                var valSource = property.GetValue(source);
                var valTarget = property.GetValue(target);
                if (valSource == null && valTarget == null)
                    continue;
                else if (valSource == null && valTarget != null)
                    return false;
                else if (valSource != null && valTarget == null)
                    return false;
                else if (valSource != valTarget)
                    return false;
                else
                    continue;
            }
            return true;
        }
        public static T Transfer<T>(this T source, T target)
        {
            if (target == null)
                target = Activator.CreateInstance<T>();
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                try
                {
                    if (property.GetCustomAttribute(typeof(NotMappedAttribute)) != null)
                        continue;
                    var value = property.GetValue(source);
                    property.SetValue(target, value);
                }
                catch (Exception ex)
                {

                }
            }
            return target;
        }
        public static string IsConnectionStringValid(string connString)
        {
            using (var l_oConnection = new SqlConnection(connString))
            {
                try
                {
                    l_oConnection.Open();
                    l_oConnection.Close();
                    return null;
                }
                catch (SqlException sEx)
                {
                    return sEx.Message;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public static Task<bool> HasNetworkConnection
        {
            get
            {
                return Task.Run(() =>
                {
                    try
                    {
                        using (var client = new WebClient())
                        using (client.OpenRead("http://google.com/generate_204"))
                            return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
            }
        }

        #region Json Conversions
        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        public static string ToJson(this object entity)
        {
            try
            {
                var v = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                });
                return v;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string ToFormatted(this double entity, int precision = 2, string unit = "sn")
        {
            try
            {
                string str = string.Format("{0:N" + precision + "}", entity);
                return str;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region Base64 Conversions
        public static string FromBase64(this string base64)
        {
            try
            {
                if (base64 == null)
                    return "";
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string ToBase64(this string str)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region Base64 Json Conversions
        public static T FromBase64Json<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json.FromBase64());
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        public static string ToBase64Json(this object entity)
        {
            try
            {
                return entity.ToJson().ToBase64();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        public static string ToStr(this object obj, string defaultVal = "")
        {
            return obj == null ? defaultVal : obj.ToString();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random(Zaman.Simdi.Millisecond);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public static Expression<Func<TItem, bool>> PropertyEquals<TItem>(PropertyInfo property, object value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, property),
                Expression.Constant(value));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }
        public static bool CanAdd(params string[] strs)
        {
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                    return false;
            }
            return true;
        }
        public static bool CanAdd(this string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }
        public static string ToStr(this string str)
        {
            return str != null ? str : "";
        }
        public static bool ToBool(this bool? b)
        {
            if (b == null)
                return false;
            bool result = false;
            bool.TryParse(b.ToString(), out result);
            return result;
        }
        public static decimal ToDecimal(this string str)
        {
            try
            {
                return Convert.ToDecimal(str.Replace('.', ','));
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static decimal ToDecimal(this decimal? dec)
        {
            try
            {
                return Convert.ToDecimal(dec);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double ToDouble(this double? dob)
        {
            try
            {
                return Convert.ToDouble(dob);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double ToDouble(this object val, double default_value = 0)
        {
            try
            {
                if (val == null)
                    return default_value;
                return Convert.ToDouble(val);
            }
            catch (Exception)
            {
                return default_value;
            }
        }
        public static int ToInteger(this object sayi)
        {
            try
            {
                if (sayi == null)
                    return 0;
                int i = 0;
                int.TryParse(sayi.ToString(), out i);
                return i;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static long ToLong(this object sayi)
        {
            try
            {
                if (sayi == null)
                    return 0;
                long i = 0;
                long.TryParse(sayi.ToString(), out i);
                return i;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static IQueryable<t> OrderByDynamic<t>(this IQueryable<t> query, string sortColumn, bool descending)
        {
            // Dynamically creates a call like this: query.OrderBy(p =&gt; p.SortColumn)
            var parameter = Expression.Parameter(typeof(t), "p");

            string command = "OrderBy";

            if (descending)
            {
                command = "OrderByDescending";
            }

            Expression resultExpression = null;

            var property = typeof(t).GetProperty(sortColumn);
            if (property == null)
                return query;
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // this is the part p =&gt; p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(t), property.PropertyType },
               query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<t>(resultExpression);
        }
        public static string CalculateTotal(decimal? price, decimal? profit, int? vat)
        {
            try
            {
                return string.Format("{0:0.00}", ((price.ToDecimal() + profit.ToDecimal()) * (100 + (vat.ToInteger())) / 100));
            }
            catch (Exception)
            {
                return "-";
            }
        }
        public static string GetAttributeDisplayName(this PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return property.Name;
            return (atts[0] as DisplayAttribute).Name;
        }
        public static string GetMetaDisplayName(this PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return property.Name;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return property.Name;
            return GetAttributeDisplayName(metaProperty);
        }

        public static Color getRandomColor()
        {
            try
            {
                Random rnd = new Random(Zaman.Simdi.Millisecond);
                return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            catch (Exception ex)
            {
                return Color.Black;
            }
        }
        public static String GetHexCode(this System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static String GetRGBCode(this System.Drawing.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }

        public static string ReviewName(string filename)
        {
            return filename.RemoveAccent().Slugify();
        }
        public static string Repair(this string txt)
        {
            return txt.RemoveAccent().Slugify();
        }
        public static string RemoveAccent(this string txt)
        {
            txt = txt.Trim();
            txt = txt.ToLower();
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string Slugify(this string phrase, string spaceChar = "-")
        {
            string str = phrase.RemoveAccent().ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", spaceChar); // //Replace spaces by dashes
            return str;
        }
        public static string ToAcceptable(this string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return strIn.Slugify();
            }
        }
        public static string Stringfy(this string phrase)
        {
            string str = phrase.RemoveAccent();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"'", "\""); // Remove all non valid chars          
            return str;
        }
        public static T CreatedBy<T>(this T entity, long created_by)
        {
            if (created_by == 0)
                return entity;
            PropertyInfo property = entity.GetType().GetProperty("created_by");
            if (property != null)
                property.SetValue(entity, created_by);
            return entity.ModifiedBy(created_by);
        }
        public static T CreatedBy<T>(this T entity, int created_by)
        {
            if (created_by == 0)
                return entity;
            PropertyInfo property = entity.GetType().GetProperty("created_by");
            if (property != null)
                property.SetValue(entity, created_by);
            return entity.ModifiedBy(created_by);
        }
        public static T ModifiedBy<T>(this T entity, long modified_by)
        {
            if (modified_by == 0)
                return entity;
            PropertyInfo property = entity.GetType().GetProperty("modified_by");
            if (property != null)
                property.SetValue(entity, modified_by);
            return entity;
        }
        public static T ModifiedBy<T>(this T entity, int modified_by)
        {
            if (modified_by == 0)
                return entity;
            PropertyInfo property = entity.GetType().GetProperty("modified_by");
            if (property != null)
                property.SetValue(entity, modified_by);
            return entity;
        }
        public static T CheckCreatedDate<T>(this T entity)
        {
            PropertyInfo property = entity.GetType().GetProperty("created_date");
            if (property != null)
                property.SetValue(entity, Zaman.Simdi);
            return entity;
        }
        public static T CheckModifiedDate<T>(this T entity)
        {
            PropertyInfo property = entity.GetType().GetProperty("modified_date");
            if (property != null)
                property.SetValue(entity, Zaman.Simdi);
            return entity;
        }
        public static T CheckIsActive<T>(this T entity)
        {
            PropertyInfo property = entity.GetType().GetProperty("is_active");
            if (property != null)
                property.SetValue(entity, true);
            return entity;
        }
        public static void WriteLog(string msg)
        {
            string folderPath = Path.Combine(System.IO.Path
                .GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + "\\Logs\\");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            using (var sw = File.AppendText(Path.Combine(folderPath, "log_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt")))
            {
                string log = $"{Zaman.Simdi.ToString("yyyy-MM-dd HH:mm:ss,FFF")}\t{msg}";
                sw.WriteLine(log);
            }
        }


        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            try
            {
                if (objectData.ToStr() == "")
                    return null;
                var serializer = new XmlSerializer(type);
                object result;

                using (TextReader reader = new StringReader(objectData))
                {
                    result = serializer.Deserialize(reader);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ObservableCollection<T> ToOC<T>(this IEnumerable<T> list)
        {
            ObservableCollection<T> OC = new ObservableCollection<T>();
            try
            {

                foreach (var item in list)
                {
                    OC.Add(item);
                }
                return OC;
            }
            catch (Exception)
            {

            }
            return OC;
        }
    }
}
