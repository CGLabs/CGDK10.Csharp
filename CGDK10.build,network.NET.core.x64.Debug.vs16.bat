@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO          Build Network Projects
@ECHO:
@ECHO            NET.core.x64.Degug
@ECHO        Build for Visual Studio 2019       
@ECHO ------------------------------------------
@ECHO @ Start Compile

@ECHO OFF
call "%VS142COMNTOOLS%..\..\VC\Auxiliary\Build\vcvarsall.bat" x86_x64


@ECHO ------------------------------------------
@ECHO  Configure (Debug/x64)
@ECHO ------------------------------------------
:COMPILE_BEGIN_DEBUG_X64

@ECHO @ Compile CGDK.buffer.NET.core
  devenv buffer\projects\CGDK10.buffer.NET.core.vs16.sln /rebuild "Debug|x64"

:COMPILE_COMMON_CLASSES_ANYCPU
@ECHO @ Compile CGDK10.system.NET.core
IF NOT EXIST "system\CGDK10.system.NET.core.vs16.sln" GOTO COMPILE_NETWORK_SOCKET_DEBUG_X64
  devenv system\CGDK10.system.NET.core.vs16.sln /rebuild "Debug|x64"

:COMPILE_NETWORK_SOCKET_DEBUG_X64
@ECHO @ Compile CGDK10.net.socket.NET.core
IF NOT EXIST "net.socket\CGDK10.net.socket.NET.core.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_DEBUG_X64
  devenv net.socket\CGDK10.net.socket.NET.core.vs16.sln /rebuild "Debug|x64"

:COMPILE_SERVER_SYSTEM_DEBUG_X64
@ECHO @ Compile CGDK10.server.system.NET.core
IF NOT EXIST "server.system\CGDK10.server.system.NET.core.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_CLASSES_DEBUG_X64
  devenv server.system\CGDK10.server.system.NET.core.vs16.sln /rebuild "Debug|x64"

:COMPILE_SERVER_SYSTEM_CLASSES_DEBUG_X64
@ECHO @ Compile CGDK10.server.query.NET.core
IF NOT EXIST "server.query\CGDK10.server.query.NET.core.vs16.sln" GOTO COMPILE_QUERY_CLASSES_DEBUG_X64
  devenv server.query\CGDK10.server.query.NET.core.vs16.sln /rebuild "Debug|x64"

:COMPILE_QUERY_CLASSES_DEBUG_X64
@ECHO @ Compile CGDK10.server.service.NET.core
IF NOT EXIST "server.service\CGDK10.server.service.NET.core.vs16.sln" GOTO COMPILE_END_DEBUG_X64
  devenv server.service\CGDK10.server.service.NET.core.vs16.sln /rebuild "Debug|x64"

:COMPILE_END_DEBUG_X64


:COMPILE_COMPLETE
@ECHO ------------------------------------------
@ECHO @ Complete!!! (For Visual Studio 2019)
@ECHO          NET.core.x64.Degug
@ECHO ------------------------------------------

pause
