using Gyminize.Core.Helpers;

using Windows.Storage;
using Windows.Storage.Streams;

namespace Gyminize.Helpers;

// Sử dụng các phương thức mở rộng này để lưu trữ và truy xuất dữ liệu ứng dụng cục bộ và roaming
// Thêm chi tiết về lưu trữ và truy xuất dữ liệu ứng dụng tại https://docs.microsoft.com/windows/apps/design/app-settings/store-and-retrieve-app-data
public static class SettingsStorageExtensions
{
    // Phần mở rộng tệp cho các tệp JSON
    private const string FileExtension = ".json";

    // Kiểm tra xem lưu trữ roaming có khả dụng hay không
    public static bool IsRoamingStorageAvailable(this ApplicationData appData)
    {
        return appData.RoamingStorageQuota == 0;
    }

    // Lưu dữ liệu vào thư mục dưới dạng tệp JSON
    public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
    {
        var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
        var fileContent = await Json.StringifyAsync(content);

        await FileIO.WriteTextAsync(file, fileContent);
    }

    // Đọc dữ liệu từ tệp JSON trong thư mục
    public static async Task<T?> ReadAsync<T>(this StorageFolder folder, string name)
    {
        if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
        {
            return default;
        }

        var file = await folder.GetFileAsync($"{name}.json");
        var fileContent = await FileIO.ReadTextAsync(file);

        return await Json.ToObjectAsync<T>(fileContent);
    }

    // Lưu dữ liệu vào ApplicationDataContainer dưới dạng chuỗi JSON
    public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
    {
        settings.SaveString(key, await Json.StringifyAsync(value));
    }

    // Lưu chuỗi vào ApplicationDataContainer
    public static void SaveString(this ApplicationDataContainer settings, string key, string value)
    {
        settings.Values[key] = value;
    }

    // Đọc dữ liệu từ ApplicationDataContainer và chuyển đổi từ chuỗi JSON
    public static async Task<T?> ReadAsync<T>(this ApplicationDataContainer settings, string key)
    {
        object? obj;

        if (settings.Values.TryGetValue(key, out obj))
        {
            return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    // Lưu tệp vào thư mục với nội dung byte
    public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name is null or empty. Specify a valid file name", nameof(fileName));
        }

        var storageFile = await folder.CreateFileAsync(fileName, options);
        await FileIO.WriteBytesAsync(storageFile, content);
        return storageFile;
    }

    // Đọc tệp từ thư mục và trả về nội dung byte
    public static async Task<byte[]?> ReadFileAsync(this StorageFolder folder, string fileName)
    {
        var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

        if ((item != null) && item.IsOfType(StorageItemTypes.File))
        {
            var storageFile = await folder.GetFileAsync(fileName);
            var content = await storageFile.ReadBytesAsync();
            return content;
        }

        return null;
    }

    // Đọc nội dung byte từ StorageFile
    public static async Task<byte[]?> ReadBytesAsync(this StorageFile file)
    {
        if (file != null)
        {
            using IRandomAccessStream stream = await file.OpenReadAsync();
            using var reader = new DataReader(stream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)stream.Size);
            var bytes = new byte[stream.Size];
            reader.ReadBytes(bytes);
            return bytes;
        }

        return null;
    }

    // Lấy tên tệp với phần mở rộng .json
    private static string GetFileName(string name)
    {
        return string.Concat(name, FileExtension);
    }
}
