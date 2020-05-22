using System;
using System.Reflection;

namespace BLL
{
    public class Setup
    {
        public static bool SaveSystemCredentials(SystemCredentials systemCredentials, bool isCrypted = true)
        {
            try
            {
                string target = "SystemCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                iniFile.IniWriteValue(target, "Crypto", isCrypted ? "E" : "H");
                iniFile.IniWriteValue(target, "AppName", isCrypted ? hashCode.EncryptionConfig(systemCredentials.AppName ?? "") : systemCredentials.AppName ?? "");
                iniFile.IniWriteValue(target, "AppVersion", isCrypted ? hashCode.EncryptionConfig(systemCredentials.AppVersion ?? "") : systemCredentials.AppVersion ?? "");
                iniFile.IniWriteValue(target, "SetupDate", isCrypted ? hashCode.EncryptionConfig(systemCredentials.SetupDate ?? "") : systemCredentials.SetupDate ?? "");
                iniFile.IniWriteValue(target, "RootUrl", isCrypted ? hashCode.EncryptionConfig(systemCredentials.RootUrl ?? "") : systemCredentials.RootUrl ?? "");
                iniFile.IniWriteValue(target, "Language", isCrypted ? hashCode.EncryptionConfig(systemCredentials.Language ?? "") : systemCredentials.Language ?? "");
                iniFile.IniWriteValue(target, "Licence", isCrypted ? hashCode.EncryptionConfig(systemCredentials.Licence ?? "") : systemCredentials.Licence ?? "");
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool SaveServerCredentials(ServerCredentials serverCredentials, bool isCrypted = true)
        {
            try
            {
                string target = "ServerCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                iniFile.IniWriteValue(target, "Crypto", isCrypted ? "E" : "H");
                iniFile.IniWriteValue(target, "DataSource", isCrypted ? hashCode.EncryptionConfig(serverCredentials.DataSource ?? "") : serverCredentials.DataSource ?? "");
                iniFile.IniWriteValue(target, "UserID", isCrypted ? hashCode.EncryptionConfig(serverCredentials.UserID ?? "") : serverCredentials.UserID ?? "");
                iniFile.IniWriteValue(target, "Password", isCrypted ? hashCode.EncryptionConfig(serverCredentials.Password ?? "") : serverCredentials.Password ?? "");
                iniFile.IniWriteValue(target, "InitialCatalog", isCrypted ? hashCode.EncryptionConfig(serverCredentials.InitialCatalog ?? "") : serverCredentials.InitialCatalog ?? "");
                iniFile.IniWriteValue(target, "ConnectTimeout", isCrypted ? hashCode.EncryptionConfig(serverCredentials.ConnectTimeout.ToString()) : serverCredentials.ConnectTimeout.ToString());
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool SaveCustomerCredentials(CustomerCredentials customerCredentials, bool isCrypted = true)
        {
            try
            {
                string target = "CustomerCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                iniFile.IniWriteValue(target, "Crypto", isCrypted ? "E" : "H");
                foreach (PropertyInfo property in customerCredentials.GetType().GetProperties())
                {
                    var value = property.GetValue(customerCredentials);
                    string val = (value ?? "").ToString();
                    iniFile.IniWriteValue(target, property.Name, isCrypted ? hashCode.EncryptionConfig(val) : val);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool SaveMailCredentials(MailCredentials mailCredentials, bool isCrypted = true)
        {
            try
            {
                string target = "MailCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                iniFile.IniWriteValue(target, "Crypto", isCrypted ? "E" : "H");
                iniFile.IniWriteValue(target, "Host", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Host ?? "") : mailCredentials.Host ?? "");
                iniFile.IniWriteValue(target, "Username", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Username ?? "") : mailCredentials.Username ?? "");
                iniFile.IniWriteValue(target, "Password", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Password ?? "") : mailCredentials.Password ?? "");
                iniFile.IniWriteValue(target, "DefaultAddress", isCrypted ? hashCode.EncryptionConfig(mailCredentials.DefaultAddress ?? "") : mailCredentials.DefaultAddress ?? "");
                iniFile.IniWriteValue(target, "Port", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Port.ToString()) : mailCredentials.Port.ToString());
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
