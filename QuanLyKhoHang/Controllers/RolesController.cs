using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data; 
using QuanLyKhoHang.Models;
using QuanLyKhoHang.Helpers; 
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        private void PopulateViewData()
        {
            ViewData["MenuItems"] = PermissionHelper.AllMenuItems;
            ViewData["Actions"] = PermissionHelper.AllActions;
        }

        public IActionResult Create()
        {
            PopulateViewData(); 
            var viewModel = new RoleManagementViewModel();
            return View(viewModel);
        }

        //Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new Role { Name = model.RoleName, Description = model.Description };
                _context.Add(role);
                await _context.SaveChangesAsync();

                if (model.SelectedPermissions != null)
                {
                    foreach (var permissionValue in model.SelectedPermissions)
                    {
                        var rolePermission = new RolePermission { RoleId = role.Id, PermissionValue = permissionValue };
                        _context.RolePermissions.Add(rolePermission);
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateViewData();
            return View(model);
        }

        //Roles/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            var currentPermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Select(rp => rp.PermissionValue)
                .ToListAsync();

            var viewModel = new RoleManagementViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                SelectedPermissions = currentPermissions
            };

            PopulateViewData();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoleManagementViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var roleToUpdate = await _context.Roles.FindAsync(id);
                if (roleToUpdate == null) return NotFound();

                roleToUpdate.Name = model.RoleName;
                roleToUpdate.Description = model.Description;
                _context.Update(roleToUpdate);

                var oldPermissions = _context.RolePermissions.Where(rp => rp.RoleId == id);
                _context.RolePermissions.RemoveRange(oldPermissions);

                if (model.SelectedPermissions != null)
                {
                    foreach (var permissionValue in model.SelectedPermissions)
                    {
                        _context.RolePermissions.Add(new RolePermission { RoleId = id, PermissionValue = permissionValue });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            PopulateViewData();
            return View(model);
        }
        
    }
}