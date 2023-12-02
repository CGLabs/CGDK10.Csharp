@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO     Set DLL for x64 [Debug] Confiqure   
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/bin/x64/Debug' to '/bin'

@ECHO OFF

@ECHO   copy '/dll/NET.core/AnyCPU/Release'
copy dll\NET.core\x64\Debug\netcoreapp3.1\*.* dll\ /y

pause
