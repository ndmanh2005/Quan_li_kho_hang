using System.Collections.Generic;

namespace QuanLyKhoHang.Helpers
{
    public class MenuItem
    {
        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;
        
        public string? ParentName { get; set; } 

        public string GroupName { get; set; } = string.Empty;
        
        public List<string>? AvailableActions { get; set; } 
    }

    public static class PermissionHelper
    {
        public static class Actions
        {
            public const string View = "Xem";
            public const string Create = "Thêm mới";
            public const string Edit = "Sửa";
            public const string Delete = "Xóa";
            public const string Report = "Báo cáo";
        }

        public static List<string> AllActions { get; } = new List<string>
        {
            Actions.View, Actions.Create, Actions.Edit, Actions.Delete, Actions.Report
        };

        public static List<MenuItem> AllMenuItems { get; } = new List<MenuItem>
        {
            new MenuItem { Name = "System", DisplayName = "Hệ thống", ParentName = null, GroupName = "System" },
            new MenuItem { Name = "UserManagement", DisplayName = "Quản lý người dùng", ParentName = "System", GroupName = "System" },
            new MenuItem { Name = "RoleManagement", DisplayName = "Quản lý vai trò", ParentName = "System", GroupName = "System" },

            new MenuItem { Name = "Category", DisplayName = "Danh mục", ParentName = null, GroupName = "Category" },
            new MenuItem { Name = "CategoryWarehouse", DisplayName = "Danh mục kho hàng", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryUnit", DisplayName = "Đơn vị tính", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryCustomerSource", DisplayName = "Nguồn khách hàng", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryProfession", DisplayName = "Ngành nghề", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryMarket", DisplayName = "Thị trường", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryBusinessType", DisplayName = "Loại hình doanh nghiệp", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryProductGroup", DisplayName = "Nhóm sản phẩm", ParentName = "Category", GroupName = "Category" },
            new MenuItem { Name = "CategoryProduct", DisplayName = "Sản phẩm", ParentName = "Category", GroupName = "Category" },

            new MenuItem { Name = "Business", DisplayName = "Nghiệp vụ", ParentName = null, GroupName = "Business" },
            new MenuItem { Name = "BusinessCustomer", DisplayName = "Quản lý khách hàng", ParentName = "Business", GroupName = "Business" },
            new MenuItem { Name = "BusinessOpportunity", DisplayName = "Cơ hội bán hàng", ParentName = "Business", GroupName = "Business" },
            new MenuItem { Name = "BusinessQuote", DisplayName = "Báo giá", ParentName = "Business", GroupName = "Business" },
            new MenuItem { Name = "BusinessContract", DisplayName = "Hợp đồng", ParentName = "Business", GroupName = "Business" },
            new MenuItem { Name = "BusinessRevenueExpense", DisplayName = "Quản lý thu chi", ParentName = "Business", GroupName = "Business" },
            
            new MenuItem { Name = "Report", DisplayName = "Báo cáo", ParentName = null, GroupName = "Report" },
            new MenuItem { Name = "ReportSales", DisplayName = "Báo cáo doanh số", ParentName = "Report", GroupName = "Report" },
            new MenuItem { Name = "ReportDebt", DisplayName = "Báo cáo công nợ", ParentName = "Report", GroupName = "Report" },
            new MenuItem { Name = "ReportInventory", DisplayName = "Báo cáo tồn kho", ParentName = "Report", GroupName = "Report" },
            
            new MenuItem { Name = "Config", DisplayName = "Cấu hình", ParentName = null, GroupName = "Config" },
            new MenuItem { Name = "ConfigSystemParams", DisplayName = "Tham số hệ thống", ParentName = "Config", GroupName = "Config" },
            new MenuItem { Name = "ConfigEmail", DisplayName = "Cấu hình Email", ParentName = "Config", GroupName = "Config" },
            new MenuItem { Name = "ConfigAccessHistory", DisplayName = "Lịch sử truy cập", ParentName = "Config", GroupName = "Config" }
        };
        public static string GeneratePermissionKey(string menuName, string action)
        {
            string actionKey = action;
            if (action == Actions.View) actionKey = "View";
            if (action == Actions.Create) actionKey = "Create";
            if (action == Actions.Edit) actionKey = "Edit";
            if (action == Actions.Delete) actionKey = "Delete";
            if (action == Actions.Report) actionKey = "Report";
            
            return $"Permissions.{menuName}.{actionKey}";
        }
    }
}