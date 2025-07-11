using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("user_roles")]
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EnterpriseId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("EnterpriseId")]
        public virtual Enterprise? Enterprise { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}