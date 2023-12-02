@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO
@ECHO   Set DLL for AnyCPU [Release] Confiqure
@ECHO ------------------------------------------
@ECHO @ Copy DLL and PDB files in '/bin/NET.framework/AnyCPU/Release' to '/bin'

@ECHO OFF

@ECHO   copy '/dll/NET.framework/AnyCPU/Release'
copy dll\NET.framework\AnyCPU\Release\*.* dll\ /y

pause
