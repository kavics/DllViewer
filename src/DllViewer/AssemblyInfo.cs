using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DllViewer
{
    public enum AssemblyKind { Unknown, Exe, Dll }

    public class AssemblyInfo
    {
        private Assembly _assembly;
        private AssemblyName _assemblyName;
        private FileInfo _fileInfo;

        public int Id { get; set; }
        public AssemblyKind Kind { get; }
        public string Name => _assemblyName.Name;
        public string FullName => _assemblyName.FullName;
        public string Location => _assembly.Location;

        public DateTime CreationTime => _fileInfo.CreationTime;
        public DateTime LastWriteTime => _fileInfo.LastWriteTime;
        public DateTime LastAccessTime => _fileInfo.LastAccessTime;

        public Version Version => _assemblyName.Version;
        public string FrameworkVersion => _assembly.ImageRuntimeVersion;

        public string Title { get; }
        public string Description { get; }
        public string Company { get; }
        public string Guid { get; }
        public string TargetFramework { get; }
        public string Copyright { get; }
        public string Product { get; }
        public string Trademark { get; }
        public string DebugInfo { get; }

        public AssemblyInfo(string path)
        {
            _fileInfo = new FileInfo(path);
            _assembly = Assembly.LoadFrom(path);
            _assemblyName = _assembly.GetName();

            switch (_fileInfo.Extension.ToLower())
            {
                case ".exe": Kind = AssemblyKind.Exe; break;
                case ".dll": Kind = AssemblyKind.Dll; break;
                default: Kind = AssemblyKind.Unknown; break;
            }

            foreach(var attr in _assembly.CustomAttributes)
            {
                switch (attr.AttributeType.FullName)
                {
                    case "System.Runtime.CompilerServices.CompilationRelaxationsAttribute": break;
                    case "System.Runtime.CompilerServices.RuntimeCompatibilityAttribute": break;
                    case "System.Diagnostics.DebuggableAttribute": DebugInfo = GetDebugInfo(attr.ConstructorArguments[0].Value); break;
                    case "System.Reflection.AssemblyConfigurationAttribute": break;
                    case "System.Runtime.InteropServices.ComVisibleAttribute": break;
                    case "System.Reflection.AssemblyFileVersionAttribute": break;
                    case "System.Reflection.AssemblyInformationalVersionAttribute": break;

                    case "System.Reflection.AssemblyProductAttribute": Product = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Reflection.AssemblyTrademarkAttribute": Trademark = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Reflection.AssemblyTitleAttribute": Title = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Reflection.AssemblyDescriptionAttribute": Description = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Reflection.AssemblyCompanyAttribute": Company = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Runtime.InteropServices.GuidAttribute": Guid = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Runtime.Versioning.TargetFrameworkAttribute": TargetFramework = attr.ConstructorArguments[0].Value.ToString(); break;
                    case "System.Reflection.AssemblyCopyrightAttribute": Copyright = attr.ConstructorArguments[0].Value.ToString(); break;
                }
            }
        }

        private string GetDebugInfo(object value)
        {
            if (!(value is Int32 modes))
                return string.Empty;
            return Convert.ToString(modes, 2);
        }

        public override string ToString()
        {
            return _assembly.ToString();
        }
    }
}
