<#
.SYNOPSIS
    Creates a new install reference
.DESCRIPTION
    Creates a new install reference to be used with Add-GacAssembly or Remove-GacAssembly
.PARAMETER Type
    Specifies the type of the install reference to be created
.PARAMETER Identifier
    Specifies the identifier of the install reference to be created
.PARAMETER Description
    Specifies the description of the install reference to be created
.INPUTS
	[PowerShellGac.InstallReference[]]
.EXAMPLE
    C:\PS> $reference = New-GacAssemblyInstallReference Opaque ([guid]::NewGuid()) 'Sample install reference'

    This example shows how you can create a new install reference.
.LINK
	Add-GacAssembly
.LINK
	Remove-GacAssembly
.LINK
	http://powershellgac.codeplex.com
#>
function New-GacAssemblyInstallReference
{
	[CmdletBinding()]
    [OutputType('PowerShellGac.InstallReference')]
	param
    (
        [Parameter(Position = 0, Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [PowerShellGac.InstallReferenceType] $Type,

        [Parameter(Position = 1, Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $Identifier,

        [Parameter(Position = 2)]
        [ValidateNotNullOrEmpty()]
        [string] $Description
    )
	
	process
    {
		New-Object -TypeName 'PowerShellGac.InstallReference' -ArgumentList $Type, $Identifier, $Description
	}
}