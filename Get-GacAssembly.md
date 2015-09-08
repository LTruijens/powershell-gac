# Get-GacAssembly

Gets the assemblies in the GAC. Assemblies can be filterd by Name, Version, Culture, PublicKeyToken or ProcessArchitecture.

## SYNTAX
```powershell
Get-GacAssembly [[-Name] <String[]>] [[-Version] <String[]>] [[-Culture] <String[]>] [[-PublicKeyToken] <String[]>] [[-ProcessorArchitecture] <ProcessorArchitecture[]>] [<CommonParameters>]

Get-GacAssembly [-AssemblyName] <AssemblyName[]> [<CommonParameters>]
````

## PARAMETERS
```powershell
-Name <String[]>
```
Filter on the Name part. Supports wildcards.
```powershell
-Version <String[]>
```
Filter on the Version part. Supports wildcards.
```powershell
-Culture <String[]>
```
Filter on the Culture part. 'neutral' or '' can be used to filter on assemblies without cultures. Supports wildcards.
```powershell
-PublicKeyToken <String[]>
```
Filter on the PublicKeyToken part. Supports wildcards.
```powershell
-ProcessorArchitecture <ProcessorArchitecture[]>
```
Filter on the ProcessorArchitecture part. Supports wildcards.
```powershell
-AssemblyName <AssemblyName[]>
```
Filter on AssemblyName
```powershell
<CommonParameters>
```
This cmdlet supports the common parameters: Verbose, Debug,
ErrorAction, ErrorVariable, WarningAction, WarningVariable,
OutBuffer and OutVariable.

## EXAMPLES
```powershell
C:\PS>Get-GacAssembly
```
This example returns all assemblies in the GAC
```powershell
C:\PS>Get-GacAssembly -Name System* -Version 2.0.0.0
```
This example returns all assemblies in the GAC with a name starting with System and version 2.0.0.0.
