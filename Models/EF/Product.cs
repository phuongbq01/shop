namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public long ID { get; set; }

        [Display(Name="Tên Sản Phẩm")]
        [StringLength(250)]
        public string Name { get; set; }

        [Display(Name = "Mã Sản Phẩm")]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Hình Ảnh")]
        [StringLength(250)]
        public string Image { get; set; }

        [Display(Name = "Thêm Hình Ảnh")]
        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

        [Display(Name = "Giá Khuyến Mãi")]
        public decimal? PromotionPrice { get; set; }

        public bool? IncludeVAT { get; set; }

        [Display(Name = "Số Lượng")]
        public int? Quantity { get; set; }

        [Display(Name = "Danh Mục")]
        public long? CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        [Display(Name = "Thời Gian Bảo Hành")]
        public int? Warranty { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(250)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescription { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
