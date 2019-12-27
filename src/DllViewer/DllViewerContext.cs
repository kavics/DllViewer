using System.ComponentModel;

namespace DllViewer
{
    public class DllViewerContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] CommandLine { get; }

        private AssemblyInfo[] _assemblies;
        public AssemblyInfo[] Assemblies
        {
            get => _assemblies;
            set
            {
                _assemblies = value;
                OnPropertyChanged(nameof(Assemblies));
            }
        }

        private AssemblyInfo _selectedAssembly;
        public AssemblyInfo SelectedAssembly
        {
            get => _selectedAssembly;
            set
            {
                _selectedAssembly = value;
                OnPropertyChanged(nameof(SelectedAssembly));
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }


        public DllViewerContext(string[] commandLineArguments = null)
        {
            CommandLine = commandLineArguments ?? new string[0];
        }
    }
}
