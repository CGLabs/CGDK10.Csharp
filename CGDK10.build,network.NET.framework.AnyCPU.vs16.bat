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
@ECHO  Configure (Debug/AnyCPU)
@ECHO ------------------------------------------
:COMPILE_BEGIN_DEBUG_ANYCPU

@ECHO @ Compile CGDK.buffer.NET.framework
  devenv buffer\projects\CGDK10.buffer.NET.framework.vs16.sln /rebuild "Debug|Any CPU"

:COMPILE_COMMON_CLASSES_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.system.NET.framework
IF NOT EXIST "system\CGDK10.system.NET.framework.vs16.sln" GOTO COMPILE_NETWORK_SOCKET_DEBUG_ANYCPU
  devenv system\CGDK10.system.NET.framework.vs16.sln /rebuild "Debug|Any CPU"

:COMPILE_NETWORK_SOCKET_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.net.socket.NET.framework
IF NOT EXIST "net.socket\CGDK10.net.socket.NET.framework.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_DEBUG_ANYCPU
  devenv net.socket\CGDK10.net.socket.NET.framework.vs16.sln /rebuild "Debug|Any CPU"

:COMPILE_SERVER_SYSTEM_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.server.system.NET.framework
IF NOT EXIST "server.system\CGDK10.server.system.NET.framework.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_CLASSES_DEBUG_ANYCPU
  devenv server.system\CGDK10.server.system.NET.framework.vs16.sln /rebuild "Debug|Any CPU"

:COMPILE_SERVER_SYSTEM_CLASSES_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.server.query.NET.framework
IF NOT EXIST "server.query\CGDK10.server.query.NET.framework.sln" GOTO COMPILE_QUERY_CLASSES_DEBUG_ANYCPU
  devenv server.query\CGDK10.server.query.NET.framework.sln /rebuild "Debug|Any CPU"

:COMPILE_QUERY_CLASSES_DEBUG_ANYCPU
@ECHO @ Compile CGDK10.server.service.NET.framework
IF NOT EXIST "server.service\CGDK10.server.service.NET.framework.sln" GOTO COMPILE_END_DEBUG_ANYCPU
  devenv server.service\CGDK10.server.service.NET.framework.sln /rebuild "Debug|Any CPU"

:COMPILE_END_DEBUG_ANYCPU


@ECHO ------------------------------------------
@ECHO  Configure (Release/AnyCPU)
@ECHO ------------------------------------------
:COMPILE_BEGIN_RELEASE_ANYCPU

@ECHO @ Compile CGDK.buffer.NET.framework
  devenv buffer\projects\CGDK10.buffer.NET.framework.vs16.sln /rebuild "Release|Any CPU"

:COMPILE_COMMON_CLASSES_RELEASE
@ECHO @ Compile CGDK10.system.NET.framework
IF NOT EXIST "system\CGDK10.system.NET.framework.vs16.sln" GOTO COMPILE_NETWORK_SOCKET_RELEASE_ANYCPU
  devenv system\CGDK10.system.NET.framework.vs16.sln /rebuild "Release|Any CPU"

:COMPILE_NETWORK_SOCKET_RELEASE_ANYCPU
@ECHO @ Compile CGDK10.net.socket.NET.framework
IF NOT EXIST "net.socket\CGDK10.net.socket.NET.framework.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_RELEASE_ANYCPU
  devenv net.socket\CGDK10.net.socket.NET.framework.vs16.sln /rebuild "Release|Any CPU"

:COMPILE_SERVER_SYSTEM_RELEASE_ANYCPU
@ECHO @ Compile CGDK10.server.system.NET.framework
IF NOT EXIST "server.system\CGDK10.server.system.NET.framework.vs16.sln" GOTO COMPILE_SERVER_SYSTEM_CLASSES_RELEASE_ANYCPU
  devenv server.system\CGDK10.server.system.NET.framework.vs16.sln /rebuild "Release|Any CPU"

:COMPILE_SERVER_SYSTEM_CLASSES_RELEASE_ANYCPU
@ECHO @ Compile CGDK10.server.query.NET.framework
IF NOT EXIST "server.query\CGDK10.server.query.NET.framework.sln" GOTO COMPILE_QUERY_CLASSES_RELEASE_ANYCPU
  devenv server.query\CGDK10.server.query.NET.framework.sln /rebuild "Release|Any CPU"

:COMPILE_QUERY_CLASSES_RELEASE_ANYCPU
@ECHO @ Compile CGDK10.CGDK10.server.service.NET.framework
IF NOT EXIST "server.service\CGDK10.server.service.NET.framework.sln" GOTO COMPILE_END_RELEASE_ANYCPU
  devenv server.service\CGDK10.server.service.NET.framework.sln /rebuild "Release|Any CPU"

:COMPILE_END_RELEASE_ANYCPU


:COMPILE_COMPLETE
@ECHO ------------------------------------------
@ECHO @ Complete!!! (For Visual Studio 2019)
@ECHO        All Projects (Release)
@ECHO ------------------------------------------

pause
