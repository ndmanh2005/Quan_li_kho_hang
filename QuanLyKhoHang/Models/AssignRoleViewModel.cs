using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyKhoHang.Models
{
    public class AssignRoleViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

         public List<SelectListItem> AvailableEnterprises { get; set; } = new List<SelectListItem>();
        
  
        public List<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();

             public int SelectedEnterpriseId { get; set; }
        
        public int SelectedRoleId { get; set; }
    }
}