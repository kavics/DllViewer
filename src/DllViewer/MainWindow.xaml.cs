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
            ResolveAssemblies(Context);
            this.DataContext = context;
            dataGrid1.ItemsSource = context.Assemblies;
        }

        private void ResolveAssemblies(DllViewerContext context)
        {
            var path = context.CommandLine.FirstOrDefault();

            if (path == null)
            {
                var thisExe = Assembly.GetExecutingAssembly();
                path = thisExe.Location;
            }

            AssemblyInfo[] assemblies;
            AssemblyInfo selectedAssembly;
            string directoryPath = null;
            string selectedPath = null;
            if (Directory.Exists(path))
            {
                directoryPath = path;
                selectedPath = null;
            }
            else if (File.Exists(path))
            {
                directoryPath = IO.Path.GetDirectoryName(path);
                selectedPath = path;
            }

            assemblies = directoryPath == null
                ? new AssemblyInfo[0]
                : GetAssemblies(directoryPath);

            selectedAssembly = selectedPath == null 
                ? assemblies.FirstOrDefault()
                : selectedAssembly = assemblies
                    .Where(a => a.Location.Equals(selectedPath, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

            for (int i = 0; i < assemblies.Length; i++)
                assemblies[i].Id = i + 1;

            context.Location = assemblies.Length == 0 ? "No assembly selected." : directoryPath;

            context.Assemblies = assemblies;
            context.SelectedAssembly = selectedAssembly;
        }

        private AssemblyInfo[] GetAssemblies(string path)
        {
            if (path == null)
                return null;
            if (Directory.Exists(path))
                return GetFolderInfo(path);
            if (File.Exists(path))
                return new[] { new AssemblyInfo(path) };
            return null;
        }

        private AssemblyInfo[] GetFolderInfo(string path)
        {
            return Directory.GetFiles(path, "*.exe")
                .Union(
                    Directory.GetFiles(path, "*.dll"))
                .Select(p => new AssemblyInfo(p))
                .ToArray();
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
