using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty; 

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty; 

        [StringLength(100)]
        public string? Barcode { get; set; } 

        public int ProductGroupId { get; set; }
        public int UnitId { get; set; } 

        [StringLength(50)]
        public string ProductType { get; set; } = "Hàng hóa"; 

        public decimal Price { get; set; } 
        public decimal? CostPrice { get; set; } 

        public decimal? MinStock { get; set; } 

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? ImageUrl { get; set; } 

        public virtual ICollection<UnitConversion> UnitConversions { get; set; } = new List<UnitConversion>();

        public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();

        public virtual ICollection<ComboComponent> ComboComponents { get; set; } = new List<ComboComponent>();
    }
}