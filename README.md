# Hệ Thống Quản Lý Thư Viện (LMS)

Hệ thống quản lý thư viện trực tuyến được xây dựng bằng ASP.NET Core 8 MVC và MySQL.

## Tính năng

### Dành cho User
- Tìm kiếm và xem danh sách sách (realtime search)
- Xem chi tiết thông tin sách kèm **điểm đánh giá trung bình**
- Gửi yêu cầu mượn sách (tối đa 3 cuốn cùng lúc)
- Hủy yêu cầu mượn đang chờ duyệt
- Xem lịch sử mượn sách với trạng thái chi tiết
- Tự trả sách đã mượn
- **Gia hạn mượn** thêm 7 ngày (tối đa 1 lần, không được gia hạn khi đã quá hạn)
- Cảnh báo sách quá hạn (số ngày quá hạn)
- **Đánh giá & nhận xét sách** (1–5 sao, chỉ sau khi đã trả sách)
- Toàn bộ tính năng yêu cầu đăng nhập

### Dành cho Admin
- Dashboard tổng quan với biểu đồ thống kê (Chart.js)
- Danh sách việc cần xử lý (quá hạn + chờ duyệt)
- Quản lý sách: thêm/sửa/xóa, phân trang
  - **Tra cứu thông tin sách tự động qua ISBN** (Open Library API)
  - Lọc sách theo tác giả với autocomplete
  - Lọc sách theo danh mục với autocomplete
- Quản lý danh mục với số sách mỗi danh mục
- Quản lý mượn sách:
  - Lọc theo trạng thái (Tất Cả / Chờ Duyệt / Đang Mượn / Quá Hạn / Đã Trả / Từ Chối)
  - Highlight đỏ các sách quá hạn, hiển thị số ngày quá hạn
  - Duyệt / Từ chối / Xác nhận trả sách
- Quản lý người dùng: tìm kiếm realtime, xem lịch sử mượn
- DueDate được tính từ ngày **duyệt** (không phải ngày gửi yêu cầu)

## Công nghệ sử dụng

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: MySQL 8.0
- **ORM**: Entity Framework Core
- **Authentication**: Cookie-based Authentication + BCrypt
- **Frontend**: Bootstrap 5, Bootstrap Icons, Chart.js
- **Template Engine**: Razor Views
- **External API**: Open Library API (ISBN lookup)

## Yêu cầu hệ thống

- .NET 8 SDK
- XAMPP (hoặc MySQL Server)
- Git

## Cài đặt và chạy

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
- Seed dữ liệu mẫu (1 admin + 5 danh mục)

### 5. Import dữ liệu mẫu (Khuyến nghị)

Để có đầy đủ dữ liệu test, import file SQL:

```bash
mysql -u root --password= --default-character-set=utf8mb4 < database_schema.sql
```

Hoặc dùng phpMyAdmin: Import → Chọn file `database_schema.sql` → Go

### 6. Truy cập

Mở trình duyệt: `https://localhost:5055` hoặc `http://localhost:5055`

## Database Setup

### Phương án A: Import SQL (Khuyến nghị)

File `database_schema.sql` bao gồm toàn bộ schema + seed data:

```bash
# UTF-8 trên Windows
cmd /c "chcp 65001 && mysql -u root --password= --default-character-set=utf8mb4 < database_schema.sql"
```

Hoặc phpMyAdmin: Import → Chọn `database_schema.sql` → Go

**Dữ liệu mẫu bao gồm:**
- 5 danh mục sách
- 20 sách mẫu (4 cuốn/danh mục)
- 4 user (1 admin + 3 user thường)
- 9 BorrowRecord phản ánh đủ mọi trạng thái
- 4 đánh giá sách

### Phương án B: EF Core Migrations

Nếu chỉ cần schema rỗng (không có seed data từ SQL):

1. Tạo database rỗng:
```sql
CREATE DATABASE lms_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```
2. Chạy `dotnet run` — EF Core sẽ tự tạo bảng và seed admin + danh mục

## Tài khoản mặc định

| Vai trò | Email | Mật khẩu |
|---------|-------|----------|
| **Admin** | admin@lms.com | Admin@123 |
| **User** | an.nguyen@email.com | User@123 |
| **User** | binh.tran@email.com | User@123 |
| **User** | chau.le@email.com | User@123 |

> Tài khoản User chỉ có sau khi import `database_schema.sql`. Có thể tự đăng ký tài khoản mới.

## Cấu trúc project

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
│   ├── Book.cs
│   ├── BorrowRecord.cs
│   ├── BookReview.cs
│   ├── Category.cs
│   └── User.cs
├── Views/              # Razor views
│   ├── Home/
│   ├── Book/           # Details kèm form đánh giá
│   ├── Borrow/         # History kèm nút gia hạn
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
│   └── js/search.js    # Realtime search
└── Program.cs          # Application entry point
```

## Quy trình sử dụng

### User
1. Đăng ký tài khoản → Đăng nhập
2. Tìm và chọn sách muốn mượn
3. Nhấn **Mượn Sách Này** → Chờ admin duyệt
4. Vào **Lịch Sử Mượn** → Nhấn **Gia Hạn** để gia hạn thêm 7 ngày (nếu chưa quá hạn)
5. Nhấn **Trả Sách** khi đọc xong
6. Sau khi trả, vào **Chi Tiết Sách** để để lại đánh giá (1–5 sao)

### Admin
1. Đăng nhập với tài khoản admin
2. Vào trang **Quản Trị**
3. Tab **Quản Lý Mượn Sách**:
   - Duyệt / Từ chối yêu cầu mượn
   - Xử lý trả sách
   - Theo dõi quá hạn
4. Quản lý sách: dùng tính năng lookup ISBN để điền thông tin tự động
5. Quản lý danh mục / người dùng

## Ghi chú

- Mỗi user chỉ được mượn tối đa **3 cuốn** cùng lúc
- Thời hạn mượn: **14 ngày** (tính từ ngày được duyệt)
- Gia hạn: tối đa **1 lần**, thêm **7 ngày**, không được gia hạn khi đã quá hạn
- Đánh giá sách: chỉ được đánh giá **sau khi trả sách**, mỗi user đánh giá **1 lần/cuốn**
- Sách trả quá hạn sẽ được đánh dấu trạng thái **Trả Muộn**
- User role **không được phép** truy cập trang Admin
- Admin role **không thể** mượn sách (chỉ quản lý)

## Liên hệ

GitHub: [Danh25204](https://github.com/Danh25204)

---

© 2026 Hệ Thống Quản Lý Thư Viện. Bản quyền được bảo lưu.
