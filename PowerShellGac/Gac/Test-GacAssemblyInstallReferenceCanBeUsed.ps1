<#
.SYNOPSIS
    Determines whether the install reference can be used
.DESCRIPTION
    Determines whether the install reference can be used with Add-GacAssembly and Remove-GacAssembly. Only types Installer, 
    FilePath en Opaque can be used. WindowsInstaller and OsInstall not.
.PARAMETER InstallReference
    Specifies the install reference to be tested
.INPUTS
	[PowerShellGac.InstallReference[]]
.EXAMPLE
    C:\PS> Test-GacAssemblyInstallReferenceCanBeUsed (New-GacAssemblyInstallReference Opaque ([guid]::NewGuid()))
	
    True
	
    C:\PS> Test-GacAssemblyInstallReferenceCanBeUsed (New-GacAssemblyInstallReference WindowsInstaller 'MSI')
	
    False

    This example shows how you can determine if the install reference can be used.
.LINK
	Add-GacAssembly
.LINK
	Remove-GacAssembly
.LINK
	http://powershellgac.codeplex.com
#>
function Test-GacAssemblyInstallReferenceCanBeUsed
{
	[CmdletBinding()]
    [OutputType('System.Boolean')]
	param
    (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [PowerShellGac.InstallReference[]] $InstallReference
    )
	
	process
    {
		foreach ($reference in $InstallReference)
		{
			$reference.CanBeUsed()
		}
	}
}