@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO    Set DLL for AnyCPU [Debug] Confiqure
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/dll/AnyCPU/Debug' to '/dll'

@ECHO OFF

@ECHO   copy '/dll/NET.core/AnyCPU/Debug'
copy dll\NET.core\AnyCPU\Debug\netcoreapp3.1\*.* dll\ /y

pause
