# New-GacAssemblyInstallReference

Creates a new install reference to be used with Add-GacAssembly or Remove-GacAssembly

## SYNTAX
```powershell
New-GacAssemblyInstallReference [-Type] {WindowsInstaller | Installer | FilePath | Opaque | OsInstall} [-Identifier] <String> [[-Description] <String>] [<CommonParameters>]
```

## PARAMETERS
```powershell
-Type
```
Specifies the type of the install reference to be created
```powershell
-Identifier <String>
```
Specifies the identifier of the install reference to be created
```powershell
-Description <String>
```
Specifies the description of the install reference to be created
```powershell
<CommonParameters>
```
This cmdlet supports the common parameters: Verbose, Debug,
ErrorAction, ErrorVariable, WarningAction, WarningVariable,
OutBuffer and OutVariable. 

## EXAMPLES
```powershell
C:\PS>$reference = New-GacAssemblyInstallReference Opaque (guid::NewGuid()) 'Sample install reference'
```
This example shows how you can create a new install reference.
