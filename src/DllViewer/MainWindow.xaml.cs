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
        private bool _selectedItemChanging;

        public MainWindow(DllViewerContext context = null)
        {
            InitializeComponent();
            Context = context ?? new DllViewerContext();
            ResolveAssemblies(Context);

            _selectedItemChanging = true;
            this.DataContext = context;
            dataGrid1.ItemsSource = context.Assemblies;
            dataGrid1.SelectedItem = context.SelectedAssembly;
            _selectedItemChanging = false;

            dataGrid1.UpdateLayout();
            dataGrid1.ScrollIntoView(dataGrid1.SelectedItem);
        }

        private void ResolveAssemblies(DllViewerContext context)
        {
            var path = context.CommandLine.FirstOrDefault();

            if (path == null)
            {
                var thisExe = Assembly.GetExecutingAssembly();
                path = thisExe.Location;
            }

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

            var assemblies = directoryPath == null
                ? new AssemblyInfo[0]
                : Directory.GetFiles(directoryPath, "*.exe")
                    .Union(
                        Directory.GetFiles(directoryPath, "*.dll"))
                    .Select(p => new AssemblyInfo(p))
                    .ToArray();

            var selectedFileName = IO.Path.GetFileName(selectedPath);
            var selectedAssembly = selectedPath == null 
                ? assemblies.FirstOrDefault()
                : assemblies.FirstOrDefault(a => IsFileNameEqual(a.Location, selectedFileName)) ?? assemblies.FirstOrDefault();

            for (int i = 0; i < assemblies.Length; i++)
                assemblies[i].Id = i + 1;

            context.Location = assemblies.Length == 0 ? "No assembly selected." : directoryPath;

            context.Assemblies = assemblies;
            context.SelectedAssembly = selectedAssembly;
        }

        private bool IsFileNameEqual(string path, string name)
        {
            return IO.Path.GetFileName(path).Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
        private void dataGrid1_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is DataGridRow row))
                return;
            if (!(row.DataContext is AssemblyInfo asmInfo))
                return;
            // do something
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_selectedItemChanging && dataGrid1.SelectedItem is AssemblyInfo asmInfo)
                Context.SelectedAssembly = asmInfo;
        }
    }
}
