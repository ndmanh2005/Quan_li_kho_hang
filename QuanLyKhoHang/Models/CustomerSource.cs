using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("customer_sources")]
    public class CustomerSource
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã nguồn là bắt buộc")]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên nguồn là bắt buộc")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;


        public int IsActive { get; set; } = 1;

        public int? CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ModifiedUser { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? DeletedUser { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}