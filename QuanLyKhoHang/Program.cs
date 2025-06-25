using Microsoft.EntityFrameworkCore;
using QuanLyKhoHang.Data;

var builder = WebApplication.CreateBuilder(args);

OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("3 chang linh ngu lam");


// Ghi thẳng chuỗi kết nối vào code để kiểm tra
var connectionString = "Server=127.0.0.1;Port=3306;Database=quanlikhohang;User=root;Password=;";
// ==========================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();