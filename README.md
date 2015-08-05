# PowerShell GAC

The Global Assembly Cache (GAC) is a machine wide repository for .Net Assemblies. PowerShell GAC provides several PowerShell commands to view and modify the GAC.

PowerShell GAC works standalone and does not depend on tools like gacutils.exe. PowerShell GAC uses the documented native GAC API, so it does not depend on any GAC internals like changing folder structures.

## Features

Supports GAC for all .Net versions, including .Net 4.0 and .Net 4.6.

Commands:
* `Get-GacAssembly` Gets the assemblies in the GAC (alias gga)
* `Get-GacAssemblyFile` Gets the FileInfo of assemblies in the GAC
* `Get-GacAssemblyInstallReference` Gets the InstallReference of assemblies in the GAC
* `Add-GacAssembly` Adds the assembly to the GAC
* `Remove-GacAssembly` Removes the assembly from the GAC
* `New-GacAssemblyInstallReference` Creates a new install reference
* `Test-AssemblyNameFullyQualified` Determines whether the assembly name is fully qualified
* `Test-GacAssemblyInstallReferenceCanBeUsed` Determines whether the install reference can be used with Add-GacAssembly and Remove-GacAssembly

Better display for the GAC contents. Can include the file version by using the FileVersion Table View.

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
