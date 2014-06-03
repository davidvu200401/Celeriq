rem Create temp folder
rmdir /s /q C:\Projects\PublishCeleriq\
mkdir C:\Projects\PublishCeleriq\
mkdir C:\Projects\PublishCeleriq\ClientTools\

rmdir /s /q C:\Projects\PublishCeleriqAgent\
mkdir C:\Projects\PublishCeleriqAgent\

rem FULL BUILD
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "C:\Projects\Celeriq\Celeriq.InMemory.sln" /t:Rebuild /p:Configuration=Release

rem Copy files to temp location
copy C:\Projects\Celeriq\Celeriq.WinService\bin\Release\*.exe C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\Celeriq.WinService\bin\Release\*.dll C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\Build\Celeriq.WinService.exe.config C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\Celeriq.WinService\InstallService.bat C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\Celeriq.WinService\UninstallService.bat C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\ThirdParty\System.Data.SQLite.dll C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\ThirdParty\AntiXSSLibrary.dll C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\Celeriq.WinService\Celeriq.WinService.exe.Template.config C:\Projects\PublishCeleriq\
copy C:\Projects\Celeriq\Docs\ReadMe.txt C:\Projects\PublishCeleriq\

rem Copy Tools
copy C:\Projects\Celeriq\Celeriq.ManagementStudio\bin\Release\*.exe C:\Projects\PublishCeleriq\ClientTools\
copy C:\Projects\Celeriq\Celeriq.ManagementStudio\bin\Release\*.dll C:\Projects\PublishCeleriq\ClientTools\
copy C:\Projects\Celeriq\Celeriq.Profiler\bin\Release\*.exe C:\Projects\PublishCeleriq\ClientTools\
copy C:\Projects\Celeriq\Celeriq.Profiler\bin\Release\*.dll C:\Projects\PublishCeleriq\ClientTools\

rem Copy Agent 
copy C:\Projects\Celeriq\Celeriq.Agent\bin\Release\*.exe C:\Projects\PublishCeleriqAgent\
copy C:\Projects\Celeriq\Celeriq.Agent\bin\Release\*.dll C:\Projects\PublishCeleriqAgent\
copy C:\Projects\Celeriq\Build\Celeriq.Agent.exe.config C:\Projects\PublishCeleriqAgent\
copy C:\Projects\Celeriq\Celeriq.Agent\InstallService.bat C:\Projects\PublishCeleriqAgent\
copy C:\Projects\Celeriq\Celeriq.Agent\UninstallService.bat C:\Projects\PublishCeleriqAgent\

del C:\Projects\PublishCeleriq\*.vshost.exe 
del C:\Projects\PublishCeleriq\Celeriq.WinService.exe.config
del C:\Projects\PublishCeleriq\ClientTools\*.vshost.exe 
del C:\Projects\PublishCeleriqAgent\*.vshost.exe 

rem ZIP THE FOLDERS
del /q "C:\Projects\celeriq-deploy.zip"
"C:\Program Files\7-Zip\7z.exe" a "C:\Projects\celeriq-deploy.zip" "C:\Projects\PublishCeleriq\"

del /q "C:\Projects\celeriqagent-deploy.zip"
"C:\Program Files\7-Zip\7z.exe" a "C:\Projects\celeriqagent-deploy.zip" "C:\Projects\PublishCeleriqAgent\"

pause