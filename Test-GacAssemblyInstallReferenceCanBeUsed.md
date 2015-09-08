# Test-GacAssemblyInstallReferenceCanBeUsed

Determines whether the install reference can be used with Add-GacAssembly and Remove-GacAssembly. Only types Installer, FilePath en Opaque can be used. WindowsInstaller and OsInstall not.

## SYNTAX
```powershell
Test-GacAssemblyInstallReferenceCanBeUsed [-InstallReference] <InstallReference[]> [<CommonParameters>]
```

## PARAMETERS
```powershell
-InstallReference <InstallReference[]>
```
Specifies the install reference to be tested
```powershell
<CommonParameters>
```
This cmdlet supports the common parameters: Verbose, Debug,
ErrorAction, ErrorVariable, WarningAction, WarningVariable,
OutBuffer and OutVariable. 

## EXAMPLES
```powershell
C:\PS>Test-GacAssemblyInstallReferenceCanBeUsed (New-GacAssemblyInstallReference Opaque ([guid]::NewGuid()))

True

C:\PS> Test-GacAssemblyInstallReferenceCanBeUsed (New-GacAssemblyInstallReference WindowsInstaller 'MSI')

False
```
This example shows how you can determine if the install reference can be used.
