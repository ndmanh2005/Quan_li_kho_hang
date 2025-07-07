using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;
using QuanLyKhoHang.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class TiersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TiersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> SearchTiers(string? searchTerm)
        {
            var query = _context.Tiers.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }
            var tiers = await query.OrderByDescending(t => t.MinSpent).ToListAsync();
            return Json(tiers);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TierViewModel model)
        {
            var otherTiers = await _context.Tiers.Where(t => t.IsActive && t.Id != model.Id).ToListAsync();
            foreach (var other in otherTiers)
            {
                if (RangesOverlap(model.MinSpent, model.MaxSpent, other.MinSpent, other.MaxSpent))
                    return Json(new { success = false, message = $"Khoảng chi tiêu bị trùng với hạng '{other.Name}'." });
            }
            var tier = new Tier { /* ... gán giá trị ... */ };
             _context.Tiers.Add(tier);
            await _context.SaveChangesAsync();
            await AddToHistory(tier, "ADD");
            return Json(new { success = true, data = tier });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] TierViewModel model)
        {
            var tierToUpdate = await _context.Tiers.FindAsync(model.Id);
            if (tierToUpdate == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hạng khách hàng." });
            }

            var otherTiers = await _context.Tiers.Where(t => t.IsActive && t.Id != model.Id).ToListAsync();
            foreach (var other in otherTiers)
            {
                 if (RangesOverlap(model.MinSpent, model.MaxSpent, other.MinSpent, other.MaxSpent))
                    return Json(new { success = false, message = $"Khoảng chi tiêu bị trùng với hạng '{other.Name}'." });
            }

            tierToUpdate.Name = model.Name;
            tierToUpdate.MinSpent = model.MinSpent;
            tierToUpdate.MaxSpent = model.MaxSpent;
            tierToUpdate.MinOrders = model.MinOrders;
            tierToUpdate.MaxOrders = model.MaxOrders;
            tierToUpdate.MinPoint = model.MinPoint;
            tierToUpdate.MaxPoint = model.MaxPoint;
            tierToUpdate.IsDefault = model.IsDefault;
            tierToUpdate.IsActive = model.IsActive;
            tierToUpdate.ModifiedAt = DateTime.UtcNow;

            if (model.IconMobileFile != null)
                tierToUpdate.IconMobile = await SaveFile(model.IconMobileFile);
            if (model.IconWebFile != null)
                tierToUpdate.IconWeb = await SaveFile(model.IconWebFile);

            await _context.SaveChangesAsync();
            await AddToHistory(tierToUpdate, "EDIT");

            return Json(new { success = true, data = tierToUpdate });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tierToDelete = await _context.Tiers.FindAsync(id);
            if (tierToDelete == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hạng khách hàng." });
            }



            _context.Tiers.Remove(tierToDelete);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa thành công!" });
        }


        private bool RangesOverlap<T>(T? minA, T? maxA, T? minB, T? maxB) where T : struct, IComparable
        {
            if (minA == null || maxA == null || minB == null || maxB == null) return false;
            return minA.Value.CompareTo(maxB.Value) < 0 && minB.Value.CompareTo(maxA.Value) < 0;
        }
        private async Task<string> SaveFile(IFormFile file) { /* ... */ return ""; }
        private async Task AddToHistory(Tier tier, string action) { /* ... */ }

    }
}