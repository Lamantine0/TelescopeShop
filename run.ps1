# Запуск TelescopeShop: остановка старого процесса → сборка → запуск из корня проекта.
# Использование: .\run.ps1 [-Urls http://127.0.0.1:5199]
param(
    [string]$Urls
)

$ErrorActionPreference = "Stop"
Set-Location -LiteralPath $PSScriptRoot

Stop-Process -Name "TelescopeShop" -Force -ErrorAction SilentlyContinue

dotnet build --nologo -v q
if ($LASTEXITCODE -ne 0) {
    Write-Host "Сборка провалена." -ForegroundColor Red
    exit $LASTEXITCODE
}

$runArgs = @()
if ($Urls) { $runArgs += @("--urls", $Urls) }

Write-Host "Запуск TelescopeShop..." -ForegroundColor Green
& dotnet run --no-build -- @runArgs
