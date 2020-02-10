$os = 'win'
$osr = "$os-x64"

Set-Location ui
npm install
npm run build
Set-Location ..
dotnet build compose/compose.csproj /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
dotnet publish compose/compose.csproj -c=Release -r="$osr" /property:GenerateFullPaths=true /property:PublishSingleFile=true /property:PublishTrimmed=true /consoleloggerparameters:NoSummary
