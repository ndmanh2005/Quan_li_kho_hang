using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("unit_conversions")]
    public class UnitConversion
    {
        public int Id { get; set; }
        public int ProductId { get; set; } 
        public int UnitId { get; set; } 
        public decimal ConversionFactor { get; set; } 
        public string? Description { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}