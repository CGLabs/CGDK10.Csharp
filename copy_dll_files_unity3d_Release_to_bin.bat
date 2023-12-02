@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO   Set DLL for AnyCPU [Unity3D] Confiqure
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/dll/Unity3D/Release' to '/dll'

@ECHO OFF

@ECHO   copy '/dll/unity3d/Release'
copy dll\unity3d\Release\*.* dll\ /y

pause
