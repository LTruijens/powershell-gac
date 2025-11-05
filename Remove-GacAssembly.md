# Remove-GacAssembly

Removes the assembly from the GAC. Can only be called from an elevated prompt.

## SYNTAX
```powershell
Remove-GacAssembly [-AssemblyName] <AssemblyName[]> [[-InstallReference] <InstallReference>] [-PassThru] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## PARAMETERS
```powershell
-AssemblyName <AssemblyName[]>
```
Specifies the assembly name. Must be fully qualified. See Test-AssemblyNameFullyQualified.
```powershell
-InstallReference <InstallReference>
```
Specifies the InstallReference to remove from the assembly in the GAC. When an InstallReference is specified, only that reference is removed. The assembly is only removed from the GAC if no other install references exist.
```powershell
-PassThru [<SwitchParameter>]
```
The AssemblyName removed is used as the output
```powershell
-WhatIf [<SwitchParameter>]
```
```powershell
-Confirm [<SwitchParameter>]
```
```powershell
<CommonParameters>
```
This cmdlet supports the common parameters: Verbose, Debug,
ErrorAction, ErrorVariable, WarningAction, WarningVariable,
OutBuffer and OutVariable. 

## EXAMPLES
```powershell
C:\PS>Get-GacAssembly -Name SomeAssembly | Remove-GacAssembly
```
This example removes all assemblies with the name SomeAssembly from the GAC.

```powershell
C:\PS>$ref = New-GacAssemblyInstallReference -Type Opaque -Id "MyApp" -Description "My Application"
C:\PS>Get-GacAssembly -Name SomeAssembly | Remove-GacAssembly -InstallReference $ref
```
This example removes the specific install reference from the assembly. If other install references exist, the assembly remains in the GAC.
