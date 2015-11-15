$ErrorActionPreference = "STOP"
$VerbosePreference = "SilentlyContinue"

Import-Module "C:\Program Files (x86)\Microsoft SDKs\Azure\PowerShell\ResourceManager\AzureResourceManager\AzureResourceManager.psd1"

if((Get-AzureAccount) -eq $null) {
    Add-AzureAccount
}

Select-AzureSubscription -SubscriptionName "Codehouse"

$resourceGroupName = "lightcore-dev"
$deploymentName = "lightcore-cm"
$storageAccountName  = "lightcoredev"

New-AzureResourceGroup -Name $resourceGroupName -Location "westeurope" -Force
New-AzureResourceGroupDeployment -Name $deploymentName -ResourceGroupName $resourceGroupName `
    -Mode Complete `
    -TemplateFile ".\ARM-CM.json" `
    -TemplateParameterFile ".\ARM-CM.param.dev.json" `
    -storageAccountNameFromTemplate $storageAccountName `
    -Verbose `
    -Force