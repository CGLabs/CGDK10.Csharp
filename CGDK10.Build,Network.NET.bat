@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO          Build Network Projects
@ECHO:
@ECHO             NET.AnyCPU
@ECHO        Build for Visual Studio       
@ECHO ------------------------------------------
@ECHO @ Start Compile

@ECHO OFF
call "%VS142COMNTOOLS%..\..\VC\Auxiliary\Build\vcvarsall.bat" x86_x64


@ECHO ------------------------------------------
@ECHO  Configure (Debug/AnyCPU)
@ECHO ------------------------------------------
:COMPILE_BEGIN_DEBUG_ANYCPU

@ECHO @ Compile CGDK.buffer.NET
  dotnet build buffer\projects\CGDK10.buffer.NET.sln -c "Debug" -a "Any CPU"

:COMPILE_COMMON_CLASSES_ANYCPU
@ECHO @ Compile CGDK10.system.NET
IF NOT EXIST "System\CGDK10.System.NET.sln" GOTO COMPILE_NETWORK_SOCKET_DEBUG_ANYCPU
  dotnet build System\CGDK10.System.NET.sln  -c "Debug"

:COMPILE_NETWORK_SOCKET_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.Net.Socket.NET
IF NOT EXIST "Net.Socket\CGDK10.Net.Socket.NET.sln" GOTO COMPILE_SERVER_SYSTEM_DEBUG_ANYCPU
  dotnet build Net.Socket\CGDK10.Net.Socket.NET.sln -c "Debug"

:COMPILE_SERVER_SYSTEM_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.server.system.NET
IF NOT EXIST "Server.System\CGDK10.server.system.NET.sln" GOTO COMPILE_SERVER_SYSTEM_CLASSES_DEBUG_ANYCPU
  dotnet build Server.System\CGDK10.server.system.NET.sln -c "Debug"

:COMPILE_END_DEBUG_ANYCPU


@ECHO ------------------------------------------
@ECHO  Configure (Release/AnyCPU)
@ECHO ------------------------------------------
:COMPILE_BEGIN_RELEASE_ANYCPU

@ECHO @ Compile CGDK.buffer.NET
  dotnet build buffer\projects\CGDK10.buffer.NET.sln /rebuild "Release|Any CPU"

:COMPILE_COMMON_CLASSES_RELEASE
@ECHO @ Compile CGDK10.system.NET
IF NOT EXIST "System\CGDK10.system.NET.sln" GOTO COMPILE_NETWORK_SOCKET_RELEASE_ANYCPU
  dotnet build System\CGDK10.system.NET.sln -c "Release" 

:COMPILE_NETWORK_SOCKET_RELEASE_ANYCPU
@ECHO @ Compile CGDK10.Net.Socket.NET
IF NOT EXIST "Net.Socket\CGDK10.Net.Socket.NET.sln" GOTO COMPILE_SERVER_SYSTEM_RELEASE_ANYCPU
  dotnet build Net.Socket\CGDK10.Net.Socket.NET.sln -c "Release"

:COMPILE_SERVER_SYSTEM_RELEASE_ANYCPU
@ECHO @ Compile CGDK10.server.system.NET
IF NOT EXIST "Server.System\CGDK10.server.system.NET.sln" GOTO COMPILE_SERVER_SYSTEM_CLASSES_RELEASE_ANYCPU
  dotnet build Server.System\CGDK10.server.system.NET.sln -c "Release"

:COMPILE_END_RELEASE_ANYCPU


:COMPILE_COMPLETE
@ECHO ------------------------------------------
@ECHO @ Complete!!! (For Visual Studio)
@ECHO           NET.AnyCPU
@ECHO ------------------------------------------

pause
