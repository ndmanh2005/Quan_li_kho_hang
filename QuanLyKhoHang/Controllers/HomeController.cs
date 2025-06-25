using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;
using QuanLyKhoHang.Models;
using System.Diagnostics;
using OfficeOpenXml;
using System.IO;

namespace QuanLyKhoHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // --- DANH SÁCH VÀ TÌM KIẾM ---
        // Hợp nhất 2 hàm Index thành 1, có chức năng tìm kiếm
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            // Chắc chắn rằng Warehouses không phải là null
            var warehousesQuery = _context.Warehouses.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                // Tìm kiếm theo Code hoặc Name
                warehousesQuery = warehousesQuery.Where(w => w.Code.Contains(searchString) || w.Name.Contains(searchString));
            }

            var warehouses = await warehousesQuery.ToListAsync();
            return View(warehouses);
        }

        // --- THÊM MỚI ---
        // GET: Hiển thị form để tạo mới
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý dữ liệu thêm mới từ form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Name,Location")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                warehouse.IsActive = 1; // Giá trị mặc định
                warehouse.IsDefault = false; // Giá trị mặc định
                warehouse.CreatedAt = DateTime.Now;
                warehouse.ModifiedAt = DateTime.Now;

                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // --- SỬA ---
        // GET: Hiển thị form để sửa, với thông tin của kho hàng có id được truyền vào
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        // POST: Xử lý lưu thông tin sau khi sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Location,IsActive,IsDefault,CreatedAt")] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    warehouse.ModifiedAt = DateTime.Now; // Cập nhật thời gian sửa
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Warehouses.Any(e => e.Id == warehouse.Id))
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
            return View(warehouse);
        }

        // --- XÓA ---
        // GET: Hiển thị trang xác nhận trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Thực hiện xóa kho hàng
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ExportToExcel()
        {
            // Lấy danh sách toàn bộ kho hàng từ database
            var warehouses = await _context.Warehouses.ToListAsync();



            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DanhSachKhoHang");

                // --- Tạo Header ---
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã kho";
                worksheet.Cells[1, 3].Value = "Tên kho hàng";
                worksheet.Cells[1, 4].Value = "Địa chỉ";
                worksheet.Cells[1, 5].Value = "Trạng thái";

                // --- Thêm dữ liệu ---
                for (int i = 0; i < warehouses.Count; i++)
                {
                    var warehouse = warehouses[i];
                    int rowIndex = i + 2; // Bắt đầu từ dòng 2

                    worksheet.Cells[rowIndex, 1].Value = i + 1;
                    worksheet.Cells[rowIndex, 2].Value = warehouse.Code;
                    worksheet.Cells[rowIndex, 3].Value = warehouse.Name;
                    worksheet.Cells[rowIndex, 4].Value = warehouse.Location;
                    worksheet.Cells[rowIndex, 5].Value = (warehouse.IsActive == 1) ? "Đang hoạt động" : "Ngừng hoạt động";
                }

                // Tự động điều chỉnh độ rộng của các cột
                worksheet.Cells.AutoFitColumns();

                // --- Trả file về cho client ---
                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);
                stream.Position = 0;

                string excelName = $"DanhSachKhoHang-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        public IActionResult DownloadTemplate()
        {


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Template");
                // Tạo header cho file mẫu
                worksheet.Cells[1, 1].Value = "Mã kho";
                worksheet.Cells[1, 2].Value = "Tên kho hàng";
                worksheet.Cells[1, 3].Value = "Địa chỉ";

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = "FileMau_KhoHang.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }       

        [HttpPost]
public async Task<IActionResult> ImportFromExcel(IFormFile file)
{
    if (file == null || file.Length == 0)
    {
        TempData["ErrorMessage"] = "Vui lòng chọn một file Excel để import.";
        return RedirectToAction(nameof(Index));
    }

    var errorRows = new List<(int Row, string ErrorMessage)>();
    var warehousesToAdd = new List<Warehouse>();

    using (var stream = new MemoryStream())
    {
        await file.CopyToAsync(stream);
        
        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                TempData["ErrorMessage"] = "File Excel không hợp lệ hoặc không có sheet nào.";
                return RedirectToAction(nameof(Index));
            }

            var rowCount = worksheet.Dimension.Rows;
            var existingCodesInDb = new HashSet<string>(await _context.Warehouses.Select(w => w.Code).ToListAsync());
            var codesInFile = new HashSet<string>();

            // --- Vòng 1: Đọc và kiểm tra dữ liệu ---
            for (int row = 2; row <= rowCount; row++)
            {
                var code = worksheet.Cells[row, 1].Value?.ToString().Trim();
                var name = worksheet.Cells[row, 2].Value?.ToString().Trim();

                // Kiểm tra lỗi
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
                {
                    errorRows.Add((row, "Mã kho và Tên kho là trường bắt buộc."));
                    continue;
                }
                if (existingCodesInDb.Contains(code))
                {
                    errorRows.Add((row, "Mã kho đã tồn tại trong cơ sở dữ liệu."));
                    continue;
                }
                if (!codesInFile.Add(code)) // Nếu không thêm được vào set -> đã tồn tại
                {
                    errorRows.Add((row, "Mã kho bị lặp lại trong chính file này."));
                    continue;
                }

                // Nếu không có lỗi, thêm vào danh sách chờ
                warehousesToAdd.Add(new Warehouse
                {
                    Code = code,
                    Name = name,
                    Location = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                    IsActive = 1,
                    IsDefault = false,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }

            // --- Vòng 2: Xử lý kết quả ---
            if (errorRows.Count > 0)
            {
                // Nếu có lỗi, thêm cột "Lỗi" và ghi chú thích
                var errorColumn = worksheet.Dimension.Columns + 1;
                worksheet.Cells[1, errorColumn].Value = "Lỗi";
                worksheet.Cells[1, errorColumn].Style.Font.Bold = true;

                foreach (var error in errorRows)
                {
                    worksheet.Cells[error.Row, errorColumn].Value = error.ErrorMessage;
                }
                
                // Lưu file có báo lỗi vào một stream mới để trả về
                var errorStream = new MemoryStream();
                await package.SaveAsAsync(errorStream);
                errorStream.Position = 0;
                
                string errorFileName = $"ImportErrors-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(errorStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", errorFileName);
            }
            else
            {
                // Nếu không có lỗi, lưu vào CSDL
                if (warehousesToAdd.Any())
                {
                    _context.Warehouses.AddRange(warehousesToAdd);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Import thành công {warehousesToAdd.Count} kho hàng!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không có dữ liệu mới hợp lệ để import.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}