using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhoHang.Models
{
    public class RoleManagementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên vai trò.")]
        [Display(Name = "Tên vai trò")]
        public string RoleName { get; set; } = string.Empty; 

        [Display(Name = "Mô tả")]
        public string? Description { get; set; } 

        public List<string> SelectedPermissions { get; set; }

        public RoleManagementViewModel()
        {
            SelectedPermissions = new List<string>();
        }
    }
}