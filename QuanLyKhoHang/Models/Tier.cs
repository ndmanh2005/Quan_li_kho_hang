using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("tiers")]
    public class Tier
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hạng là bắt buộc")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public decimal? MinSpent { get; set; }
        public decimal? MaxSpent { get; set; }

        public int? MinOrders { get; set; }
        public int? MaxOrders { get; set; }

        public int? MinPoint { get; set; }
        public int? MaxPoint { get; set; }

        [StringLength(500)]
        public string? IconMobile { get; set; } 

        [StringLength(500)]
        public string? IconWeb { get; set; } 

        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public int? CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ModifiedUser { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? DeletedUser { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}