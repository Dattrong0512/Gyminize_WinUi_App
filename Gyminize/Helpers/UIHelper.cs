using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Helpers;

public static class UIHelper
{
    public static void UpdateLoadingSpinnerOnWinUI(bool isLoading)
    {
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        dispatcherQueue.TryEnqueue(() =>
        {
            if (isLoading)
            {
                var loadingSpinner = new ProgressRing
                {
                    IsActive = true,
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var rootGrid = GetRootGrid();
                if (rootGrid != null && !rootGrid.Children.Contains(loadingSpinner))
                {
                    rootGrid.Children.Add(loadingSpinner);
                }
            }
            else
            {
                var rootGrid = GetRootGrid();
                if (rootGrid != null)
                {
                    var spinner = rootGrid.Children.OfType<ProgressRing>().FirstOrDefault();
                    if (spinner != null)
                    {
                        rootGrid.Children.Remove(spinner);
                    }
                }
            }
        });
    }

    private static Grid? GetRootGrid()
    {
        if (App.MainWindow?.Content is Grid rootGrid)
        {
            return rootGrid;
        }
        return null;
    }
}
