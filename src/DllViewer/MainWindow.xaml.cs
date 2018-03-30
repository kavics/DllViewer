using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using IO = System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DllViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal DllViewerContext Context { get; set; }

        public MainWindow(DllViewerContext context = null)
        {
            InitializeComponent();
            Context = context ?? new DllViewerContext();
            // TextBlock1.Text = GetInfo(context.CommandLine.FirstOrDefault()) ?? "No available information.";
            var assemblies =  GetInfo(context.CommandLine.FirstOrDefault());
            for (int i = 0; i < assemblies.Length; i++)
                assemblies[i].Id = i + 1;
            dataGrid1.ItemsSource = assemblies;
        }

        private AssemblyInfo[] GetInfo(string path)
        {
            if (path == null)
                return null;
            if (Directory.Exists(path))
                return GetFolderInfo(path);
            if (File.Exists(path))
                return new[] { GetFileInfo(path) };
            return null;
        }

        private AssemblyInfo[] GetFolderInfo(string path)
        {
            return Directory.GetFiles(path, "*.exe")
                .Union(
                    Directory.GetFiles(path, "*.dll"))
                .Select(p => GetFileInfo(p))
                .ToArray();
        }

        private AssemblyInfo GetFileInfo(string path)
        {
            return new AssemblyInfo(path);
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int q = 1;
        }
        private void dataGrid1_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is DataGridRow row))
                return;
            if (!(row.DataContext is AssemblyInfo asmInfo))
                return;

            int q = 1;
        }
    }
}
