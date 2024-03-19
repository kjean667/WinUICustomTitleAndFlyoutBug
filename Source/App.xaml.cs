namespace WinUICustomTitleBarInhibitsControlInput
{
  public partial class App
  {
    public App()
    {
      InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
      var window = new MainWindow();
      window.Activate();
    }

  }
}
