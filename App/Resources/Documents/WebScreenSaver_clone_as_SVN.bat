:: WebScreenSaver_clone_as_SVN.bat
::-------------------------------------------------------------
SET prg="%ProgramFiles%\TortoiseSVN\bin\svn.exe"
IF NOT EXIST %prg% SET prg="%ProgramFiles(x86)%\TortoiseSVN\bin\svn.exe"
IF NOT EXIST %prg% SET prg="%ProgramW6432%\TortoiseSVN\bin\svn.exe"
%prg% checkout https://github.com/JocysCom/WebScreenSaver.git/trunk ".\WebScreenSaver"
pause
