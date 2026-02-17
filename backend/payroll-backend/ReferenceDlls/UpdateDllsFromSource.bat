ECHO BATCH Copy Started

SET SourceBasePath=D:\Ecstasy\Reno\boilerplate\
SET SourceProfile=Release
SET DestinationFolderPath=%~dp0

IF EXIST %SourceBasePath% (

	copy /Y "%SourceBasePath%Common\Foundation\Reno.Foundation.CommonUtils\bin\%SourceProfile%\netstandard2.0\Reno.Foundation.CommonUtils.dll" "%DestinationFolderPath%Reno.Foundation.CommonUtils.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Foundation.CommonUtils\bin\%SourceProfile%\netstandard2.0\Reno.Foundation.CommonUtils.pdb" "%DestinationFolderPath%Reno.Foundation.CommonUtils.pdb"
	
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.BAL\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.BAL.dll" "%DestinationFolderPath%Reno.Web.Foundation.BAL.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.BAL\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.BAL.pdb" "%DestinationFolderPath%Reno.Web.Foundation.BAL.pdb"
	
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.Config\bin\%SourceProfile%\netstandard2.0\Reno.Web.Foundation.Config.dll" "%DestinationFolderPath%Reno.Web.Foundation.Config.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.Config\bin\%SourceProfile%\netstandard2.0\Reno.Web.Foundation.Config.pdb" "%DestinationFolderPath%Reno.Web.Foundation.Config.pdb"
	
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.DAL\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.DAL.dll" "%DestinationFolderPath%Reno.Web.Foundation.DAL.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.DAL\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.DAL.pdb" "%DestinationFolderPath%Reno.Web.Foundation.DAL.pdb"

	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.DomainModels\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.DomainModels.dll" "%DestinationFolderPath%Reno.Web.Foundation.DomainModels.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.DomainModels\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.DomainModels.pdb" "%DestinationFolderPath%Reno.Web.Foundation.DomainModels.pdb"
	
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.ServiceModels\bin\%SourceProfile%\netstandard2.0\Reno.Web.Foundation.ServiceModels.dll" "%DestinationFolderPath%Reno.Web.Foundation.ServiceModels.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.ServiceModels\bin\%SourceProfile%\netstandard2.0\Reno.Web.Foundation.ServiceModels.pdb" "%DestinationFolderPath%Reno.Web.Foundation.ServiceModels.pdb"
	
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.Web\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.Web.dll" "%DestinationFolderPath%Reno.Web.Foundation.Web.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.Web\bin\%SourceProfile%\net6.0\Reno.Web.Foundation.Web.pdb" "%DestinationFolderPath%Reno.Web.Foundation.Web.pdb"
	
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.WebClient\bin\%SourceProfile%\netstandard2.0\Reno.Web.Foundation.WebClient.dll" "%DestinationFolderPath%Reno.Web.Foundation.WebClient.dll"
	copy /Y "%SourceBasePath%Common\Foundation\Reno.Web.Foundation.WebClient\bin\%SourceProfile%\netstandard2.0\Reno.Web.Foundation.WebClient.pdb" "%DestinationFolderPath%Reno.Web.Foundation.WebClient.pdb"
		
)ELSE (ECHO DOES NOT EXIST - %SourceBasePath% )

ECHO BATCH Copy Ended
pause