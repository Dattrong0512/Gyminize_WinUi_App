// Không gian tên Gyminize.Core.Contracts.Services chứa giao diện IFileService.
namespace Gyminize.Core.Contracts.Services
{
    // Giao diện IFileService định nghĩa các phương thức để thao tác với tệp tin.
    public interface IFileService
    {
        // Đọc nội dung từ tệp tin và trả về đối tượng kiểu T.
        T Read<T>(string folderPath, string fileName);

        // Lưu nội dung vào tệp tin.
        void Save<T>(string folderPath, string fileName, T content);

        // Xóa tệp tin.
        void Delete(string folderPath, string fileName);
    }
}