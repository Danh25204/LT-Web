# 📚 Hệ Thống Quản Lý Thư Viện (LMS)

Hệ thống quản lý thư viện trực tuyến được xây dựng bằng ASP.NET Core 8 MVC và MySQL.

## ✨ Tính năng

### Dành cho User
- 🔍 Tìm kiếm và xem danh sách sách
- 📖 Xem chi tiết thông tin sách
- 📝 Gửi yêu cầu mượn sách
- 📜 Xem lịch sử mượn sách
- ↩️ Tự trả sách đã mượn
- 👤 Đăng ký/Đăng nhập tài khoản

### Dành cho Admin
- 📊 Dashboard tổng quan hệ thống
- 📚 Quản lý sách (thêm/sửa/xóa)
- 👥 Quản lý người dùng
- 📋 Quản lý danh mục
- ✅ Duyệt/từ chối yêu cầu mượn sách
- 📥 Quản lý trả sách và theo dõi quá hạn

## 🛠️ Công nghệ sử dụng

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: MySQL 8.0
- **ORM**: Entity Framework Core
- **Authentication**: Cookie-based Authentication + BCrypt
- **Frontend**: Bootstrap 5, Bootstrap Icons
- **Template Engine**: Razor Pages

## 📋 Yêu cầu hệ thống

- .NET 8 SDK
- XAMPP (hoặc MySQL Server)
- Git

## 🚀 Cài đặt và chạy

### 1. Clone repository

```bash
git clone https://github.com/Danh25204/LT-Web.git
cd LT-Web
```

### 2. Cấu hình database

Mở XAMPP Control Panel và khởi động **MySQL**.

Tạo database mới:
```sql
CREATE DATABASE lms_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Cấu hình connection string

Kiểm tra file `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=lms_db;User=root;Password=;CharSet=utf8mb4;"
  }
}
```

### 4. Chạy project

```bash
cd "LMS Project/LMS Project"
dotnet run
```

Project sẽ tự động:
- Áp dụng migrations (tạo bảng)
- Seed dữ liệu mẫu (1 admin + 50 sách)

### 5. Truy cập

Mở trình duyệt: `https://localhost:5055` hoặc `http://localhost:5055`

## 🔐 Tài khoản mặc định

| Vai trò | Email | Mật khẩu |
|---------|-------|----------|
| **Admin** | admin@lms.com | Admin@123 |
| **User** | _Đăng ký mới_ | - |

## 📁 Cấu trúc project

```
LMS Project/
├── Controllers/         # MVC Controllers
│   ├── HomeController.cs
│   ├── BookController.cs
│   ├── BorrowController.cs
│   ├── AccountController.cs
│   ├── AdminController.cs
│   └── CategoryController.cs
├── Models/             # Domain models
├── Views/              # Razor views
│   ├── Home/
│   ├── Book/
│   ├── Borrow/
│   ├── Account/
│   ├── Admin/
│   └── Shared/
├── Services/           # Business logic
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/       # Data access layer
│   ├── Interfaces/
│   └── Implementations/
├── Data/               # DbContext & Migrations
├── wwwroot/            # Static files (CSS, JS, images)
└── Program.cs          # Application entry point
```

## 🎯 Quy trình sử dụng

### User
1. Đăng ký tài khoản → Đăng nhập
2. Tìm và chọn sách muốn mượn
3. Nhấn **Mượn Sách Này** → Chờ admin duyệt
4. Vào **Đang Mượn** → Nhấn **Trả Sách** khi đọc xong

### Admin
1. Đăng nhập với tài khoản admin
2. Vào trang **Quản Trị**
3. Tab **Quản Lý Mượn Sách**:
   - Duyệt/Từ chối yêu cầu mượn
   - Xử lý trả sách
   - Theo dõi quá hạn
4. Quản lý sách/danh mục/người dùng

## 📝 Ghi chú

- Mỗi user chỉ được mượn tối đa **3 cuốn** cùng lúc
- Thời hạn mượn: **14 ngày**
- Sách trả quá hạn sẽ được đánh dấu trạng thái **Quá Hạn**
- User role **không được phép** truy cập trang Admin
- Admin role **không thể** mượn sách (chỉ quản lý)

## 📞 Liên hệ

GitHub: [Danh25204](https://github.com/Danh25204)

---

© 2026 Hệ Thống Quản Lý Thư Viện. Bản quyền được bảo lưu.
