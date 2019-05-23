namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using BLL;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base(Credentials.ConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
