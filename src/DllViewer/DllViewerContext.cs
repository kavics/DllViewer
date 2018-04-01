using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllViewer
{
    public class DllViewerContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] CommandLine { get; }

        private AssemblyInfo[] _assemblies;
        public AssemblyInfo[] Assemblies
        {
            get { return _assemblies; }
            set
            {
                _assemblies = value;
                OnPropertyChanged(nameof(Assemblies));
            }
        }

        private AssemblyInfo _selectedAssembly;
        public AssemblyInfo SelectedAssembly
        {
            get { return _selectedAssembly; }
            set
            {
                _selectedAssembly = value;
                OnPropertyChanged(nameof(SelectedAssembly));
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
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
