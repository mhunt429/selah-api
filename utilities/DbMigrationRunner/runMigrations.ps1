# Check for local_settings.ps1 file with personal overrides and import them
if (Test-Path "./env.ps1") {
    Write-Output "Triggering local settings import"
    . ./env.ps1 2>&1
}

dotnet restore
dotnet run --project ./
