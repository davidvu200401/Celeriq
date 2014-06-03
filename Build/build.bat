rem Create temp folder
rmdir /s /q C:\Projects\PublishCeleriq\
mkdir C:\Projects\PublishCeleriq\
mkdir C:\Projects\PublishCeleriq\ClientTools\

rmdir /s /q C:\Projects\PublishCeleriqAgent\
mkdir C:\Projects\PublishCeleriqAgent\

rem FULL BUILD
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "C:\Projects\CeleriqGit\Celeriq.InMemory.sln" /t:Rebuild /p:Configuration=Release

rem Copy files to temp location
copy C:\Projects\CeleriqGit\Celeriq.WinService\bin\Release\*.exe C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\Celeriq.WinService\bin\Release\*.dll C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\Build\Celeriq.WinService.exe.config C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\Celeriq.WinService\InstallService.bat C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\Celeriq.WinService\UninstallService.bat C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\ThirdParty\System.Data.SQLite.dll C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\ThirdParty\AntiXSSLibrary.dll C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\Celeriq.WinService\Celeriq.WinService.exe.Template.config C:\Projects\PublishCeleriq\
copy C:\Projects\CeleriqGit\Docs\ReadMe.txt C:\Projects\PublishCeleriq\

rem Copy Tools
copy C:\Projects\CeleriqGit\Celeriq.ManagementStudio\bin\Release\*.exe C:\Projects\PublishCeleriq\ClientTools\
copy C:\Projects\CeleriqGit\Celeriq.ManagementStudio\bin\Release\*.dll C:\Projects\PublishCeleriq\ClientTools\
copy C:\Projects\CeleriqGit\Celeriq.Profiler\bin\Release\*.exe C:\Projects\PublishCeleriq\ClientTools\
copy C:\Projects\CeleriqGit\Celeriq.Profiler\bin\Release\*.dll C:\Projects\PublishCeleriq\ClientTools\

rem Copy Agent 
copy C:\Projects\CeleriqGit\Celeriq.Agent\bin\Release\*.exe C:\Projects\PublishCeleriqAgent\
copy C:\Projects\CeleriqGit\Celeriq.Agent\bin\Release\*.dll C:\Projects\PublishCeleriqAgent\
copy C:\Projects\CeleriqGit\Build\Celeriq.Agent.exe.config C:\Projects\PublishCeleriqAgent\
copy C:\Projects\CeleriqGit\Celeriq.Agent\InstallService.bat C:\Projects\PublishCeleriqAgent\
copy C:\Projects\CeleriqGit\Celeriq.Agent\UninstallService.bat C:\Projects\PublishCeleriqAgent\

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