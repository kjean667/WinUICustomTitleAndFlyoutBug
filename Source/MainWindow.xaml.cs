using Windows.Graphics;
using Microsoft.UI.Xaml;

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
  }
}
