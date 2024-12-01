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
    class WindowService : IWindowService
    {
        private readonly AppWindow _appWindow;
        private readonly OverlappedPresenter _presenter;

        public WindowService()
        {
            // Lấy AppWindow từ MainWindow
            var hWnd = WindowNative.GetWindowHandle(App.MainWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            // Lấy OverlappedPresenter từ AppWindow
            _presenter = _appWindow.Presenter as OverlappedPresenter;
        }

        public void SetIsResizable(bool isResizable)
        {
            if (_presenter != null)
            {
                _presenter.IsResizable = isResizable;
            }
        }

        public void SetIsMaximizable(bool isMaximizable)
        {
            if (_presenter != null)
            {
                _presenter.IsMaximizable = isMaximizable;
            }
        }

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
