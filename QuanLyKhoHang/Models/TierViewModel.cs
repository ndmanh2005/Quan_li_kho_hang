using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; 

namespace QuanLyKhoHang.Models
{
    public class TierViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hạng là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        public decimal? MinSpent { get; set; }
        public decimal? MaxSpent { get; set; }
        public int? MinOrders { get; set; }
        public int? MaxOrders { get; set; }
        public int? MinPoint { get; set; }
        public int? MaxPoint { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;
        
        public string? ExistingIconMobile { get; set; }
        public string? ExistingIconWeb { get; set; }

        public IFormFile? IconMobileFile { get; set; }
        public IFormFile? IconWebFile { get; set; }
    }
}