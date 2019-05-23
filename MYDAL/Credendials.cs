using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
            public static string ProgramVersionCode { get; set; }
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
        public static string Host { get; set; }
        public static string Catalog { get; set; }
        public static string User_id { get; set; }
        public static string DBPassword { get; set; }
        public static string ConnectionTimeout { get; set; }

        public static string ConnectionString(string target = "Remote")
        {
            IniFile iniFile;
            HashCode hashCode = new HashCode();
            iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            string crypto = iniFile.IniReadValue(target, "Crypto");
            if (crypto == "E")
            {
                Host = hashCode.DecryptionConfig(iniFile.IniReadValue(target, "Host"));
                User_id = hashCode.DecryptionConfig(iniFile.IniReadValue(target, "UserId"));
                DBPassword = hashCode.DecryptionConfig(iniFile.IniReadValue(target, "Password"));
                Catalog = hashCode.DecryptionConfig(iniFile.IniReadValue(target, "Catalog"));
                ConnectionTimeout = hashCode.DecryptionConfig(iniFile.IniReadValue(target, "ConnectionTimeout"));
            }
            else
            {
                Host = iniFile.IniReadValue(target, "Host");
                User_id = iniFile.IniReadValue(target, "UserId");
                DBPassword = iniFile.IniReadValue(target, "Password");
                Catalog = iniFile.IniReadValue(target, "Catalog");
                ConnectionTimeout = iniFile.IniReadValue(target, "ConnectionTimeout");
            }
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = Host;
            conn_string.Database = Catalog;
            conn_string.UserID = User_id;
            conn_string.Password = DBPassword;
            return conn_string.ToString();
        }

        public static MailCredentials MailCredentials
        {
            get
            {
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                string crypto = iniFile.IniReadValue("Mail", "Crypto");
                if (crypto == "E")
                {
                    return new MailCredentials()
                    {
                        Host = hashCode.DecryptionConfig(iniFile.IniReadValue("Mail", "Host")),
                        Username = hashCode.DecryptionConfig(iniFile.IniReadValue("Mail", "Username")),
                        Password = hashCode.DecryptionConfig(iniFile.IniReadValue("Mail", "Password")),
                        Port = Convert.ToInt32(hashCode.DecryptionConfig(iniFile.IniReadValue("Mail", "Port"))),
                        DefaultAddress = hashCode.DecryptionConfig(iniFile.IniReadValue("Mail", "DefaultAddress"))
                    };
                }
                else
                {
                    return new MailCredentials()
                    {
                        Host = iniFile.IniReadValue("Mail", "Host"),
                        Username = iniFile.IniReadValue("Mail", "Username"),
                        Password = iniFile.IniReadValue("Mail", "Password"),
                        Port = Convert.ToInt32(iniFile.IniReadValue("Mail", "Port")),
                        DefaultAddress = iniFile.IniReadValue("Mail", "DefaultAddress")
                    };
                }
            }
        }
        public static readonly string MailSignature = "<div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><img style=\"height:54px;width:215px;\" src=\"https://resize.yandex.net/mailservice?url=http%3A%2F%2Fwww.modsoft.com.tr%2Fwp-content%2Fuploads%2F2018%2F03%2Fmodsoft-logo-with-slogan-sm.png&amp;proxy=yes&amp;key=6b690bbba0f9b3a26a64341555b1a0c3\"></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><span style=\"color:#1f497d;\"><strong style=\"color:#1155cc;\"><span style=\"color:#1b6698;font-family:tahoma,sans-serif;font-size:9pt;\">MODSOFT Bilişim Teknolojileri ve Arge San.Tic.Ltd.Şti.</span></strong></span></div><div style=\"color:#222222;font-family:arial,sans-serif;font-size:12.8px;margin:0cm 0cm 0.0001pt;\"><a href=\"http://www.modsoft.com.tr/\" data-vdir-href=\"https://mail.yandex.ru/re.jsx?uid=1130000027065823&amp;c=LIZA&amp;cv=14.5.5&amp;mid=165507286305865806&amp;h=a,C0VdLKa4E2vCFPLXVbkvEA&amp;l=aHR0cDovL3d3dy5tb2Rzb2Z0LmNvbS50ci8\" data-orig-href=\"http://www.modsoft.com.tr/\" class=\"daria-goto-anchor\" target=\"_blank\" rel=\"noopener noreferrer\"><strong><font color=\"#1b6698\" face=\"Tahoma, sans-serif\"><span style=\"font-size:12px;\">http://www.modsoft.com.tr</span></font></strong></a></div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">Hisariçi Mah. Turan Cad.</span></font></div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">Hasanbaba Çarşısı Kat:2 No:235</span></font></div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">10010 Karesi / Balıkesir</span></font></div><div style=\"margin:0cm 0cm 0.0001pt;\">&nbsp;</div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">Tlf : <span class=\"wmi-callto\">+90 266 241 9587</span></span></font></div><div style=\"margin:0cm 0cm 0.0001pt;\"><font color=\"#333333\" face=\"tahoma, sans-serif\"><span style=\"font-size:12px;\">Fax : <span class=\"wmi-callto\">+90 266 241 9587</span> </span></font></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><span style=\"color:#333333;font-family:tahoma,sans-serif;font-size:9pt;\">Gsm : <span class=\"wmi-callto\">+90 546 913 3401</span></span></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><a href=\"mailto:muzafferdaghan@modsoft.com.tr\" class=\"ns-action\" data-click-action=\"common.go\" data-params=\"new_window&amp;url=%23compose%3Fmailto%3Dmuzafferdaghan%2540modsoft.com.tr\"><span style=\"color:#333333;font-family:tahoma,sans-serif;font-size:9pt;\">muzafferdaghan@modsoft.com.tr</span></a></div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\">&nbsp;</div><div style=\"color:#222222;margin:0cm 0cm 0.0001pt;font-size:11pt;font-family:Calibri,sans-serif;\"><span style=\"color:#333333;font-family:tahoma,sans-serif;font-size:9pt;\">&nbsp;</span></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">Bu mesajda, yalnizca muhatabini ilgilendiren, kisiye veya kuruma ozel bilgiler yer aliyor olabilir. Mesajin muhatabi degilseniz, icerigini ve varsa ekindeki dosyalari kimseye aktarmayiniz ya da kopyalamayiniz. Boyle bir durumda lutfen gondereni uyarip, mesaji imha ediniz. Mail listemizden çıkmak için mail atmanızı rica ederiz.</font></span></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><strong><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">Gostermis oldugunuz hassasiyetten oturu tesekkür ederiz.</font></span></strong></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\">&nbsp;</div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">This e-mail may contain confidential and/or privileged information. If you are not the intended recipient (or have received this e-mail in error) please notify the sender immediately and destroy this e-mail. Any unauthorised copying, disclosure or distribution of the material in this e-mail is strictly forbidden.</font></span></div><div style=\"color:#222222;font-size:12.8px;margin:0cm 0cm 0.0001pt;font-family:Calibri,sans-serif;\"><strong><span style=\"font-family:arial,sans-serif;\"><font size=\"1\">Thank you for your co-operation.</font></span></strong></div><div>&nbsp;</div>";
    }
}
