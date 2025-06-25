// File: QuanLyKhoHang/Models/warehouse.cs

namespace QuanLyKhoHang.Models
{
    // Sử dụng tên class là "Warehouse" (viết hoa) cho nhất quán
    public class Warehouse 
    {
        public int Id { get; set; }
        public string? Code { get; set; } // Mã kho
        public string Name { get; set; }
        public string? Location { get; set; } // Địa chỉ kho
        public int IsActive { get; set; } // Trạng thái
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}