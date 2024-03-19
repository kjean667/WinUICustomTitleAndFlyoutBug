using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using Windows.Graphics;
using Windows.UI;
using WinRT.Interop;

namespace WinUICustomTitleBarInhibitsControlInput
{
  public static class WindowTitleHelper
  {
    public static void ExtendContentIntoTitleBar(Window window, FrameworkElement? dragAreaElement)
    {
      if (dragAreaElement != null && AppWindowTitleBar.IsCustomizationSupported())
      {
        dragAreaElement.LayoutUpdated += (_, _) => { ExtendContentIntoTitleBarAndUpdateDragArea(window, dragAreaElement); };
      }
    }

    private static void ExtendContentIntoTitleBarAndUpdateDragArea(Window window, FrameworkElement dragAreaElement)
    {
      var appWindow = GetAppWindowForCurrentWindow(window);
      if (appWindow == null)
      {
        return;
      }
      var titleBar = appWindow.TitleBar;

      titleBar.ButtonBackgroundColor = Colors.Transparent;
      titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
      titleBar.ButtonHoverBackgroundColor = Color.FromArgb(40, 255, 255, 255);
      titleBar.ButtonPressedBackgroundColor = Color.FromArgb(60, 255, 255, 255);

      titleBar.ButtonForegroundColor = Color.FromArgb(255, 200, 200, 200);
      titleBar.ButtonInactiveForegroundColor = Color.FromArgb(255, 100, 100, 100);
      titleBar.ButtonHoverForegroundColor = Color.FromArgb(255, 255, 255, 255);
      titleBar.ButtonPressedForegroundColor = Color.FromArgb(255, 200, 200, 200);

      titleBar.ExtendsContentIntoTitleBar = true;
      if (AppWindowTitleBar.IsCustomizationSupported() && titleBar.ExtendsContentIntoTitleBar)
      {
        try
        {
          var scale = GetScaleAdjustment(window);
          titleBar.SetDragRectangles(new[]
          {
            new RectInt32(
              (int)(dragAreaElement.ActualOffset.X * scale),
              (int)(dragAreaElement.ActualOffset.Y * scale),
              (int)(dragAreaElement.ActualWidth * scale),
              (int)(dragAreaElement.ActualHeight * scale))
          });
        }
        catch (Exception)
        {
          // Ignore
        }
      }
    }

    [DllImport("Shcore.dll", SetLastError = true)]
    internal static extern int GetDpiForMonitor(IntPtr hMonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

    internal enum MonitorDpiType
    {
      MDT_Effective_DPI = 0,
      MDT_Angular_DPI = 1,
      MDT_Raw_DPI = 2,
      MDT_Default = MDT_Effective_DPI
    }

    private static double GetScaleAdjustment(Window window)
    {
      var hWnd = WindowNative.GetWindowHandle(window);
      var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
      var displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
      var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

      var result = GetDpiForMonitor(hMonitor, MonitorDpiType.MDT_Default, out var dpiX, out _);
      if (result != 0)
      {
        throw new Exception("Could not get DPI for monitor.");
      }

      var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
      return scaleFactorPercent / 100.0;
    }

    private static AppWindow? GetAppWindowForCurrentWindow(Window window)
    {
      var hWnd = WindowNative.GetWindowHandle(window);
      var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
      return AppWindow.GetFromWindowId(wndId);
    }
  }
}
