# Usage instructions

* [Get-GacAssembly](Get-GacAssembly.md) Gets the assemblies in the GAC (alias gga)
* [Get-GacAssemblyFile](Get-GacAssemblyFile.md) Gets the FileInfo of assemblies in the GAC
* [Get-GacAssemblyInstallReference](Get-GacAssemblyInstallReference.md) Gets the InstallReference of assemblies in the GAC
* [Add-GacAssembly](Add-GacAssembly.md) Adds the assembly to the GAC
* [Remove-GacAssembly](Remove-GacAssembly.md) Removes the assembly from the GAC
* [New-GacAssemblyInstallReference](New-GacAssemblyInstallReference.md) Creates a new install reference
* [Test-AssemblyNameFullyQualified](Test-AssemblyNameFullyQualified.md) Determines whether the assembly name is fully qualified
* [Test-GacAssemblyInstallReferenceCanBeUsed](Test-GacAssemblyInstallReferenceCanBeUsed.md) Determines whether the install reference can be used with Add-GacAssembly and Remove-GacAssembly
* [FileVersion Table View](FileVersionTableView.md) Adds column showing the FileVersion

# Alternatives

There are some alternatives to PowerShell GAC to view are modify the contents of the GAC. PowerShell GAC has the following advantages compare to some of these alternatives:
* Supports all .Net versions, including .Net 4.0 en .Net 4.5, even from PowerShell 2.0
* No other tools needed
* Does not depend on changing internals, but uses the documented GAC API
* Full range of features of the GAC accessible
* Extraction of assemblies from the GAC possible
* Listing file versions of the assemblies in the GAC
* Great integration with other PowerShell commands and scripts

## System.EnterpriseServices.Internal.Publish

The .Net Framework comes contains two simples method to add and remove assemblies from the GAC. There are no methods to view the contents of the GAC. These methods do not return any error codes nor exceptions, so there is no feedback on the results. Also in order to remove an assembly from the GAC you need specify the path to an assembly with the same identity as the assembly in the GAC. Administrative rights are required.

The methods [GacInstall](https://msdn.microsoft.com/en-us/library/system.enterpriseservices.internal.publish.gacinstall.aspx) and [GacRemove](https://msdn.microsoft.com/en-us/library/system.enterpriseservices.internal.publish.gacremove.aspx) can be found in the `System.EnterpriseServices.Internal.Publish` class. This class can be found in the `System.EnterpriseServices` assembly. 

The .Net 4.0 version of the `System.EnterpriseServices` assembly is needed to add or remove .Net 4.0 or .Net 4.5 assemblies. Since PowerShell 2.0 is not able to load .Net 4.0 assemblies it is needed to [run PowerShell 2.0 using .Net 4.0](https://stackoverflow.com/questions/2094694/how-can-i-run-powershell-with-the-net-4-runtime) or use PowerShell 3.0.

The following PowerShell script can be used to add and remove assemblies from the GAC.
```powershell
Add-Type -AssemblyName System.EnterpriseServices
$publish = New-Object System.EnterpriseServices.Internal.Publish
$publish.GacInstall('c:\folder\some.dll')

Add-Type -AssemblyName System.EnterpriseServices
$publish = New-Object System.EnterpriseServices.Internal.Publish
$publish.GacRemove('c:\folder\some.dll')
```

## Gacutil.exe

Gacutil is a command line tool to view and modify the GAC. It has almost the same features as PowerShell GAC. This tool comes with Visual Studio, .Net Framework SDK and Windows SDK. The .Net 4.0 version of this tool is needed to install .Net 4.0 or 4.5 assemblies. Administrative rights are needed for adding and removing assemblies from the GAC. For more information see [here](https://msdn.microsoft.com/en-us/library/ex0ss12c(v=vs.110).aspx).

# Build instructions for developers

Uses the [GAC](https://support.microsoft.com/kb/317540) or [Fusion](https://msdn.microsoft.com/en-us/library/ms404523.aspx) API to do its magic.

Use VisualStudio 2010 or 2012 to open the solution or build from the commandline with `MSBuild.exe PowerShellGac.csproj`. No need for VisualStudio.
