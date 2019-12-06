using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public static class Utilities
    {
        public static bool IsServerConnected
        {
            get
            {
                using (var l_oConnection = new SqlConnection(Credentials.ConnectionString))
                {
                    try
                    {
                        l_oConnection.Open();
                        l_oConnection.Close();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }
        public static string ToJson(this object entity)
        {
            try
            {
                var v = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return v;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static Expression<Func<TItem, bool>> PropertyEquals<TItem>(PropertyInfo property, object value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, property),
                Expression.Constant(value));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
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
        public static double ToDouble(this object val)
        {
            try
            {
                return Convert.ToDouble(val);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int ToInteger(this object sayi)
        {
            try
            {
                int i = 0;
                int.TryParse(sayi.ToString(), out i);
                return i;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static long ToLong(this string longStr)
        {
            try
            {
                return Convert.ToInt64(longStr);
            }
            catch (Exception)
            {
                return 0;
            }
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
                typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return property.Name;
            return (atts[0] as DisplayNameAttribute).DisplayName;
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

        static int colorIndex = 0;
        public static Color getRandomColor()
        {
            try
            {
                KnownColor[] names = new KnownColor[] { KnownColor.DarkBlue, KnownColor.CadetBlue, KnownColor.BlueViolet, KnownColor.DeepSkyBlue, KnownColor.DodgerBlue, KnownColor.AliceBlue, KnownColor.CornflowerBlue, KnownColor.MediumPurple, KnownColor.Purple, KnownColor.Violet, KnownColor.DarkGray, KnownColor.DarkTurquoise, KnownColor.Turquoise };
                KnownColor randomColorName = names[colorIndex];
                colorIndex = colorIndex == names.Length - 1 ? 0 : ++colorIndex;
                return Color.FromKnownColor(randomColorName);
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
        public static T CreatedBy<T>(this T entity, long created_by)
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
    }
}
