namespace MS.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using MS.BLL;

    public partial class DataContext : DbContext
    {
        public DataContext(string conStr = "")
            : base(conStr)
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
