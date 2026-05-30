$servers = @(
    "PNAM\SQLEXPRESS",
    ".\SQLEXPRESS",
    "(localdb)\MSSQLLocalDB",
    ".",
    "localhost"
)

$database = "computer_store"
$success = $false

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "AUTOMATIC SQL SERVER CONNECTION & SCAFFOLDING" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

# Ensure dotnet-ef tool is installed/updated
Write-Host "Checking/Installing dotnet-ef tool..." -ForegroundColor Cyan
dotnet tool install --global dotnet-ef 2>$null
dotnet tool update --global dotnet-ef 2>$null

foreach ($server in $servers) {
    Write-Host ""
    Write-Host ">>> Trying server: $server ..." -ForegroundColor Yellow
    $connString = "Server=$server;Database=$database;Trusted_Connection=True;TrustServerCertificate=True;"
    
    # Run the scaffold command
    dotnet ef dbcontext scaffold $connString Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Data --data-annotations --force
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Scaffold database successfully using Server: $server!" -ForegroundColor Green
        $success = $true
        
        # Save connection string to appsettings.json
        Write-Host "Updating appsettings.json with connection string..." -ForegroundColor Cyan
        $appsettingsPath = Join-Path $PSScriptRoot "appsettings.json"
        if (Test-Path $appsettingsPath) {
            $json = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
            
            # Create ConnectionStrings object if it doesn't exist
            if (-not ($json | Get-Member -Name "ConnectionStrings")) {
                $json | Add-Member -MemberType NoteProperty -Name "ConnectionStrings" -Value (New-Object PSObject)
            }
            
            # Set default connection
            if ($json.ConnectionStrings | Get-Member -Name "DefaultConnection") {
                $json.ConnectionStrings.DefaultConnection = $connString
            } else {
                $json.ConnectionStrings | Add-Member -MemberType NoteProperty -Name "DefaultConnection" -Value $connString
            }
            
            $json | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
            Write-Host "appsettings.json updated successfully!" -ForegroundColor Green
        }
        break
    } else {
        Write-Host "Failed to connect/scaffold using server: $server." -ForegroundColor DarkYellow
    }
}

Write-Host ""
if ($success) {
    Write-Host "SUCCESS: Database scaffolded and configured!" -ForegroundColor Green
} else {
    Write-Host "ERROR: Could not connect to any local SQL Server instance." -ForegroundColor Red
    Write-Host "Please check if your SQL Server is running and the database '$database' exists." -ForegroundColor Red
}
Write-Host "=============================================" -ForegroundColor Cyan
