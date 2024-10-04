// Sử dụng các thư viện cần thiết.
using System.Text;
using Gyminize.Core.Contracts.Services;
using Newtonsoft.Json;

namespace Gyminize.Core.Services
{
    // Lớp này cung cấp các thao tác với tệp tin như đọc, lưu và xóa tệp tin.
    public class FileService : IFileService
    {
        // Đọc tệp tin từ thư mục chỉ định và giải mã nội dung của nó thành kiểu dữ liệu chỉ định.
        public T Read<T>(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

        // Mã hóa nội dung và lưu nó vào tệp tin trong thư mục chỉ định.
        public void Save<T>(string folderPath, string fileName, T content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileContent = JsonConvert.SerializeObject(content);
            File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
        }

        // Xóa tệp tin chỉ định từ thư mục chỉ định.
        public void Delete(string folderPath, string fileName)
        {
            if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
        }
    }
}