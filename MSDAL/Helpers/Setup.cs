using IniParser;
using IniParser.Model;
using System;
using System.IO;
using System.Reflection;

namespace MS.BLL
{
    public class Setup
    {
        public static string ConfigFile
        {
            get
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
                if (!File.Exists(file))
                    File.Create(file);
                return file;
            }
        }
        private static FileIniDataParser parser = new FileIniDataParser();
        private static IniData data = parser.ReadFile(ConfigFile);
        public static void Complete()
        {
            parser.WriteFile(ConfigFile, data);
        }
        public static bool SaveSystemCredentials(SystemCredentials systemCredentials, bool isCrypted = true)
        {
            try
            {
                string target = "SystemCredentials";
                HashCode hashCode = new HashCode();               
                data[target]["Crypto"] = isCrypted ? "E" : "H";
                data[target]["AppName"]= isCrypted ? hashCode.EncryptionConfig(systemCredentials.AppName ?? "") : systemCredentials.AppName ?? "";
                data[target]["AppVersion"]= isCrypted ? hashCode.EncryptionConfig(systemCredentials.AppVersion ?? "") : systemCredentials.AppVersion ?? "";
                data[target]["SetupDate"]= isCrypted ? hashCode.EncryptionConfig(systemCredentials.SetupDate ?? "") : systemCredentials.SetupDate ?? "";
                data[target]["RootUrl"]= isCrypted ? hashCode.EncryptionConfig(systemCredentials.RootUrl ?? "") : systemCredentials.RootUrl ?? "";
                data[target]["Language"]= isCrypted ? hashCode.EncryptionConfig(systemCredentials.Language ?? "") : systemCredentials.Language ?? "";
                data[target]["Licence"]= isCrypted ? hashCode.EncryptionConfig(systemCredentials.Licence ?? "") : systemCredentials.Licence ?? "";
                
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
                Credentials.serverCredentials = null;
                string target = "ServerCredentials";
                HashCode hashCode = new HashCode();                
                data[target]["Crypto"]= isCrypted ? "E" : "H";
                data[target]["DataSource"]= isCrypted ? hashCode.EncryptionConfig(serverCredentials.DataSource ?? "") : serverCredentials.DataSource ?? "";
                data[target]["UserID"]= isCrypted ? hashCode.EncryptionConfig(serverCredentials.UserID ?? "") : serverCredentials.UserID ?? "";
                data[target]["Password"]= isCrypted ? hashCode.EncryptionConfig(serverCredentials.Password ?? "") : serverCredentials.Password ?? "";
                data[target]["InitialCatalog"]= isCrypted ? hashCode.EncryptionConfig(serverCredentials.InitialCatalog ?? "") : serverCredentials.InitialCatalog ?? "";
                data[target]["ConnectTimeout"]= isCrypted ? hashCode.EncryptionConfig(serverCredentials.ConnectTimeout.ToString()) : serverCredentials.ConnectTimeout.ToString();
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
                HashCode hashCode = new HashCode();
                data[target]["Crypto"]= isCrypted ? "E" : "H";
                foreach (PropertyInfo property in customerCredentials.GetType().GetProperties())
                {
                    var value = property.GetValue(customerCredentials);
                    string val = (value ?? "").ToString();
                    data[target][property.Name]= isCrypted ? hashCode.EncryptionConfig(val) : val;
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
                HashCode hashCode = new HashCode();
                data[target]["Crypto"] = isCrypted ? "E" : "H";
                data[target]["Host"] = isCrypted ? hashCode.EncryptionConfig(mailCredentials.Host ?? "") : mailCredentials.Host ?? "";
                data[target]["Username"] = isCrypted ? hashCode.EncryptionConfig(mailCredentials.Username ?? "") : mailCredentials.Username ?? "";
                data[target]["Password"] = isCrypted ? hashCode.EncryptionConfig(mailCredentials.Password ?? "") : mailCredentials.Password ?? "";
                data[target]["DefaultAddress"] = isCrypted ? hashCode.EncryptionConfig(mailCredentials.DefaultAddress ?? "") : mailCredentials.DefaultAddress ?? "";
                data[target]["Port"] = isCrypted ? hashCode.EncryptionConfig(mailCredentials.Port.ToString()) : mailCredentials.Port.ToString();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
