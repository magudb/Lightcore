$ErrorActionPreference = "STOP"
$VerbosePreference = "Continue"

# Build code
dnvm use -version 1.0.0-beta8 -runtime coreclr
dnu restore .\src\Lightcore
dnu build .\src\Lightcore
dnu restore .\src\DemoWebsite
dnu build .\src\DemoWebsite

# Publish code
dnu publish .\src\DemoWebsite --configuration Release --out .\artifacts\app --runtime dnx-coreclr-win-x86.1.0.0-beta8

# Build docker image
docker build -t lightcore .

$localIp = Get-NetAdapter -InterfaceDescription "Hyper-V Virtual Ethernet Adapter" | Get-NetIPAddress -AddressFamily IPv4 | % { $_.IPAddress }

# Run docker image
docker run -p 5000:5000 -d --name=lightcore --add-host=lightcore-cm.local:$localIp --env=ASPNET_ENV=Development lightcore

# Wait
sleep -Seconds 5

# Open browser
docker-machine ip dockerdev | % { Start-Process "http://$($_):5000"}