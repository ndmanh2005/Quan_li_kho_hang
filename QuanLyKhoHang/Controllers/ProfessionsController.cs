using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;
using QuanLyKhoHang.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class ProfessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var professionsQuery = _context.Professions
                                           .Where(p => p.IsActive != -1) 
                                           .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                professionsQuery = professionsQuery.Where(p => p.Code.Contains(searchString) || p.Name.Contains(searchString));
            }

            var professions = await professionsQuery.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(professions);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Name,IsActive")] Profession profession)
        {
            if (ModelState.IsValid)
            {
                var existingProfession = await _context.Professions.FirstOrDefaultAsync(p => p.Code == profession.Code && p.IsActive != -1);
                if (existingProfession != null)
                {
                    TempData["ErrorMessage"] = "Mã ngành nghề đã tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                profession.CreatedAt = DateTime.Now;
                profession.ModifiedAt = DateTime.Now;

                _context.Add(profession);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới ngành nghề thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Thông tin không hợp lệ, vui lòng thử lại.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,IsActive")] Profession profession)
        {
            if (id != profession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var professionToUpdate = await _context.Professions.FindAsync(id);
                    if (professionToUpdate == null || professionToUpdate.IsActive == -1)
                    {
                        return NotFound();
                    }

                    professionToUpdate.Name = profession.Name;
                    professionToUpdate.IsActive = profession.IsActive;
                    professionToUpdate.ModifiedAt = DateTime.Now;
                    
                    _context.Update(professionToUpdate);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật ngành nghề thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Professions.Any(e => e.Id == profession.Id))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var profession = await _context.Professions.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }



            profession.IsActive = -1; 
            profession.DeletedAt = DateTime.Now;

            _context.Update(profession);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Xóa ngành nghề thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}