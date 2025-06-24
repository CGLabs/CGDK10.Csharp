@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO    Set DLL for x64 [Release] Confiqure   
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/bin/x64/Release' to '/bin'

@ECHO OFF

@ECHO   copy '/dll/NET.core/AnyCPU/Release'
copy dll\NET.core\AnyCPU\Release\netcoreapp3.1\*.* dll\ /y

pause
