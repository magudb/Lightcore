$ErrorActionPreference = "STOP"
$VerbosePreference = "Continue"

# Settings
$name = "lightcore-demowebsite"

# Set current dir to where to docker file is
Set-Location .\..

# Build docker image
docker build -t $name .

$localIp = Get-NetAdapter -InterfaceDescription "Hyper-V Virtual Ethernet Adapter" | Get-NetIPAddress -AddressFamily IPv4 | % { $_.IPAddress }

# Stop and remove existing image
docker stop $name
docker rm $name

# Run docker image
docker run -p 5000:5000 -d --name=$name --add-host=lightcore-cm.local:$localIp --env=ASPNET_ENV=Development $name

# Wait
sleep -Seconds 5

# Open browser
docker-machine ip dockerdev | % { Start-Process "http://$($_):5000"}