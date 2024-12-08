using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Contracts.Services;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using Windows.Graphics;

namespace Gyminize.Services
{
    /// <summary>
    /// Dịch vụ quản lý cửa sổ ứng dụng, bao gồm các thao tác thay đổi kích thước, tiêu đề và các tính năng của cửa sổ.
    /// </summary>
    class WindowService : IWindowService
    {
        private readonly AppWindow _appWindow; ///< Cửa sổ ứng dụng chính.
        private readonly OverlappedPresenter _presenter; ///< Trình bày cửa sổ.

        /// <summary>
        /// Khởi tạo dịch vụ cửa sổ, lấy đối tượng AppWindow và OverlappedPresenter từ cửa sổ ứng dụng chính.
        /// </summary>
        public WindowService()
        {
            // Lấy AppWindow từ MainWindow
            var hWnd = WindowNative.GetWindowHandle(App.MainWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            // Lấy OverlappedPresenter từ AppWindow
            _presenter = _appWindow.Presenter as OverlappedPresenter;
        }

        /// <summary>
        /// Đặt tính năng thay đổi kích thước của cửa sổ.
        /// </summary>
        /// <param name="isResizable">Xác định xem cửa sổ có thể thay đổi kích thước hay không.</param>
        public void SetIsResizable(bool isResizable)
        {
            if (_presenter != null)
            {
                _presenter.IsResizable = isResizable;
            }
        }

        /// <summary>
        /// Đặt tính năng tối đa hóa của cửa sổ.
        /// </summary>
        /// <param name="isMaximizable">Xác định xem cửa sổ có thể tối đa hóa hay không.</param>
        public void SetIsMaximizable(bool isMaximizable)
        {
            if (_presenter != null)
            {
                _presenter.IsMaximizable = isMaximizable;
            }
        }

        /// <summary>
        /// Thay đổi kích thước cửa sổ.
        /// </summary>
        /// <param name="width">Chiều rộng mới của cửa sổ.</param>
        /// <param name="height">Chiều cao mới của cửa sổ.</param>
        public void SetWindowSize(int width, int height)
        {
            if (_appWindow != null)
            {
                var newSize = new SizeInt32(width, height);
                _appWindow.Resize(newSize);
            }
            else
            {
                Console.WriteLine("AppWindow chưa được khởi tạo.");
            }
        }

        /// <summary>
        /// Đặt tiêu đề cho cửa sổ. Nếu tiêu đề là chuỗi rỗng, tiêu đề sẽ được ẩn.
        /// </summary>
        /// <param name="title">Tiêu đề mới của cửa sổ.</param>
        public void SetTitle(string title)
        {
            if (_appWindow != null)
            {
                if (title == "")
                {
                    _appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
                    _appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                    _appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                }
                else
                {
                    _appWindow.TitleBar.ExtendsContentIntoTitleBar = false;
                    _appWindow.Title = title;
                }
            }
            else
            {
                Console.WriteLine("AppWindow chưa được khởi tạo.");
            }
        }
    }
}
