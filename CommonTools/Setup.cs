using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Setup
    {
        public static bool SaveSystemCredentials(SystemCredentials systemCredentials)
        {
            try
            {
                string target = "SystemCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                string crypto = iniFile.IniReadValue(target, "Crypto");
                bool isCrypted = crypto == "E";
                iniFile.IniWriteValue(target, "RootUrl", isCrypted ? hashCode.EncryptionConfig(systemCredentials.RootUrl) : systemCredentials.RootUrl);
                iniFile.IniWriteValue(target, "Language", isCrypted ? hashCode.EncryptionConfig(systemCredentials.Language) : systemCredentials.Language);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool SaveServerCredentials(ServerCredentials serverCredentials)
        {
            try
            {
                string target = "ServerCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                string crypto = iniFile.IniReadValue(target, "Crypto");
                bool isCrypted = crypto == "E";
                iniFile.IniWriteValue(target, "DataSource", isCrypted ? hashCode.EncryptionConfig(serverCredentials.DataSource) : serverCredentials.DataSource);
                iniFile.IniWriteValue(target, "UserID", isCrypted ? hashCode.EncryptionConfig(serverCredentials.UserID) : serverCredentials.UserID);
                iniFile.IniWriteValue(target, "Password", isCrypted ? hashCode.EncryptionConfig(serverCredentials.Password) : serverCredentials.Password);
                iniFile.IniWriteValue(target, "InitialCatalog", isCrypted ? hashCode.EncryptionConfig(serverCredentials.InitialCatalog) : serverCredentials.InitialCatalog);
                iniFile.IniWriteValue(target, "ConnectionTimeout", isCrypted ? hashCode.EncryptionConfig(serverCredentials.ConnectTimeout.ToString()) : serverCredentials.ConnectTimeout.ToString());
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool SaveCustomerCredentials(CustomerCredentials customerCredentials)
        {
            try
            {
                string target = "CustomerCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                string crypto = iniFile.IniReadValue(target, "Crypto");
                bool isCrypted = crypto == "E";
                foreach (PropertyInfo property in customerCredentials.GetType().GetProperties())
                {
                    string val = property.GetValue(customerCredentials).ToString();
                    iniFile.IniWriteValue(target, property.Name, isCrypted ? hashCode.EncryptionConfig(val) : val);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool SaveMailCredentials(MailCredentials mailCredentials)
        {
            try
            {
                string target = "ServerCredentials";
                IniFile iniFile;
                HashCode hashCode = new HashCode();
                iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
                string crypto = iniFile.IniReadValue(target, "Crypto");
                bool isCrypted = crypto == "E";
                iniFile.IniWriteValue(target, "Host", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Host) : mailCredentials.Host);
                iniFile.IniWriteValue(target, "Username", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Username) : mailCredentials.Username);
                iniFile.IniWriteValue(target, "Password", isCrypted ? hashCode.EncryptionConfig(mailCredentials.Password) : mailCredentials.Password);
                iniFile.IniWriteValue(target, "DefaultAddress", isCrypted ? hashCode.EncryptionConfig(mailCredentials.DefaultAddress) : mailCredentials.DefaultAddress);
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
