using Gyminize.Core.Models;

namespace Gyminize.Core.Contracts.Services
{
    // Gỡ bỏ lớp này khi các trang/tính năng của bạn đang sử dụng dữ liệu của bạn.
    public interface ISampleDataService
    {
        // Lấy dữ liệu lưới nội dung không đồng bộ.
        Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();
    }
}