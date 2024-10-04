using System.Runtime.InteropServices;
using System.Text;

namespace Gyminize.Helpers;

// Lớp trợ giúp cho runtime
// Cung cấp phương thức để kiểm tra xem ứng dụng có đang chạy dưới dạng gói MSIX hay không.
public class RuntimeHelper
{
    // Khai báo hàm GetCurrentPackageFullName từ kernel32.dll
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

    // Thuộc tính kiểm tra xem ứng dụng có đang chạy dưới dạng gói MSIX hay không
    public static bool IsMSIX
    {
        get
        {
            var length = 0;

            // Nếu hàm GetCurrentPackageFullName trả về mã lỗi 15700L, ứng dụng không chạy dưới dạng gói MSIX
            return GetCurrentPackageFullName(ref length, null) != 15700L;
        }
    }
}
