$ErrorActionPreference = "Stop"

$projectPath = "./AscendTV.LethalCompany.ScrapLossBalancer/AscendTV.LethalCompany.ScrapLossBalancer.csproj"
$publishDir = "./publish/"
$pluginDir = "./publish/BepInEx/plugins/"
$dllName = "AscendTV.LethalCompany.ScrapLossBalancer.dll"
$dllPath = "./AscendTV.LethalCompany.ScrapLossBalancer/bin/Release/netstandard2.1/$dllName"
$filesToCopy = @("icon.png", "README.md", "CHANGELOG.md")

# 1. Manage publish directory
if (Test-Path $publishDir) {
    Remove-Item -Path $publishDir -Recurse
}
New-Item -ItemType Directory -Path $pluginDir

# 2. Build the project
dotnet build $projectPath --configuration Release --no-restore

# 3. Copy DLL
Copy-Item $dllPath $pluginDir

# 4. Copy additional files
$filesToCopy | ForEach-Object {
    Copy-Item $_ $publishDir
}

# 5. Create manifest.json
try {
    Add-Type -Path $dllPath
    $pluginInfo = [AscendTV.LethalCompany.ScrapLossBalancer.PluginInfo]

    $manifestContent = [PSCustomObject]@{
        name = $pluginInfo::ShortName
        description = $pluginInfo::Description
        version_number = $pluginInfo::ShortVersion
        author = "ascendedguard"
        website_url = "https://github.com/ascendedguard/LethalCompany.ScrapLossBalancer"
        dependencies = @("BepInEx-BepInExPack-5.4.2100")
    } | ConvertTo-Json -Depth 100

    $manifestContent | Out-File -FilePath "$publishDir/manifest.json"
} finally {
    # Remove the added types to release the DLL
    [System.AppDomain]::CurrentDomain.GetAssemblies() |
        Where-Object { $_.Location -eq $dllPath } |
        ForEach-Object { [System.AppDomain]::CurrentDomain.Unload([System.Runtime.Loader.AssemblyLoadContext]::GetLoadContext($_)) }
}

# 6. Zip the files
$version = $pluginInfo::ShortVersion
$zipName = "AscendTV.LethalCompany.ScrapLossBalancer-$version.zip"
$zipPath = Join-Path $publishDir $zipName

# Create the zip archive
Compress-Archive -Path "$publishDir*" -DestinationPath $zipPath

# Remove the original files
$filesToDelete = @("icon.png", "README.md", "CHANGELOG.md", "manifest.json", "BepInEx/plugins/$dllName", "BepInEx/plugins", "BepInEx")
$filesToDelete | ForEach-Object {
    $filePath = Join-Path $publishDir $_

    if (Test-Path $filePath) {
        Remove-Item -Path $filePath
    }
}

$finalZipLocation = $zipPath | Resolve-Path
Write-Output ""
Write-Output "Published file: $finalZipLocation"