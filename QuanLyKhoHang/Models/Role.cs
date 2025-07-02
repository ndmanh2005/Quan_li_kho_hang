using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoHang.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }

        public int IsActive { get; set; } = 1;

        public int? CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ModifiedUser { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}