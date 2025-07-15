using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuanLyKhoHang.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = new Product();

        public List<UnitConversion> UnitConversions { get; set; } = new List<UnitConversion>();
        
        public List<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();

        public List<ComboComponent> ComboComponents { get; set; } = new List<ComboComponent>();

        public IFormFile? ImageFile { get; set; }

        public List<SelectListItem> ProductGroups { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Units { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Hàng hóa", Text = "Hàng hóa" },
            new SelectListItem { Value = "Dịch vụ", Text = "Dịch vụ" },
            new SelectListItem { Value = "Combo", Text = "Combo - Đóng gói" }
        };
    }
}