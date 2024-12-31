# Hướng dẫn BACKUP Và RESTORE

- Chỉnh lại đường dẫn dến thư mục bin của Postgres đã cài, trong tệp. pg_bin_directory.exe
- **_Back up_**:
  - Chạy file backup.bat để thực hiện sao lưu.
  - Bản lưu được lưu trữ trong thư mục backup cùng đường dẫn.
- **_Restore_**:
  - Chạy file restore.bat để thực hiện phục hồi.
  - Cần có bản lưu trước đó trong thư mục backup (mặc định chọn bản lưu gần nhất).
- **_Schedule cho backup_**:
  - Nhóm sử dụng Storage trên Vercel chỉ hỗ trợ lưu trữ không có backup và restore nên chức năng lập lịch cũng làm thủ công trên máy (chỉ hỗ trợ Windows).
  - Mặc định sao lưu lúc 3 AM, chỉnh sửa biến TASK_TIME trong scheduler_on.bat để set lại giờ backup tự động.
  - Chạy scheduler_on.bat để tạo và enable task, scheduler_off.bat để disable và xóa task.
