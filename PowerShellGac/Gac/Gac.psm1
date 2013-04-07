#requires -version 2.0

Set-StrictMode -Version Latest

. $PSScriptRoot\Test-AssemblyNameFullyQualified.ps1
. $PSScriptRoot\Test-GacAssemblyInstallReferenceCanBeUsed.ps1
. $PSScriptRoot\New-GacAssemblyInstallReference.ps1
. $PSScriptRoot\Get-GacAssembly.ps1
. $PSScriptRoot\Get-GacAssemblyFile.ps1
. $PSScriptRoot\Get-GacAssemblyInstallReference.ps1
. $PSScriptRoot\Add-GacAssembly.ps1
. $PSScriptRoot\Remove-GacAssembly.ps1

Update-FormatData -PrependPath (Join-Path $PSScriptRoot 'Gac.Format.ps1xml')

Set-Alias -Name gga -Value Get-GacAssembly
Export-ModuleMember -Function Get-GacAssembly -Alias gga
Export-ModuleMember -Function Test-AssemblyNameFullyQualified, Test-GacAssemblyInstallReferenceCanBeUsed, New-GacAssemblyInstallReference, Get-GacAssemblyFile, Get-GacAssemblyInstallReference, Add-GacAssembly, Remove-GacAssembly
