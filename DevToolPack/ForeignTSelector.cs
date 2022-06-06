using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack
{
    public partial class ForeignTSelector : DevExpress.XtraEditors.XtraForm
    {
        DbContext Context;
        Type ForeignType;
        public object Entity { get; set; }
        public ForeignTSelector(DbContext ctx, Type foreign_type, string parent_field)
        {
            Context = ctx;
            ForeignType = foreign_type;
            InitializeComponent();
            treeList.ParentFieldName = parent_field;
            treeList.KeyFieldName = "id";
        }
        protected override void OnLoad(EventArgs e)
        {
            LoadDataSource();
        }

        private async void LoadDataSource(object sender = null, KeyEventArgs e = null)
        {
            treeList.LoadingPanelVisible = true;
            treeList.DataSource = await Task.Run(() =>
            {
                DbSet _dbSet = Context.Set(ForeignType);
                var list = _dbSet.ToListAsync();
                if (ForeignType.GetProperty("is_active") == null)
                    list.Result.ToList();
                return list.Result.Where(x => (bool)x.GetType().GetProperty("is_active").GetValue(x)).ToList();
            });
            treeList.BestFitColumns();
            treeList.LoadingPanelVisible = false;
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            if(treeList.GetFocusedRow() != null)
            {
                Entity = treeList.GetFocusedRow();
                DialogResult = DialogResult.OK;
            }
        }
    }
}