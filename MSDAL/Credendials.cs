using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class Credentials
    {
        public static string Password { get { return "97yZ6hN2jk6r4YYh2xzHJe7n"; } }
        public static class Session
        {
            /// <summary>
            /// Hata
            /// </summary>
            public static Exception Exception { get; set; }
            /// <summary>
            /// Program Sürüm Kodu
            /// </summary>
            public static string VersionCode { get; set; }
            /// <summary>
            /// Sürüm Notları
            /// </summary>
            public static string VersionDetails { get; set; }
            /// <summary>
            /// Yönetici mi yoksa Teknisyen modunda mı çalıştığı 
            /// </summary>
            public static string RunningMode { get; set; }
            /// <summary>
            /// Login olmuş kullanıcı
            /// </summary>
            public static object CurrentUser { get; set; }
            /// <summary>
            /// Login olma zamanı
            /// </summary>
            public static DateTime LoginTime { get; set; }
            /// <summary>
            /// İşlemin Başlangıç Zamanı
            /// </summary>
            public static DateTime ProcessStartTime { get; set; }
            /// <summary>
            /// İşlemin Bitiş Zamanı
            /// </summary>
            public static DateTime ProcessEndTime { get; set; }
            /// <summary>
            /// Bilgisayarın IP'leri
            /// </summary>
            public static string[] IP { get; set; }
            /// <summary>
            /// Bilgisayar Adı
            /// </summary>
            public static string ComputerName { get; set; }
            /// <summary>
            /// Uygulamanın bulunduğu klasör
            /// </summary>
            public static string FolderPath { get; set; }
            /// <summary>
            /// Network Bilgileri
            /// </summary>
            public static IList<string> NetworkAdapter { get; set; }

        }
        private static SystemCredentials systemCredentials { get; set; }
        private static ServerCredentials serverCredentials { get; set; }
        private static CustomerCredentials customerCredentials { get; set; }
        private static MailCredentials mailCredentials { get; set; }

        public static string ToConnectionString(this ServerCredentials serverCredentials)
        {
            SqlConnectionStringBuilder conn_string = new SqlConnectionStringBuilder();
            conn_string.DataSource = serverCredentials.DataSource;
            conn_string.InitialCatalog = serverCredentials.InitialCatalog;
            conn_string.UserID = serverCredentials.UserID;
            conn_string.Password = serverCredentials.Password;
            conn_string.ConnectTimeout = serverCredentials.ConnectTimeout;
            return conn_string.ToString();
        }
        public static string ConnectionString
        {
            get
            {
                // TODO deploy ederken değiştir
                //serverCredentials = ServerCredentials();
                //SqlConnectionStringBuilder conn_string = new SqlConnectionStringBuilder();
                //conn_string.DataSource = serverCredentials.DataSource;
                //conn_string.InitialCatalog = serverCredentials.InitialCatalog;
                //conn_string.UserID = serverCredentials.UserID;
                //conn_string.Password = serverCredentials.Password;
                //conn_string.ConnectTimeout = serverCredentials.ConnectTimeout;
                //return conn_string.ToString();

                SqlConnectionStringBuilder conn_string = new SqlConnectionStringBuilder();
                conn_string.DataSource = "MODSOFT\\SQLEXPRESS";
                conn_string.InitialCatalog = "DocuMOD";
                conn_string.UserID = "sa";
                conn_string.Password = "Mu43zo93";
                conn_string.ConnectTimeout = 360;
                return conn_string.ToString();
            }
        }

        public static SystemCredentials SystemCredentials()
        {
            string section = "SystemCredentials";
            IniFile iniFile;
            iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            string crypto = iniFile.IniReadValue(section, "Crypto");
            systemCredentials = new SystemCredentials();
            systemCredentials.AppName = iniFile.IniReadValue(section, "AppName");
            systemCredentials.AppVersion = iniFile.IniReadValue(section, "AppVersion");
            systemCredentials.SetupDate = iniFile.IniReadValue(section, "SetupDate");
            systemCredentials.RootUrl = iniFile.IniReadValue(section, "RootUrl");
            systemCredentials.Language = iniFile.IniReadValue(section, "Language");
            if (crypto == "E")
            {
                HashCode hashCode = new HashCode();
                systemCredentials.AppName = hashCode.DecryptionConfig(systemCredentials.AppName);
                systemCredentials.AppVersion = hashCode.DecryptionConfig(systemCredentials.AppVersion);
                systemCredentials.SetupDate = hashCode.DecryptionConfig(systemCredentials.SetupDate);
                systemCredentials.RootUrl = hashCode.DecryptionConfig(systemCredentials.RootUrl);
                systemCredentials.Language = hashCode.DecryptionConfig(systemCredentials.Language);
            }
            return systemCredentials;
        }

        public static ServerCredentials ServerCredentials()
        {
            if (serverCredentials == null || serverCredentials.DataSource == null || serverCredentials.DataSource == "")
            {
                string section = "ServerCredentials";
                IniFile iniFile;
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                string crypto = iniFile.IniReadValue(section, "Crypto");
                serverCredentials = new ServerCredentials();
                serverCredentials.DataSource = iniFile.IniReadValue(section, "DataSource");
                serverCredentials.UserID = iniFile.IniReadValue(section, "UserID");
                serverCredentials.Password = iniFile.IniReadValue(section, "Password");
                serverCredentials.InitialCatalog = iniFile.IniReadValue(section, "InitialCatalog");
                serverCredentials.ConnectTimeout = iniFile.IniReadValue(section, "ConnectTimeout").ToInteger();
                if (crypto == "E")
                {
                    HashCode hashCode = new HashCode();
                    serverCredentials.DataSource = hashCode.DecryptionConfig(serverCredentials.DataSource);
                    serverCredentials.UserID = hashCode.DecryptionConfig(serverCredentials.UserID);
                    serverCredentials.Password = hashCode.DecryptionConfig(serverCredentials.Password);
                    serverCredentials.InitialCatalog = hashCode.DecryptionConfig(serverCredentials.InitialCatalog);
                    serverCredentials.ConnectTimeout = (hashCode.DecryptionConfig(iniFile.IniReadValue(section, "ConnectTimeout"))).ToInteger();
                }
            }
            return serverCredentials;
        }
        public static CustomerCredentials CustomerCredentials()
        {
            IniFile iniFile;
            iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            string section = "CustomerCredentials";
            string crypto = iniFile.IniReadValue(section, "Crypto");
            customerCredentials = new CustomerCredentials()
            {
                Logo = iniFile.IniReadValue(section, "Logo"),
                FullName = iniFile.IniReadValue(section, "FullName"),
                ShortName = iniFile.IniReadValue(section, "ShortName"),
                Address = iniFile.IniReadValue(section, "Address"),
                Region = iniFile.IniReadValue(section, "Region"),
                Province = iniFile.IniReadValue(section, "Province"),
                Country = iniFile.IniReadValue(section, "Country"),
                Phone = iniFile.IniReadValue(section, "Phone"),
                Mersis = iniFile.IniReadValue(section, "Mersis"),
                TaxNo = iniFile.IniReadValue(section, "TaxNo"),
                TaxRegion = iniFile.IniReadValue(section, "TaxRegion"),
                Mail = iniFile.IniReadValue(section, "Mail"),
                Web = iniFile.IniReadValue(section, "Web")
            };
            if (crypto == "E")
            {
                HashCode hashCode = new HashCode();
                customerCredentials.Logo = hashCode.DecryptionConfig(customerCredentials.Logo);
                customerCredentials.FullName = hashCode.DecryptionConfig(customerCredentials.FullName);
                customerCredentials.ShortName = hashCode.DecryptionConfig(customerCredentials.ShortName);
                customerCredentials.Address = hashCode.DecryptionConfig(customerCredentials.Address);
                customerCredentials.Region = hashCode.DecryptionConfig(customerCredentials.Region);
                customerCredentials.Province = hashCode.DecryptionConfig(customerCredentials.Province);
                customerCredentials.Country = hashCode.DecryptionConfig(customerCredentials.Country);
                customerCredentials.Phone = hashCode.DecryptionConfig(customerCredentials.Phone);
                customerCredentials.Mersis = hashCode.DecryptionConfig(customerCredentials.Mersis);
                customerCredentials.TaxNo = hashCode.DecryptionConfig(customerCredentials.TaxNo);
                customerCredentials.TaxRegion = hashCode.DecryptionConfig(customerCredentials.TaxRegion);
                customerCredentials.Mail = hashCode.DecryptionConfig(customerCredentials.Mail);
                customerCredentials.Web = hashCode.DecryptionConfig(customerCredentials.Web);
            }
            return customerCredentials;
        }
        public static MailCredentials MailCredentials()
        {
            IniFile iniFile;
            iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            string section = "MailCredentials";
            string crypto = iniFile.IniReadValue(section, "Crypto");
            mailCredentials = new MailCredentials()
            {
                Host = iniFile.IniReadValue(section, "Host"),
                Username = iniFile.IniReadValue(section, "Username"),
                Password = iniFile.IniReadValue(section, "Password"),
                Port = (iniFile.IniReadValue(section, "Port")).ToInteger(),
                DefaultAddress = iniFile.IniReadValue(section, "DefaultAddress")
            };
            if (crypto == "E")
            {
                HashCode hashCode = new HashCode();
                mailCredentials.Host = hashCode.DecryptionConfig(mailCredentials.Host);
                mailCredentials.Username = hashCode.DecryptionConfig(mailCredentials.Username);
                mailCredentials.Password = hashCode.DecryptionConfig(mailCredentials.Password);
                mailCredentials.Port = (hashCode.DecryptionConfig(iniFile.IniReadValue(section, "Port"))).ToInteger();
                mailCredentials.DefaultAddress = hashCode.DecryptionConfig(mailCredentials.DefaultAddress);
            }
            return mailCredentials;
        }
        public static string MailSignature
        {
            get
            {
                return "<div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><img style=\"height:54px;width:215px;\" src=\"" + CustomerCredentials().Logo + "\"></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><span style=\"color:#1f497d;\"><strong style=\"color:#1155cc;\"><span style=\"color:#1b6698;font-family:tahoma,sans-serif;font-size:9pt;\">" + CustomerCredentials().FullName + "</span></strong></span></div><div style=\"color:#222222;font-family:arial,sans-serif;font-size:12.8px;margin:0cm 0cm 0.0001pt;\"><a href=\"" + CustomerCredentials().Web + "\" data-vdir-href=\"https://mail.yandex.ru/re.jsx?uid=1130000027065823&amp;c=LIZA&amp;cv=14.5.5&amp;mid=165507286305865806&amp;h=a,C0VdLKa4E2vCFPLXVbkvEA&amp;l=aHR0cDovL3d3dy5tb2Rzb2Z0LmNvbS50ci8\" data-orig-href=\"" + CustomerCredentials().Web + "\" class=\"daria-goto-anchor\" target=\"_blank\" rel=\"noopener noreferrer\"><strong><font color=\"#1b6698\" face=\"Tahoma, sans-serif\"><span style=\"font-size:12px;\">" + CustomerCredentials().Web + "</span></font></strong></a></div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">" + CustomerCredentials().Address + "</span></font></div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">" + CustomerCredentials().Region + "/" + CustomerCredentials().Province + "</span></font></div><div style=\"margin:0cm 0cm 0.0001pt;\">&nbsp;</div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">Tlf : <span class=\"wmi-callto\">" + CustomerCredentials().Phone + "</span></span></font></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><a href=\"mailto:" + CustomerCredentials().Mail + "\" class=\"ns-action\" data-click-action=\"common.go\" data-params=\"new_window&amp;url=%23compose%3Fmailto%3D" + CustomerCredentials().Mail + "\"><span style=\"color:#333333;font-family:tahoma,sans-serif;font-size:9pt;\">" + CustomerCredentials().Mail + "</span></a></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\">&nbsp;</div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><span style=\"color:#333333;font-family:tahoma,sans-serif;font-size:9pt;\">&nbsp;</span></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">Bu mesajda, yalnizca muhatabini ilgilendiren, kisiye veya kuruma ozel bilgiler yer aliyor olabilir. Mesajin muhatabi degilseniz, icerigini ve varsa ekindeki dosyalari kimseye aktarmayiniz ya da kopyalamayiniz. Boyle bir durumda lutfen gondereni uyarip, mesaji imha ediniz. Mail listemizden çıkmak için mail atmanızı rica ederiz.</font></span></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><strong><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">Gostermis oldugunuz hassasiyetten oturu tesekkür ederiz.</font></span></strong></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\">&nbsp;</div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">This e-mail may contain confidential and/or privileged information. If you are not the intended recipient (or have received this e-mail in error) please notify the sender immediately and destroy this e-mail. Any unauthorised copying, disclosure or distribution of the material in this e-mail is strictly forbidden.</font></span></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><strong><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">Thank you for your co-operation.</font></span></strong></div><div>&nbsp;</div>";
            }
        }
    }
}
