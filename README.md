# PowerShell GAC

The Global Assembly Cache (GAC) is a machine wide repository for .Net Assemblies. PowerShell GAC provides several PowerShell commands to view and modify the GAC.

PowerShell GAC works standalone and does not depend on tools like gacutils.exe. PowerShell GAC uses the documented native GAC API, so it does not depend on any GAC internals like changing folder structures.

## Features

Supports GAC for all .Net versions, including .Net 4.0 and .Net 4.6.

Commands:
* [Get-GacAssembly](Get-GacAssembly.md) Gets the assemblies in the GAC (alias gga)
* [Get-GacAssemblyFile](Get-GacAssemblyFile.md) Gets the FileInfo of assemblies in the GAC
* [Get-GacAssemblyInstallReference](Get-GacAssemblyInstallReference.md) Gets the InstallReference of assemblies in the GAC
* [Add-GacAssembly](Add-GacAssembly.md) Adds the assembly to the GAC
* [Remove-GacAssembly](Remove-GacAssembly.md) Removes the assembly from the GAC
* [New-GacAssemblyInstallReference](New-GacAssemblyInstallReference.md) Creates a new install reference
* [Test-AssemblyNameFullyQualified](Test-AssemblyNameFullyQualified.md) Determines whether the assembly name is fully qualified
* [Test-GacAssemblyInstallReferenceCanBeUsed](Test-GacAssemblyInstallReferenceCanBeUsed.md) Determines whether the install reference can be used with Add-GacAssembly and Remove-GacAssembly
* [FileVersion Table View](FileVersionTableView.md) Adds column showing the FileVersion

## Examples

```powershell
# Show the assemblies in the GAC, including the file version
Get-GacAssembly SomeCompany* | Format-Table -View FileVersion

# Backup the assemblies in C:\Temp\SomeCompany
Get-GacAssembly SomeCompany* | Get-GacAssemblyFile | Copy-Item -Destination C:\Temp\SomeCompany

# Remove the assemblies from the GAC
# Can only be run from an elevated prompt
Get-GacAssembly SomeCompany* | Remove-GacAssembly

# Add the assemblies back to the GAC
# Can only be run from an elevated prompt
Add-GacAssembly C:\Temp\SomeCompany\*.dll
```

## Requirements

PowerShell 2.0 or higher

## Installation

Installation instructions can be found [here](INSTALL.md).

## Documentation

Documentation can be found [here](DOCUMENTATION.md).
