<#
.SYNOPSIS
    Determines whether the assembly name is fully qualified
.DESCRIPTION
    Determines whether the assembly name is fully qualified. An assembly name is fully qualified if
	it contains all the following parts:
	* Name
	* Version
	* Culture
	* PublicKeyToken
	* ProcessorArchitecture

    Note that the ProcessorArchitecture always has a valid default value of None. Test-AssemblyNameFullyQualified 
	also returns True if only the ProcessorArchitecture is not specified.
.PARAMETER AssemblyName
    Specifies the assembly name to be tested
.INPUTS
	[System.Reflection.AssemblyName[]]
.EXAMPLE
    C:\PS> Test-AssemblyNameFullyQualified 'System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, ProcessorArchitecture=MSIL'
	
    True
	
    C:\PS> Test-AssemblyNameFullyQualified 'System, Version=2.0.0.0'
	
    False

    This example shows how you can determine if the assembly name is fully qualified.
.LINK
	http://powershellgac.codeplex.com
#>
function Test-AssemblyNameFullyQualified
{
	[CmdletBinding()]
    [OutputType('System.Boolean')]
	param
    (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [System.Reflection.AssemblyName[]] $AssemblyName
    )
	
	process
    {
		foreach ($assmName in $AssemblyName)
		{
			[PowerShellGac.GlobalAssemblyCache]::IsFullyQualifiedAssemblyName($assmName)
		}
	}
}