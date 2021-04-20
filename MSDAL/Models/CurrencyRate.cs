namespace MS.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CurrencyRate
    {
        public int id { get; set; }

        public int currency_id { get; set; }

        [Column(TypeName = "money")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal forex_buying { get; set; }

        [Column(TypeName = "money")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal forex_selling { get; set; }

        [Column(TypeName = "money")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal banknote_buying { get; set; }

        [Column(TypeName = "money")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal banknote_selling { get; set; }

        public DateTime created_date { get; set; }

        public bool is_active { get; set; }
    }
}
