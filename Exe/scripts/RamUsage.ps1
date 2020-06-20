$os = Get-Ciminstance Win32_OperatingSystem
$pctFree = 100 - [math]::Round(($os.FreePhysicalMemory/$os.TotalVisibleMemorySize)*100,2)
Write-Host $pctFree;