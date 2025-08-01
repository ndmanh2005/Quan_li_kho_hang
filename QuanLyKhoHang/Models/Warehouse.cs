

namespace QuanLyKhoHang.Models
{

    public class Warehouse 
    {
        public int Id { get; set; }
        public string? Code { get; set; } 
        public string Name { get; set; }
        public string? Location { get; set; } 
        public int IsActive { get; set; } 
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}