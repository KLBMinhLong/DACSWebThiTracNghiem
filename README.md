
# 📘 Hệ thống thi trắc nghiệm trực tuyến - ASP.NET Core

Đây là đồ án cơ sở thực hiện tại Trường Đại học Công nghệ TP.HCM (HUTECH), với mục tiêu xây dựng một hệ thống thi trắc nghiệm trực tuyến hỗ trợ làm bài, quản lý đề thi, người dùng, và đánh giá kết quả.

---

## 👨‍💻 Thành viên thực hiện

| Họ tên              | MSSV         | Lớp      |
|---------------------|--------------|----------|
| Đinh Thanh Dân      | 2280619005   | 22DTHE5  |
| Nguyễn Minh Long    | 2280601773   | 22DTHE5  |
| Đào Văn Thắng       | 2280602984   | 22DTHE5  |

**Giảng viên hướng dẫn**: ThS. Bùi Mạnh Toàn

---

## 🚀 Chức năng nổi bật

### 👤 Người dùng
- Đăng nhập hệ thống bằng tài khoản
- Làm bài thi theo đề thi được mở
- Tự động lưu câu trả lời từng câu
- Không mất trạng thái khi reload/mất mạng
- Xem kết quả chi tiết sau khi thi

### 🧑‍💼 Quản trị viên (Admin)
- Quản lý danh sách đề thi, câu hỏi, chủ đề
- Nhập câu hỏi từ file Excel
- Quản lý người dùng và phân quyền
- Xem lịch sử thi, lọc theo đề/người dùng
- Quản lý liên hệ gửi từ người dùng

### 💬 Bình luận
- Bình luận theo đề thi, hiển thị thời gian thực
- Cho phép người dùng xóa bình luận của mình

---

## 🛠 Công nghệ sử dụng

- ASP.NET Core MVC (.NET 6)
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Authentication bằng Cookie + Role-based Authorization
- ClosedXML để import Excel
- AJAX/fetch API để xử lý bình luận và thời gian thực

---

## ⚙️ Cài đặt & chạy hệ thống

### 1. Clone project:

```bash
git clone https://github.com/your-username/thi-trac-nghiem.git
```

### 2. Cấu hình CSDL:

- Mở `appsettings.json` và cập nhật chuỗi kết nối:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ThiTracNghiemDb;Trusted_Connection=True;"
}
```

- Chạy migration:
```bash
dotnet ef database update
```

### 3. Chạy ứng dụng:
```bash
dotnet run
```
Mặc định sẽ chạy tại: http://localhost:5000

---

## 🔐 Tài khoản demo

| Vai trò | Tài khoản | Mật khẩu |
|---------|-----------|----------|
| Admin   | admin     | 123456   |
| User    | user      | 123456   |

---

## 🖼 Giao diện

- Giao diện responsive, thân thiện, hỗ trợ hình ảnh + âm thanh trong câu hỏi
- Giao diện thi rõ ràng, có đếm giờ, lưu từng đáp án
- Quản trị chia tách với người dùng, bảo vệ bằng role

---

## 📄 Bản quyền

Dự án thực hiện trong khuôn khổ học phần Đồ án cơ sở - Trường Đại học Công nghệ TP.HCM (HUTECH).  
Không sử dụng cho mục đích thương mại nếu không có sự cho phép của nhóm thực hiện.

