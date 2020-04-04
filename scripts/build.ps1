$path = 'build'
$cl = Get-Location
$env:NODE_ENV = 'production'

if (Test-Path $path) {
    Get-ChildItem -Path $path -Include OmegaGraf* -File -Recurse | ForEach-Object { $_.Delete() }
}
else {
    New-Item $path -ItemType Directory
}

Set-Location ui
npm ci
npm run build
Set-Location $cl

dotnet build compose/compose.csproj /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary

$v = 'v' + (Get-Content 'VERSION')

##########################################

$os = 'win'
$osr = "$os-x64"
dotnet publish compose/compose.csproj -c=Release -r="$osr" /property:GenerateFullPaths=true /property:PublishSingleFile=true /property:PublishTrimmed=true /consoleloggerparameters:NoSummary

$path = 'build/win'
if (!(Test-Path $path)) {
    New-Item $path -ItemType Directory
}
Copy-Item -Path 'compose/bin/Release/netcoreapp3.1/win-x64/publish/*' -Destination "build/win/" -Recurse
Move-Item -Path 'build/win/compose.exe' -Destination "build/win/OmegaGraf.exe"

Set-Location $path
Compress-Archive -Path "OmegaGraf.exe" -DestinationPath "OmegaGraf-$osr-$v.zip"
Set-Location $cl

##########################################

$os = 'linux'
$osr = "$os-x64"
dotnet publish compose/compose.csproj -c=Release -r="$osr" /property:GenerateFullPaths=true /property:PublishSingleFile=true /property:PublishTrimmed=true /consoleloggerparameters:NoSummary

$path = 'build/lnx'
if (!(Test-Path $path)) {
    New-Item $path -ItemType Directory
}
Copy-Item -Path 'compose/bin/Release/netcoreapp3.1/linux-x64/publish/*' -Destination "build/lnx/" -Recurse
Move-Item -Path 'build/lnx/compose' -Destination "build/lnx/OmegaGraf"

Set-Location $path
tar -cvzf "OmegaGraf-$osr-$v.tar.gz" "OmegaGraf"
Set-Location $cl