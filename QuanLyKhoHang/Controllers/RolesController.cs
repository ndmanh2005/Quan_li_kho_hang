using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;
using QuanLyKhoHang.Models;

namespace QuanLyKhoHang.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var rolesQuery = _context.Roles
                                     .Where(p => p.IsActive != -1)
                                     .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                rolesQuery = rolesQuery.Where(p => p.Code.Contains(searchString) || p.Name.Contains(searchString));
            }

            var roles = await rolesQuery.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(roles);
        }

        // Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Name,Description,IsActive")] Role role)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _context.Roles.FirstOrDefaultAsync(p => p.Code == role.Code && p.IsActive != -1);
                if (existingRole != null)
                {
                    TempData["ErrorMessage"] = "Mã nhóm quyền đã tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                role.CreatedAt = DateTime.Now;
                role.ModifiedAt = DateTime.Now;

                _context.Add(role);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới nhóm quyền thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Thông tin không hợp lệ, vui lòng thử lại.";
            return RedirectToAction(nameof(Index));
        }

        // Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Description,IsActive")] Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var roleToUpdate = await _context.Roles.FindAsync(id);
                    if (roleToUpdate == null || roleToUpdate.IsActive == -1)
                    {
                        return NotFound();
                    }

                    roleToUpdate.Name = role.Name;
                    roleToUpdate.Description = role.Description;
                    roleToUpdate.IsActive = role.IsActive;
                    roleToUpdate.ModifiedAt = DateTime.Now;

                    _context.Update(roleToUpdate);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật nhóm quyền thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Roles.Any(e => e.Id == role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thông tin cập nhật không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }

        // Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // KIỂM TRA BẢNG USER ĐƯỢC THÊM VÀO SAU


            role.IsActive = -1;

            _context.Update(role);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa nhóm quyền thành công!";
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Roles/Authorize/5
        public async Task<IActionResult> Authorize(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            
            var currentPermissions = await _context.RolePermissions
                                                   .Where(rp => rp.RoleId == id)
                                                   .ToListAsync();
            
            
            
            ViewBag.Functions = new List<string> { "WAREHOUSE", "PROFESSION", "ROLE", "CUSTOMER", "SUPPLIER" };
            
            ViewBag.RoleName = role.Name;
            ViewBag.RoleId = role.Id;
            
            return View(currentPermissions);
        }

        //Roles/Authorize
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authorize(int roleId, List<RolePermission> permissions)
        {
            // Xóa tất cả các quyền cũ của role này
            var oldPermissions = _context.RolePermissions.Where(rp => rp.RoleId == roleId);
            _context.RolePermissions.RemoveRange(oldPermissions);

            // Thêm các quyền mới được gửi lên từ form
            if (permissions != null)
            {
                foreach (var p in permissions)
                {
                    // Chỉ thêm vào nếu có ít nhất một quyền được chọn
                    if (p.CanView || p.CanCreate || p.CanEdit || p.CanDelete || p.CanImport || p.CanExport)
                    {
                        p.RoleId = roleId;
                        _context.RolePermissions.Add(p);
                    }
                }
            }
            
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Phân quyền chức năng thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}