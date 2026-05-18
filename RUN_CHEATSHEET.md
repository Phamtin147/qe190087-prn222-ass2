# FUNewsManagementSystem Run Cheatsheet

## 1. Yêu cầu trước khi chạy

- .NET SDK đang dùng trong project: `net10.0`
- MariaDB/MySQL chạy ở:
  - host: `localhost`
  - port: `3306`
  - user: `root`
  - password: `1`

Kiểm tra MariaDB có đăng nhập được không:

```bash
mariadb -h localhost -P 3306 -u root -p1 -e "SHOW DATABASES;"
```

Nếu máy chỉ có command `mysql` thì dùng:

```bash
mysql -h localhost -P 3306 -u root -p1 -e "SHOW DATABASES;"
```

## 2. Tạo database và bảng MariaDB

Đứng ở thư mục assignment:

```bash
cd /home/amtia/Documents/prn222/PRN222/Assignments
```

Import script MariaDB:

```bash
mariadb -h localhost -P 3306 -u root -p1 < FUNewsManagement_mariadb.sql
```

Hoặc nếu dùng `mysql`:

```bash
mysql -h localhost -P 3306 -u root -p1 < FUNewsManagement_mariadb.sql
```

Script này sẽ tạo database `FUNewsManagement`, các bảng, khóa ngoại, và seed account/category/tag cơ bản.

## 3. Kiểm tra connection string

File cấu hình:

```text
FUNewsManagementSystem.Web/appsettings.json
```

Connection string hiện tại:

```json
"ConnectionStrings": {
  "FUNewsManagement": "Server=localhost;Port=3306;Database=FUNewsManagement;User=root;Password=1;TreatTinyAsBoolean=true"
}
```

Nếu password/port MariaDB khác thì sửa ở đây.

## 4. Restore và build project

```bash
dotnet restore FUNewsManagementSystem.slnx
dotnet build FUNewsManagementSystem.slnx
```

Build thành công mong đợi:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## 5. Chạy web app

```bash
dotnet run --project FUNewsManagementSystem.Web/FUNewsManagementSystem.Web.csproj
```

Nếu muốn ép port cụ thể:

```bash
dotnet run --project FUNewsManagementSystem.Web/FUNewsManagementSystem.Web.csproj --urls http://localhost:5099
```

Sau đó mở trình duyệt:

```text
http://localhost:5099
```

## 6. Account đăng nhập mẫu

Admin account lấy từ `appsettings.json`:

```text
Email: admin@FUNewsManagementSystem.org
Password: @@abc123@@
```

Staff account lấy từ database MariaDB:

```text
Email: IsabellaDavid@FUNewsManagement.org
Password: @1
```

Lecturer account lấy từ database MariaDB:

```text
Email: EmmaWilliam@FUNewsManagement.org
Password: @1
```

## 7. Luồng test nhanh

1. Import DB bằng `FUNewsManagement_mariadb.sql`.
2. Chạy `dotnet build FUNewsManagementSystem.slnx`.
3. Chạy web app.
4. Vào `/Account/Login`.
5. Login bằng staff account:

```text
IsabellaDavid@FUNewsManagement.org / @1
```

Nếu login thành công, app sẽ redirect sang trang news/staff.

## 8. Lỗi thường gặp

### Không connect được DB

Kiểm tra MariaDB service đang chạy chưa:

```bash
systemctl status mariadb
```

Kiểm tra lại user/password:

```bash
mariadb -h localhost -P 3306 -u root -p1
```

### Unknown database `FUNewsManagement`

Bạn chưa import DB. Chạy lại:

```bash
mariadb -h localhost -P 3306 -u root -p1 < FUNewsManagement_mariadb.sql
```

### Port app bị trùng

Chạy app bằng port khác:

```bash
dotnet run --project FUNewsManagementSystem.Web/FUNewsManagementSystem.Web.csproj --urls http://localhost:5100
```

Mở:

```text
http://localhost:5100
```
