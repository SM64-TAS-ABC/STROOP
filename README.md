# STROOP
*SuperMario64 Technical Runtime Observer and Object Processor*

  STROOP is a diagnostic tool for Super Mario 64 that displays and allows for simple editing of various game values and information. It can connect to a running emulator and update values in real time. Some core features include views of loaded/unloaded objects, Mario structure variables, camera + HUD values, an overhead map display, and many more.



## Downloading STROOP

The latest release of STROOP can be downloaded from our [Releases Page](https://github.com/SM64-TAS-ABC/STROOP/releases). From here .zip files of recent builds can be found. The files can then be extracted and stroop.exe can be started.

Latest development builds with the newest features, bug fixes can be found on the continuous [Development Release](https://github.com/SM64-TAS-ABC/STROOP/releases/vDev). Likewise, stroop.exe can be started.

## Requirements

  As of the current build, STROOP has the following system requirements:
  * Windows 10 / Windows 8.1 / Windows 8 / Windows 7 64-bit or 32-bit
  * OpenGL 3.0 or greater (3.0 requirement for map tab, 1.0 requirement for model tab only)
  * .NET Framework 4.6.1 (See [.NET Framework System Requirements](https://msdn.microsoft.com/en-us/library/8z6watww(v=vs.110).aspx) for more information)
  * Supported Emulators
    * Mupen
    * Bizhawk
    * Nemu
    * Mupen64Plus
    * ~~Project64~~ (broken)
    * Dolphin
  * 64 Marios (Must be super)
  * Marios must be American, Japanese, PAL, or Shindou

## Building

Requirements:
  * Visual Studio *(2017 recommended)*

OpenTK is a prerequisite for building STROOP. This is easiest installed by using the NuGet package manager. STROOP can be easily built from the source code by opening up the solution file in Visual Studio and performing a build.
