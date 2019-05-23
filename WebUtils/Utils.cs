using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using BLL;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using SalonApp.Web.Models;
using SalonApp.DATA;
using SalonApp.BLL;
using System.Net;
using System.Text;
using System.Web.Helpers;
using ImageResizer;

namespace WebUtils
{
    public static class Utils
    {
        public static class Cookies
        {
            private static readonly string cookie_header = "salonapp_";
            public static void Save(Controller Controller, string key, object entity)
            {
                HttpCookie cookie = Controller.Request.Cookies[cookie_header+key];
                if (cookie != null)
                    cookie.Value = JsonConvert.SerializeObject(entity);
                else
                {
                    cookie = new HttpCookie(cookie_header+key);
                    cookie.Value = JsonConvert.SerializeObject(entity);
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Controller.Response.Cookies.Add(cookie);
            }
            public static void Save(Controller Controller, string key, string value)
            {
                HttpCookie cookie = Controller.Request.Cookies[cookie_header + key];
                if (cookie != null)
                    cookie.Value = value;
                else
                {
                    cookie = new HttpCookie(cookie_header + key);
                    cookie.Value = value;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Controller.Response.Cookies.Add(cookie);
            }
            public static void Save(HttpContextBase Context, string key, string value)
            {
                HttpCookie cookie = Context.Request.Cookies[cookie_header + key];
                if (cookie != null)
                    cookie.Value = value;
                else
                {
                    cookie = new HttpCookie(cookie_header + key);
                    cookie.Value = value;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Context.Response.Cookies.Add(cookie);
            }
            public static T Get<T>(Controller Controller, string key)
            {
                HttpCookie cookie = Controller.Request.Cookies[cookie_header + key];
                if (cookie != null)
                    return JsonConvert.DeserializeObject<T>(cookie.Value);
                else
                {
                    return default(T);
                }
            }
            public static T Get<T>(HttpContextBase Context, string key)
            {
                HttpCookie cookie = Context.Request.Cookies[cookie_header + key];
                if (cookie != null)
                    return JsonConvert.DeserializeObject<T>(cookie.Value);
                else
                {
                    return default(T);
                }
            }
            public static List<KeyValuePair<string, string>> GetAll(HttpRequestBase Request, string startsWith)
            {
                var keys = Request.Cookies.AllKeys.Where(k => k.StartsWith(startsWith));
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                foreach (var key in keys)
                {
                    list.Add(new KeyValuePair<string, string>(key, Request.Cookies[key].Value));
                }
                return list;
            }
            public static List<KeyValuePair<string, string>> GetAll(HttpContextBase Context, string startsWith)
            {
                var keys = Context.Request.Cookies.AllKeys.Where(k => k.StartsWith(startsWith));
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                foreach (var key in keys)
                {
                    list.Add(new KeyValuePair<string, string>(key, Context.Request.Cookies[key].Value));
                }
                return list;
            }
            public static void Delete(Controller Controller, string key)
            {
                try
                {
                    HttpCookie cookie = Controller.Request.Cookies[cookie_header + key];
                    if (cookie != null)
                    {
                        Controller.Response.Cookies[cookie_header + key].Expires = DateTime.Now.AddDays(-1);
                    }
                }
                catch (Exception)
                {

                }
            }
            public static void Delete(HttpContextBase Context, string key)
            {
                try
                {
                    HttpCookie cookie = Context.Request.Cookies[cookie_header + key];
                    if (cookie != null)
                    {
                        Context.Response.Cookies[cookie_header + key].Expires = DateTime.Now.AddDays(-1);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public static string MakeActiveClass(this UrlHelper urlHelper, string controller, string action = null)
        {
            string result = null;

            string controllerName = urlHelper.RequestContext.RouteData.Values["controller"].ToString();
            string actionName = urlHelper.RequestContext.RouteData.Values["action"].ToString();
            if (action != null && controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase) && actionName.Equals(action, StringComparison.OrdinalIgnoreCase))
                result = "active";
            else if (controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
                result = "active";

            return result;
        }
        public static byte[] ToByteArray(this HttpPostedFileBase file)
        {
            byte[] imgData;
            using (var reader = new BinaryReader(file.InputStream))
            {
                imgData = reader.ReadBytes(file.ContentLength);
            }
            return imgData;
        }
        public static string ToEnglish(this string phrase)
        {
            phrase = phrase.Replace("ı", "i");
            phrase = phrase.Replace("ö", "o");
            phrase = phrase.Replace("ü", "u");
            phrase = phrase.Replace("ç", "c");
            phrase = phrase.Replace("ğ", "g");
            phrase = phrase.Replace("ş", "s");
            phrase = phrase.Replace("İ", "I");
            phrase = phrase.Replace("Ö", "O");
            phrase = phrase.Replace("Ü", "U");
            phrase = phrase.Replace("Ç", "C");
            phrase = phrase.Replace("Ğ", "G");
            phrase = phrase.Replace("Ş", "S");
            return phrase;
        }
        public static string FormatPhone(string phone)
        {
            string result = phone.Replace(" ", "").Trim();
            if (result.StartsWith("5"))
                result = "+90" + result;
            if (result.StartsWith("0"))
                result = "+9" + result;
            if (result.StartsWith("9"))
                result = "+" + result;
            return result;
        }
        public static string Theme(string defaultTheme = "MaterialCompact")
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Theme"];
            if (cookie != null)
                return cookie.Value;
            else
            {
                return defaultTheme;
            }
        }

        public static string GetDisplayName<T>()
        {
            var displayName = typeof(T).GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return "";
        }
        public static string GetDisplayName(this Type type)
        {
            var displayName = type.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return "";
        }
        public static string GetDisplayName(this PropertyInfo property)
        {
            var displayName = property.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return property.Name;
        }
        public static bool IsRequired(this PropertyInfo property)
        {
            object[] attrs = property.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                RequiredAttribute reqAttr = attr as RequiredAttribute;
                if (reqAttr != null)
                    return true;
            }
            return false;
        }
        public static void Equalize(this object target, object source)
        {
            foreach (PropertyInfo prop in target.GetType().GetProperties().Where(p => !p.GetMethod.IsVirtual && p.Name != "created_date" && p.Name != "modified_date" && p.Name != "is_active" && p.Name != "details"))
            {
                var value = source.GetType().GetProperty(prop.Name).GetValue(source);
                target.GetType().GetProperty(prop.Name).SetValue(target, value);
            }
        }

        public static bool ValidateActionParameters(params int[] parameters)
        {
            foreach (int parameter in parameters)
            {
                if (parameter <= 0)
                    return false;
            }
            return true;
        }
        public static string CreateVisitorLog(HttpRequestBase request)
        {
            VisitorLog visitorLog = new VisitorLog();
            visitorLog.http_user_agent = request.ServerVariables["HTTP_USER_AGENT"];
            visitorLog.remote_addr = request.ServerVariables["REMOTE_ADDR"];
            visitorLog.remote_host = request.ServerVariables["REMOTE_HOST"];
            visitorLog.request_method = request.ServerVariables["REQUEST_METHOD"];
            visitorLog.server_name = request.ServerVariables["SERVER_NAME"];
            visitorLog.server_port = request.ServerVariables["SERVER_PORT"];
            visitorLog.server_software = request.ServerVariables["SERVER_SOFTWARE"];
            visitorLog.page_url = request.ServerVariables["URL"];
            visitorLog.controller = request.RequestContext.RouteData.Values["controller"].ToString();
            visitorLog.action = request.RequestContext.RouteData.Values["action"].ToString();
            visitorLog.DateCreated = DateTime.Now;
            return JsonConvert.SerializeObject(visitorLog);
        }
        
        private static string ReviewName(string filename)
        {
            filename.Trim();
            filename = filename.ToLower();
            filename = filename.Replace(" ", "-");
            filename = filename.Replace("/", "-");
            filename = filename.Replace("ç", "c");
            filename = filename.Replace("ğ", "g");
            filename = filename.Replace("ı", "i");
            filename = filename.Replace("ö", "o");
            filename = filename.Replace("ş", "s");
            filename = filename.Replace("ü", "u");

            return filename;
        }
        public static string SaveFile(FileModel model)
        {
            try
            {
                string fname = Zaman.Simdi.Ticks + "-" + model.filename.RemoveAccent();
                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + model.folder), fname);
                Byte[] fileBytes = Convert.FromBase64String(model.file);
                if (model.folder.Contains("Images"))
                    CreateSmalls(model, fname);
                else
                    System.IO.File.WriteAllBytes(path, fileBytes);
                string res = "Files/" + model.folder + fname;
                res = res.Replace("//", "/");
                return res;
            }
            catch (Exception ex)
            {
                Logger.Save("FileUpload Error","",ex);
                return "";
            }
        }

        private static void CreateSmalls(FileModel model, string fname)
        {
            Byte[] fileBytes = Convert.FromBase64String(model.file);
            WebImage[] result = new WebImage[3];
            WebImage img = new WebImage(fileBytes);

            var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + model.folder), fname);
            img.Save(path);



            // Thumbnail
            ImageBuilder.Current.Build(path, System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + model.folder) + "/th-" + fname, new ResizeSettings { Width = 60, Height = 60 });
            // Small
            ImageBuilder.Current.Build(path, System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + model.folder) + "/sm-" + fname, new ResizeSettings { Width = 128, Height = 128 });
            // Medium
            ImageBuilder.Current.Build(path, System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + model.folder) + "/md-" + fname, new ResizeSettings { Width = 300, Height = 300 });
            // Large
            ImageBuilder.Current.Build(path, System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + model.folder) + "/lg-" + fname, new ResizeSettings { Width = 600, Height = 600 });


        }
    }
}