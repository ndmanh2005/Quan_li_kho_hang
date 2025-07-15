using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("product_attribute_values")]
    public class ProductAttributeValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; } 
        public string AttributeName { get; set; } = string.Empty; 
        public string AttributeValue { get; set; } = string.Empty;

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}