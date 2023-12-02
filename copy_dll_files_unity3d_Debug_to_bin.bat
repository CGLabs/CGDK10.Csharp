@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO   Set DLL for AnyCPU [unity3d] Confiqure
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/dll/unity3d/Debug' to '/bin'

@ECHO OFF

@ECHO   copy '/dll/unity3d/Debug'
copy dll\unity3d\Debug\*.* dll\ /y

pause
