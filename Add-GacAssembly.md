# Add-GacAssembly

Adds the assembly to the GAC. An assembly is only added to the GAC if the assembly is not already added to the GAC or if the file version is higher than the assembly already in the GAC. Can only be called from an elevated prompt.

It must be a strong named/signed assembly (PublicKeyToken must be set).

## SYNTAX
```powershell
Add-GacAssembly [-Path] <String[]> [[-InstallReference] <InstallReference>] [-Force] [-PassThru] [-WhatIf] [-Confirm] [<CommonParameters>]

Add-GacAssembly [-LiteralPath] <String[]> [[-InstallReference] <InstallReference>] [-Force] [-PassThru] [-WhatIf] [
-Confirm] [<CommonParameters>]
```

## PARAMETERS
```powershell
-Path <String[]>
```
Specifies the path to the assembly. Wildcards are permitted. 
```powershell
-LiteralPath <String[]>
```
Specifies the path to the assembly. Unlike Path, the value of the LiteralPath parameter is used exactly as it is typed. No characters are interpreted as wildcards.
```powershell
-InstallReference <InstallReference>
```
Specifies the InstallReference to add the assembly to the GAC
```powershell
-Force [<SwitchParameter>]
```
Force the addition to the GAC even if the file version of the assembly already in the GAC is higher
```powershell
-PassThru [<SwitchParameter>]
```
The AssemblyName added is returned as output
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
C:\PS>Add-GacAssembly .\SomeAssembly.dll
```
This example adds the SomeAssembly.dll assembly to the GAC.
