# STROOP
*SuperMario64 Technical Runtime Observer and Object Processor*

  STROOP is a diagnostic tool for Super Mario 64 which displays and allows for simple editing of various game values and information. It can connect to a running emulator and update values in real time. Some core features include viewing loaded/unloaded objects, viewing Mario structure variables, viewing camera + HUD values, viewing overhead maps of courses, and viewing many more.
  
  *(...I finally rewrote this description... later - Dane)*
  
       
## Downloading Stroop

The latest release of Stroop can be downloaded from our [Releases Page](https://github.com/SM64-STROOP/STROOP/releases). From here .zip files of recent builds can be found. The files can then be extracted and Stroop.exe can be opened.

Latest development builds with the newest features, bug fixes (...I mean bug introductions) can be found on the continuous [Development Release](https://github.com/SM64-STROOP/STROOP/releases/vDev). Likewise, Stroop.exe can be run.
  
## Requirements

  As if the current build, Stroop has the following system requirements:
  * Windows 10 / Windows 8.1 / Windows 7 64-bit or 32-bit
  * OpenGL 3.0 or greater (3.0 requirement for map tab, 1.0 requirement for model tab only) 
  * .NET Framework 4.7 (See [.NET Framework System Requirements](https://msdn.microsoft.com/en-us/library/8z6watww(v=vs.110).aspx) for more information)
  * [Mupen 0.5 rerecording](http://adelikat.tasvideos.org/emulatordownloads/mupen64-rr/Mupen64%20v8%20installer.zip) (0.5.1 will not work) (Bizhawk + Nemu partially supported on Development branch)
  * 64 Marios (Must be super)
  * Marios must be American, Japanese or PAL
  
## Status 
  
  Stroop is currently under a development phase, however, pre-releases have been made. Current priorities include various new features and high-priority bug fixes. Prior Release 1.0, minor bug fixes will be addressed, and after, code cleanup will be focused on.
 
## Building

Requirements:
  * Visual Studio *(2017 recommended)*
  * OpenTK 1.1 *(Version 1.1.1589.5942 used, others may work)*
  
Before building Stroop, OpenTK is a prerequisite. This is easiest installed by using the NuGet package manager. Stroop can be easily built from the source code by opening up the solution file in Visual Studio and performing a build. 
