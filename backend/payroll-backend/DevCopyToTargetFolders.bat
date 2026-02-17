ECHO BATCH Copy Started


SET DllVersion=%1
SET DllTargetDir=%2
SET DllTargetName=%3


SET BaseCodeFolderPath=%~dp0..\..\

Rem Replace or add files in packages folder
REM SET SomeAppPackagesRelativeFolderForDev=%BaseCodeFolderPath%ghwebprojs\GhCloud\packages

REM SET DllPackagesFolderPath=%SomeAppPackagesRelativeFolderForDev%\%DllTargetName%.%DllVersion%\lib\netstandard2.0
REM ECHO Target Path - %DllPackagesFolderPath%
REM IF EXIST %DllPackagesFolderPath% (
	REM copy /Y "%DllTargetDir%\%DllTargetName%.dll" "%DllPackagesFolderPath%\%DllTargetName%.dll"
	REM copy /Y "%DllTargetDir%\%DllTargetName%.pdb" "%DllPackagesFolderPath%\%DllTargetName%.pdb"
	REM copy /Y NUL "%DllPackagesFolderPath%\FILES_FROM_LOCAL_DEV"
REM )ELSE (ECHO DOES NOT EXIST - %DllPackagesFolderPath% )


Rem Replace or add files in Target folder
SET SomeAppPackagesRelativeFolderForDev=%BaseCodeFolderPath%FrontEnd

REM SET DllPackagesFolderPath=%SomeAppPackagesRelativeFolderForDev%\%DllTargetName%.%DllVersion%\lib\netstandard2.0
SET DllPackagesFolderPath=%SomeAppPackagesRelativeFolderForDev%
ECHO Target Path - %DllPackagesFolderPath%
IF EXIST %DllPackagesFolderPath% (
	ECHO "%DllTargetDir%\%DllTargetName%.dll"
	copy /Y "%DllTargetDir%\%DllTargetName%.dll" "%DllPackagesFolderPath%\%DllTargetName%.dll"
	copy /Y "%DllTargetDir%\%DllTargetName%.pdb" "%DllPackagesFolderPath%\%DllTargetName%.pdb"
	copy /Y NUL "%DllPackagesFolderPath%\FILE_FROM_LOCAL_DEV_%DllTargetName%"
)ELSE (ECHO DOES NOT EXIST - %DllPackagesFolderPath% )


ECHO BATCH Copy Ended
pause