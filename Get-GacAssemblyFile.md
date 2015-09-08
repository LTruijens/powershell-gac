# Get-GacAssemblyFile

Gets the FileInfo of assemblies in the GAC. FileInfo can be used as input for Copy-Item to
extract an assembly from the GAC or to get the VersionInfo from the assembly.

## SYNTAX
```powershell
Get-GacAssemblyFile [-AssemblyName] <AssemblyName[]> [<CommonParameters>]
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
C:\PS>Get-GacAssembly -Name System -Version 4.0.0.0 | Get-GacAssemblyFile | Copy-Item -Destination C:\Temp
```
This example extracts the System assembly with version 4.0.0.0 from the GAC to the C:\Temp path.
```powershell
C:\PS>(Get-GacAssembly -Name System | Get-GacAssemblyFile).VersionInfo

ProductVersion  FileVersion      FileName
--------------  -----------      --------
2.0.50727.6401  2.0.50727.640... C:\Windows\assembly\GACMSIL\System\2.0.0.0_b77a5c561934e089\System.dll
4.0.30319.18033 4.0.30319.180... C:\Windows\Microsoft.Net\assembly\GACMSIL\System\v4.04.0.0.0b77a5c561934e089\
S...
```
This example returns the VersionInfo all System assemblies in the GAC.
