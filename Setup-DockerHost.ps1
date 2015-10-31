$ErrorActionPreference = "STOP"
$VerbosePreference = "Continue"

$machineName = "dockerdev"
$found = if((docker-machine ls | Select-String $machineName)) { $true }

if(!$found)
{
    Write-Warning "Machine '$machineName' not found, creating..."

    docker-machine create -d hyper-v $machineName -debug
}
else
{
    Write-Verbose "Host '$machineName' running..."
}

Write-Verbose "Setting up enviroment..."

docker-machine.exe env $machineName --shell powershell
docker-machine.exe env --shell=powershell $machineName | Invoke-Expression

Write-Verbose "Running hello-world"

docker run hello-world