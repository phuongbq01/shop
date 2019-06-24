namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string Text { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        public bool Status { get; set; }

        public int? TypeID { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(250)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        public virtual MenuType MenuType { get; set; }
    }
}