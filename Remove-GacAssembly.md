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
Specifies the InstallReference used to add the assembly to the GAC
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
