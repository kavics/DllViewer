using System.Windows;

namespace DllViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wnd = new MainWindow(new DllViewerContext(e.Args));
            wnd.Show();
        }
    }
}
