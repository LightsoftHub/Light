echo "Begin build projects..."
dotnet build ActiveDirectory/ActiveDirectory.csproj --configuration Release --no-restore
dotnet build Identity/Identity.csproj --configuration Release --no-restore
dotnet build Identity.EntityFrameworkCore/Identity.EntityFrameworkCore.csproj --configuration Release --no-restore

echo "Begin pack projects..."
dotnet pack --no-build ActiveDirectory/ActiveDirectory.csproj --configuration Release --output D:\Publish\LightFramework
dotnet pack --no-build Identity/Identity.csproj --configuration Release --output D:\Publish\LightFramework
dotnet pack --no-build Identity.EntityFrameworkCore/Identity.EntityFrameworkCore.csproj --configuration Release --output D:\Publish\LightFramework

:: hold for view messages
pause