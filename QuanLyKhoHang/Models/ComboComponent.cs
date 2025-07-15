using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhoHang.Models
{
    [Table("combo_components")]
    public class ComboComponent
    {
        public int Id { get; set; }
        public int ParentProductId { get; set; } 
        public int ComponentProductId { get; set; } 
        public decimal Quantity { get; set; } 

        [ForeignKey("ParentProductId")]
        public virtual Product? ParentProduct { get; set; }
    }
}