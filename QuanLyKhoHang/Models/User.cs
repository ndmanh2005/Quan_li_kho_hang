using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? LockedEnd { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}