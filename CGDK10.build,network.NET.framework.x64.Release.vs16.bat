@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO          Build Network Projects
@ECHO:
@ECHO           All Projects (Release)
@ECHO        Build for Visual Studio 2019       
@ECHO ------------------------------------------
@ECHO @ Start Compile

@ECHO OFF
call "%VS142COMNTOOLS%..\..\VC\Auxiliary\Build\vcvarsall.bat" x86_x64


@ECHO ------------------------------------------
@ECHO  Configure (Release/x64)
@ECHO ------------------------------------------
:COMPILE_BEGIN_RELEASE_X64

@ECHO @ Compile CGDK.buffer.NET.framework
  devenv buffer\projects\CGDK10.buffer.NET.framework.vs16.sln /rebuild "Release|x64"

:COMPILE_COMMON_CLASSES_RELEASE_X64
@ECHO @ Compile CGDK10.system.NET.framework
IF NOT EXIST "system\CGDK10.system.NET.framework.vs16.sln" GOTO COMPILE_NETWORK_SOCKET_RELEASE_X64
  devenv system\CGDK10.system.NET.framework.vs16.sln /rebuild "Release|x64"

:COMPILE_NETWORK_SOCKET_RELEASE_X64
@ECHO @ Compile CGDK10.net.socket.NET.framework
IF NOT EXIST "net.socket\CGDK10.net.socket.NET.framework.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_RELEASE_X64
  devenv net.socket\CGDK10.net.socket.NET.framework.vs16.sln /rebuild "Release|x64"

:COMPILE_SERVER_SYSTEM_RELEASE_X64
@ECHO @ Compile CGDK10.server.system.NET.framework
IF NOT EXIST "server.system\CGDK10.server.system.NET.framework.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_CLASSES_RELEASE_X64
  devenv server.system\CGDK10.server.system.NET.framework.vs16.sln /rebuild "Release|x64"

:COMPILE_SERVER_SYSTEM_CLASSES_RELEASE_X64
@ECHO @ Compile CGDK10.server.query.NET.framework
IF NOT EXIST "server.query\CGDK10.server.query.NET.framework.sln" GOTO COMPILE_QUERY_CLASSES_RELEASE_X64
  devenv server.query\CGDK10.server.query.NET.framework.sln /rebuild "Release|x64"

:COMPILE_QUERY_CLASSES_RELEASE_X64
@ECHO @ Compile CGDK10.server.service.NET.framework
IF NOT EXIST "server.service\CGDK10.server.service.NET.framework.sln" GOTO COMPILE_END_RELEASE_X64
  devenv server.service\CGDK10.server.service.NET.framework.sln /rebuild "Release|x64"

:COMPILE_END_RELEASE_X64


:COMPILE_COMPLETE
@ECHO ------------------------------------------
@ECHO @ Complete!!! (For Visual Studio 2019)
@ECHO        All Projects (Release)
@ECHO ------------------------------------------

pause
