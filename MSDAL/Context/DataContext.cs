namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using BLL;

    public partial class DataContext : DbContext
    {
        public DataContext(string connectionString = null)
            : base(connectionString==null?Credentials.ConnectionString:connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
