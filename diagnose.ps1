Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "SQL SERVER DIAGNOSTIC UTILITY" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

Write-Host "1. Checking SQL Server Services..." -ForegroundColor Yellow
$services = Get-Service -Name "*SQL*" -ErrorAction SilentlyContinue
if ($services) {
    foreach ($svc in $services) {
        $color = if ($svc.Status -eq "Running") { "Green" } else { "Red" }
        Write-Host "Service: $($svc.Name) ($($svc.DisplayName)) -> Status: $($svc.Status)" -ForegroundColor $color
    }
} else {
    Write-Host "No SQL Server services found!" -ForegroundColor Red
}

Write-Host ""
Write-Host "2. Checking Registry for Installed Instances..." -ForegroundColor Yellow
$regPath = "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"
if (Test-Path $regPath) {
    $instances = Get-ItemProperty -Path $regPath
    $instanceNames = $instances.PSObject.Properties | Where-Object { $_.MemberType -eq "NoteProperty" -and $_.Name -notmatch "PS" }
    if ($instanceNames) {
        foreach ($inst in $instanceNames) {
            Write-Host "Found Instance Name: $($inst.Name) (Registry Value: $($inst.Value))" -ForegroundColor Green
        }
    } else {
        Write-Host "No SQL Server instances found in registry!" -ForegroundColor Red
    }
} else {
    $regPath32 = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL"
    if (Test-Path $regPath32) {
        $instances = Get-ItemProperty -Path $regPath32
        $instanceNames = $instances.PSObject.Properties | Where-Object { $_.MemberType -eq "NoteProperty" -and $_.Name -notmatch "PS" }
        foreach ($inst in $instanceNames) {
            Write-Host "Found Instance Name (32-bit): $($inst.Name) (Registry Value: $($inst.Value))" -ForegroundColor Green
        }
    } else {
        Write-Host "SQL Server Registry keys not found." -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "3. Testing Local Connections to Database 'computer_store'..." -ForegroundColor Yellow
$servers = @(
    "PNAM\SQLEXPRESS",
    ".\SQLEXPRESS",
    "(localdb)\MSSQLLocalDB",
    ".",
    "localhost"
)

foreach ($server in $servers) {
    Write-Host "Testing connection to: $server" -ForegroundColor Yellow
    $connString = "Server=$server;Database=computer_store;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=3"
    $connection = New-Object System.Data.SqlClient.SqlConnection($connString)
    try {
        $connection.Open()
        Write-Host "SUCCESS: Connected successfully to $server!" -ForegroundColor Green
        $connection.Close()
    } catch {
        Write-Host "FAILED: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "=============================================" -ForegroundColor Cyan
