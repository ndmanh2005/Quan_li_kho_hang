using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoHang.Models
{
    public class Profession
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã ngành nghề là bắt buộc.")]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên ngành nghề là bắt buộc.")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        // is_active: 1 = đang hoạt động, 0 = ngừng hoạt động, -1 = đã xóa
        public int IsActive { get; set; } = 1;

        public int? CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ModifiedUser { get; set; }
        public DateTime ModifiedAt { get; set; }
        
        // Các trường được dùng cho chức năng xóa mềm
        public int? DeletedUser { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}