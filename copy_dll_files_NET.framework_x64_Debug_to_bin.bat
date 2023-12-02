@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO     Set DLL for x64 [Debug] Confiqure   
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/bin/x64/Debug' to '/bin'

@ECHO OFF

@ECHO   copy '/dll/NET.framework/AnyCPU/Release'
copy dll\NET.framework\x64\Debug\*.* dll\ /y

pause
