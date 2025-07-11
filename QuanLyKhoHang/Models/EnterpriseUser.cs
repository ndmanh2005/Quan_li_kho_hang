using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("enterprise_users")]
    public class EnterpriseUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EnterpriseId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        
    }
}