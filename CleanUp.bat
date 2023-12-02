@ECHO ------------------------------------------
@ECHO                CGDK8 for C#
@ECHO          clean up projects directories
@ECHO:
@ECHO ------------------------------------------

@ECHO @ delete temporary directories...
rd "buffer\projects\buffer\obj"  /S /Q >nul
rd "net.socket\socket\obj"  /S /Q >nul
rd "net.socket\wcf_controls\obj"  /S /Q >nul
rd "net.socket\winform_controls\obj"  /S /Q >nul
rd "server.service\doss\obj"  /S /Q >nul
rd "server.system\group\obj"  /S /Q >nul
rd "server.system\log\obj"  /S /Q >nul
rd "server.system\service\obj"  /S /Q >nul
rd "server.system\time_event\obj"  /S /Q >nul
rd "server.system\winform_controls\obj"  /S /Q >nul
rd "server.system\wpf_controls\obj"  /S /Q >nul
rd "system\object\obj"  /S /Q >nul
rd "system\winform_controls\obj"  /S /Q >nul
rd "system\wpf_controls\obj"  /S /Q >nul

pause
