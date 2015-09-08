# Test-AssemblyNameFullyQualified

Determines whether the assembly name is fully qualified. An assembly name is fully qualified if
it contains all the following parts:
* Name
* Version
* Culture
* PublicKeyToken
* ProcessorArchitecture

## SYNTAX
```powershell
Test-AssemblyNameFullyQualified [-AssemblyName] <AssemblyName[]> [<CommonParameters>]
```

## PARAMETERS
```powershell
-AssemblyName <AssemblyName[]>
```
Specifies the assembly name to be tested
```powershell
<CommonParameters>
```
This cmdlet supports the common parameters: Verbose, Debug,
ErrorAction, ErrorVariable, WarningAction, WarningVariable,
OutBuffer and OutVariable. 

## EXAMPLES
```powershell
C:\PS>Test-AssemblyNameFullyQualified 'System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, ProcessorArchitecture=MSIL'

True
C:\PS> Test-AssemblyNameFullyQualified 'System, Version=2.0.0.0'
False
```
This example shows how you can determine if the assembly name is fully qualified.
