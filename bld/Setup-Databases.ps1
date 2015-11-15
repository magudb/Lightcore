$VerbosePreference = "SilentlyContinue"
$ErrorActionPreference = "STOP"
$sqlPackageExe = "C:\Program Files (x86)\Microsoft SQL Server\120\DAC\bin\SqlPackage.exe"
$databases ="Lightcore_Core", "Lightcore_Master", "Lightcore_Web"
$paramsFile = "$PSCommandPath.param.json"

if(!(Test-Path $paramsFile)) 
{
    throw "Parameter file not found at '$paramsFile'"
}

$params = Get-Content -Path $paramsFile | Out-String | ConvertFrom-Json
$params

$databases | % {
    $file = Join-Path $params.tempDir "$_.bacpac"

    if(!(Test-Path $file)) 
    {
        & $sqlPackageExe /Action:Export /SourceConnectionString:"data source=(local);initial catalog=$_;trusted_connection=true;" /TargetFile:"$file"
    }
    else
    {
        Write-Warning "BACPAC at '$file' exists, skipping... delete first to generate a new one."
    }
}

Import-Module "C:\Program Files (x86)\Microsoft SDKs\Azure\PowerShell\ResourceManager\AzureResourceManager\AzureResourceManager.psd1"

if((Get-AzureAccount) -eq $null) {
    Add-AzureAccount
}

Select-AzureSubscription -SubscriptionName "Codehouse"

$storageAccount = "lightcoredev"
$storageContainerName = "databases"
$storageContext = New-AzureStorageContext -StorageAccountName $storageAccount -StorageAccountKey $params.storageKey

if(!(Get-AzureStorageContainer -Name $storageContainerName -Context $storageContext -ErrorAction SilentlyContinue))
{
    New-AzureStorageContainer -Name $storageContainerName -Context $storageContext
}

$databases | % {
    $file = Get-Item (Join-Path $params.tempDir "$_.bacpac")
        
    Set-AzureStorageBlobContent -File $file.FullName -Container $storageContainerName -Blob $file.Name -Context $storageContext -Force
}

$sqlServerName = "lightcore-dev-sql-cm"
$sqlServerContext = New-AzureSqlDatabaseServerContext -ServerName $sqlServerName -Credential (Get-Credential -Message "Enter username/password for $sqlServerName")
$storageContainer = Get-AzureStorageContainer -Name $storageContainerName -Context $storageContext

$databases | % {
    $file = Get-Item (Join-Path $params.tempDir "$_.bacpac")

    Start-AzureSqlDatabaseImport -SqlConnectionContext $sqlServerContext -StorageContainer $storageContainer -DatabaseName $file.BaseName -BlobName $file.Name
}