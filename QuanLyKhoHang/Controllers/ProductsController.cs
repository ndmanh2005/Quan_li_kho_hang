using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data; 
using QuanLyKhoHang.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }

        public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Code.Contains(searchTerm) || p.Name.Contains(searchTerm));
            }

            int pageSize = 10; 
            var paginatedList = await query.OrderByDescending(p => p.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
            ViewData["TotalPages"] = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            ViewData["CurrentPage"] = pageNumber;
            ViewData["SearchTerm"] = searchTerm;

            return View(paginatedList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductViewModel();
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
          
                if (viewModel.ImageFile != null)
                {
                    viewModel.Product.ImageUrl = await SaveFile(viewModel.ImageFile);
                }

                _context.Products.Add(viewModel.Product);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            
            var product = await _context.Products
                
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var viewModel = new ProductViewModel { Product = product };
            await PopulateDropdowns(viewModel);
            
            return View(viewModel);
        }


        private async Task PopulateDropdowns(ProductViewModel viewModel)
        {
            
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(file.FileName);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
            Directory.CreateDirectory(uploadsFolder);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return "/uploads/products/" + uniqueFileName;
        }
    }
}