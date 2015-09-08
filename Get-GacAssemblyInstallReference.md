# Get-GacAssemblyInstallReference

Gets the InstallRefernence of assemblies in the GAC. InstallRefernence shows if an assembly is referenced by an installer.

## SYNTAX
```powershell
Get-GacAssemblyInstallReference [-AssemblyName] <AssemblyName[]> [<CommonParameters>]
```

## PARAMETERS
```powershell
-AssemblyName <AssemblyName[]>
```
Specifies the assembly name. Must be fully qualified. See Test-AssemblyNameFullyQualified.
```powershell
<CommonParameters>
```
This cmdlet supports the common parameters: Verbose, Debug,
ErrorAction, ErrorVariable, WarningAction, WarningVariable,
OutBuffer and OutVariable. 

## EXAMPLES
```powershell
C:\PS>Get-GacAssembly -Name System | Get-GacAssemblyInstallReference

Type Identifier Description
---- ---------- -----------
Opaque {71F8EFBF-09AF-418D-91F1-52707CDFA274} .NET Framework Redist Setup
Opaque {71F8EFBF-09AF-418D-91F1-52707CDFA274} .NET Framework Redist Setup
```
This example returns the InstallReferences from the System assemblies in the GAC.
