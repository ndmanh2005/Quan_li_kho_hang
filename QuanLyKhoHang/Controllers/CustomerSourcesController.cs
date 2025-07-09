using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data; 
using QuanLyKhoHang.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class CustomerSourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerSourcesController(ApplicationDbContext context)
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
            var query = _context.CustomerSources.Where(cs => cs.IsActive != -1).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(cs => cs.Code.Contains(searchTerm) || cs.Name.Contains(searchTerm));
            }

            var result = await query.OrderBy(cs => cs.Name).ToListAsync();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerSource customerSource)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            bool codeExists = await _context.CustomerSources.AnyAsync(cs => cs.Code == customerSource.Code && cs.IsActive != -1);
            if (codeExists)
            {
                return Json(new { success = false, message = "Mã nguồn khách hàng đã tồn tại trong hệ thống." });
            }

            customerSource.CreatedAt = DateTime.UtcNow;

            _context.CustomerSources.Add(customerSource);
            await _context.SaveChangesAsync();

            return Json(new { success = true, data = customerSource });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CustomerSource customerSource)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var sourceToUpdate = await _context.CustomerSources.FindAsync(customerSource.Id);
            if (sourceToUpdate == null)
            {
                return Json(new { success = false, message = "Không tìm thấy nguồn khách hàng." });
            }

            sourceToUpdate.Name = customerSource.Name;
            sourceToUpdate.IsActive = customerSource.IsActive;
            sourceToUpdate.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Json(new { success = true, data = sourceToUpdate });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var sourceToDelete = await _context.CustomerSources.FindAsync(id);
            if (sourceToDelete == null)
            {
                return Json(new { success = false, message = "Không tìm thấy nguồn khách hàng." });
            }

            sourceToDelete.IsActive = -1;
            sourceToDelete.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa thành công!" });
        }
    }
}