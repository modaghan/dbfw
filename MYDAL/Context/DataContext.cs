namespace DAL
{
    using System;
    using System.Data.Entity;
    using MySql.Data.MySqlClient;
    using MySql.Data.EntityFramework;
    using BLL;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base(ConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static string Host { get; set; }
        public static string Catalog { get; set; }
        public static string User_id { get; set; }
        public static string DBPassword { get; set; }
        public static string ConnectTimeout { get; set; }
        public static string ConnectionString
        {
            get
            {
                string target = "Remote";
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
                    ConnectTimeout = hashCode.DecryptionConfig(iniFile.IniReadValue(target, "ConnectTimeout"));
                }
                else
                {
                    Host = iniFile.IniReadValue(target, "Host");
                    User_id = iniFile.IniReadValue(target, "UserId");
                    DBPassword = iniFile.IniReadValue(target, "Password");
                    Catalog = iniFile.IniReadValue(target, "Catalog");
                    ConnectTimeout = iniFile.IniReadValue(target, "ConnectTimeout");
                }
                MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
                conn_string.Server = Host;
                conn_string.Database = Catalog;
                conn_string.UserID = User_id;
                conn_string.Password = DBPassword;
                conn_string.ConnectionTimeout = Utilities.UTamsayi(ConnectTimeout);
                return conn_string.ToString();
            }
        }
    }
}
