using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("history_tiers")]
    public class HistoryTier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TierId { get; set; } 

        [Required]
        [StringLength(50)]
        public string ActionType { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public decimal? MinSpent { get; set; }
        public decimal? MaxSpent { get; set; }
        public int? MinOrders { get; set; }
        public int? MaxOrders { get; set; }
        public int? MinPoint { get; set; }
        public int? MaxPoint { get; set; }
        public string? IconMobile { get; set; }
        public string? IconWeb { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}