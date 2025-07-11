using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;
using QuanLyKhoHang.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Assign(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userEnterprises = await _context.EnterpriseUsers
                .Where(ue => ue.UserId == userId)
                
                .ToListAsync();
            

            var allEnterprises = new List<Enterprise> {
                new Enterprise { Id = 1, Name = "Công ty A - Hà Nội" },
                new Enterprise { Id = 2, Name = "Công ty B - TP.HCM" }
            };

            var viewModel = new AssignRoleViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                AvailableEnterprises = allEnterprises.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAndUserRole(int userId, int enterpriseId)
        {

            var rolesInEnterprise = await _context.Roles
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();
            
            var currentUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.EnterpriseId == enterpriseId);

            return Json(new {
                availableRoles = rolesInEnterprise,
                selectedRoleId = currentUserRole?.RoleId
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssignment([FromBody] AssignRoleViewModel model)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == model.UserId && ur.EnterpriseId == model.SelectedEnterpriseId);

            if (userRole != null)
            {
                userRole.RoleId = model.SelectedRoleId;
            }
            else
            {
                var newUserRole = new UserRole
                {
                    UserId = model.UserId,
                    EnterpriseId = model.SelectedEnterpriseId,
                    RoleId = model.SelectedRoleId,
                    IsActive = true
                };
                _context.UserRoles.Add(newUserRole);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật phân quyền thành công!" });
        }
    }
}