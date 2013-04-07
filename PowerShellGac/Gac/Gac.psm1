#requires -version 2.0

Set-StrictMode -Version Latest

. .\Test-AssemblyNameFullyQualified.ps1
. .\Test-GacAssemblyInstallReferenceCanBeUsed.ps1
. .\New-GacAssemblyInstallReference.ps1
. .\Get-GacAssembly.ps1
. .\Get-GacAssemblyFile.ps1
. .\Get-GacAssemblyInstallReference.ps1
. .\Add-GacAssembly.ps1
. .\Remove-GacAssembly.ps1

Update-FormatData -PrependPath (Join-Path $PSScriptRoot 'Gac.Format.ps1xml')

Set-Alias -Name gga -Value Get-GacAssembly
Export-ModuleMember -Function Get-GacAssembly -Alias gga
Export-ModuleMember -Function Test-AssemblyNameFullyQualified, Test-GacAssemblyInstallReferenceCanBeUsed, New-GacAssemblyInstallReference, Get-GacAssemblyFile, Get-GacAssemblyInstallReference, Add-GacAssembly, Remove-GacAssembly
