using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;
using QuanLyKhoHang.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? searchTerm)
        {
            var query = _context.Users.Where(u => u.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FullName.Contains(searchTerm) ||
                                         u.Email.Contains(searchTerm) ||
                                         (u.PhoneNumber != null && u.PhoneNumber.Contains(searchTerm)));
            }

            var users = await query.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.PhoneNumber,
                Locked = u.LockedEnd != null && u.LockedEnd > DateTime.UtcNow, 
            }).ToListAsync();

            return Json(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng." });
            }

            bool isCurrentlyLocked = user.LockedEnd != null && user.LockedEnd > DateTime.UtcNow;

            if (isCurrentlyLocked)
            {
                user.LockedEnd = null; 
            }
            else
            {

                var activeEnterpriseCount = await _context.EnterpriseUsers.CountAsync(eu => eu.UserId == id);

                if (activeEnterpriseCount >= 2)
                {
                    return Json(new { success = false, message = "Người dùng thuộc nhiều đơn vị khác nhau, không được phép khóa." });
                }

                user.LockedEnd = DateTime.MaxValue;
            }

            await _context.SaveChangesAsync();

            var newLockedStatus = user.LockedEnd != null && user.LockedEnd > DateTime.UtcNow;
            return Json(new { success = true, locked = newLockedStatus });
        }
    }
}