using Windows.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace WinUICustomTitleBarInhibitsControlInput
{
  public sealed partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      AppWindow.Resize(new SizeInt32(500, 300));
      WindowTitleHelper.ExtendContentIntoTitleBar(this, MenuBarContent);
    }

    private void ListView_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
      MyFlyout.Hide();
    }
  }
}
