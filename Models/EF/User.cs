namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public long ID { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Tên Đăng Nhập")]
        [Display(Name="Tên Đăng Nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên Đăng Nhập Dài Từ 3 - 50 Ký Tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Mật Khẩu")]
        [Display(Name = "Mật Khẩu")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật Khẩu Dài Từ 6 - 32 Ký Tự" )]
        public string Password { get; set; }

        [Display(Name = "Loại Tài Khoản")]
        [StringLength(50)]
        public string GroupID { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Tên Người Dùng")]
        [Display(Name = "Tên Người Dùng")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = " Tên Dài Từ 1 - 50 Ký Tự")]
        public string Name { get; set; }

        [Display(Name = "Địa Chỉ")]
        [StringLength(100, MinimumLength = 1, ErrorMessage ="Địa Chỉ Dài Từ 1 - 100 Ký Tự")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Nhập Email")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Điện Thoại")]
        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(250)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Trạng Thái Hoạt Động")]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual UserGroup UserGroup { get; set; }
    }
}
