using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DllViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        internal DllViewerContext Context { get; set; }
        private readonly bool _selectedItemChanging;

        public MainWindow(DllViewerContext context = null)
        {
            InitializeComponent();
            Context = context ?? new DllViewerContext();
            ResolveAssemblies(Context);

            _selectedItemChanging = true;
            this.DataContext = Context;
            DataGrid1.ItemsSource = Context.Assemblies;
            DataGrid1.SelectedItem = Context.SelectedAssembly;
            _selectedItemChanging = false;

            DataGrid1.UpdateLayout();
            DataGrid1.ScrollIntoView(DataGrid1.SelectedItem);
        }

        private static void ResolveAssemblies(DllViewerContext context)
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
            }
            else if (File.Exists(path))
            {
                directoryPath = Path.GetDirectoryName(path);
                selectedPath = path;
            }

            var assemblies = directoryPath == null
                ? new AssemblyInfo[0]
                : Directory.GetFiles(directoryPath, "*.exe")
                    .Union(
                        Directory.GetFiles(directoryPath, "*.dll"))
                    .Select(p => new AssemblyInfo(p))
                    .ToArray();

            var selectedFileName = Path.GetFileName(selectedPath);
            var selectedAssembly = selectedPath == null 
                ? assemblies.FirstOrDefault()
                : assemblies.FirstOrDefault(a => IsFileNameEqual(a.Location, selectedFileName)) ?? assemblies.FirstOrDefault();

            for (var i = 0; i < assemblies.Length; i++)
                assemblies[i].Id = i + 1;

            context.Location = assemblies.Length == 0 ? "No assembly selected." : directoryPath;

            context.Assemblies = assemblies;
            context.SelectedAssembly = selectedAssembly;
        }

        private static bool IsFileNameEqual(string path, string name)
        {
            return Path.GetFileName(path ?? string.Empty).Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
        private void dataGrid1_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (!(e.Source is DataGridRow row))
            //    return;
            //if (!(row.DataContext is AssemblyInfo asmInfo))
            //    return;
            //// do something
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_selectedItemChanging && DataGrid1.SelectedItem is AssemblyInfo asmInfo)
                Context.SelectedAssembly = asmInfo;
        }
    }
}
