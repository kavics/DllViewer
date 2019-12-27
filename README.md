# DllViewer

Simple Windows 10 application that shows a grid of the important properties of a selected .NET DLL and all .NET assemblies in the directory.

## Installation

Get latest binaries fom the [Github](https://github.com/kavics/DllViewer/releases "Github releases")

## Use from shell

The tool can be started from a shell (command-line, PowerShell) with one parameter. For example:

``` text
DllViewer.exe "C:\Program Files\MyTool\bin\MyTool.dll"
```

- The parameter can be an existing directory or file.
- If the parameter is missing, the tool starts with its own directory.
- If the file or directory is not found, the tool does not start.
- Only the first parameter is used.

## Use from explorer

Another starting option is the double click on a DLL file in the Windows Explorer. To achieve this, you need to modify the file association of the ".dll" extension.

- Double click on a ".dll" file
- Select "Open with..." from the context menu.
- Click "Show apps" and scroll down.
- Click "Look for another app on this PC".
- Check the "Always use this app for open .dll files" option.
- Browse the "DllViewer.exe" executable file from the installation directory.

...and achievement reached: double click on a ".dll" opens the tool.

For remove the association delete the tool's executable (or move or rename the "DllViewer.exe"), restart the computer and restore the application.

&lt;? Enjoy