@ECHO OFF
::"$(ProjectDir)Documents\JocysCom.sign.bat" "$(TargetPath)"
SET file=%~1
IF "%file%" == "" SET file=..\..\App\bin\Debug\WebScreenSaver.scr
CALL:SIG "%file%"
pause

GOTO:EOF
::=============================================================
:SIG :: Sign and Timestamp Code
::-------------------------------------------------------------
:: SIGNTOOL.EXE Note:
:: Use the Windows 7 Platform SDK web installer that lets you
:: download just the components you need. Just choose the
:: ".NET developer \ Tools" and deselect everything else.
set sgt=%ProgramFiles%\Microsoft SDKs\Windows\v7.1\Bin\signtool.exe
if not exist "%sgt%" set sgt=%ProgramFiles(x86)%\Microsoft SDKs\Windows\v7.1A\Bin\signtool.exe
if not exist "%sgt%" set sgt=%ProgramFiles(x86)%\Microsoft SDKs\ClickOnce\SignTool\signtool.exe
set pfx=D:\_Backup\Configuration\SSL\CodeSign_Standard\2016\Evaldas_Jocys.CodeSign.pfx
set d=Jocys.com Web Screen Saver
set du=http://www.jocys.com/projects/WebScreenSaver
set vsg=http://timestamp.verisign.com/scripts/timestamp.dll
if not exist "%sgt%" CALL:Error "%sgt%"
if not exist "%~1"   CALL:Error "%~1"
if not exist "%pfx%" CALL:Error "%pfx%"
"%sgt%" sign /f "%pfx%" /d "%d%" /du "%du%" /t "%vsg%" /v "%~1"
GOTO:EOF

:Error
echo File doesn't Exist: "%~1"
pause