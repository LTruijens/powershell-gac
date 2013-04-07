<#
.SYNOPSIS
    Gets the InstallReference of assemblies in the GAC
.DESCRIPTION
    Gets the InstallRefernence of assemblies in the GAC. InstallRefernence shows if an assembly
	is referenced by an installer.
.PARAMETER AssemblyName
    Specifies the assembly name. Must be fully qualified. See Test-AssemblyNameFullyQualified.
.INPUTS
	[System.Reflection.AssemblyName[]]
.EXAMPLE
    C:\PS> Get-GacAssembly -Name System | Get-GacAssemblyInstallReference
	
    Type             Identifier                                          Description
	----             ----------                                          -----------
	Opaque           {71F8EFBF-09AF-418D-91F1-52707CDFA274}              .NET Framework Redist Setup
    Opaque           {71F8EFBF-09AF-418D-91F1-52707CDFA274}              .NET Framework Redist Setup

    This example returns the InstallReferences from the System assemblies in the GAC.
.LINK
	Test-AssemblyNameFullyQualified
.LINK
	http://powershellgac.codeplex.com
#>
function Get-GacAssemblyInstallReference
{
	[CmdletBinding()]
    [OutputType('PowerShellGac.InstallReference')]
	param
    (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( { Test-AssemblyNameFullyQualified $_ } )]
        [System.Reflection.AssemblyName[]] $AssemblyName
	)

    process
    {
        foreach ($assmName in $AssemblyName)
        {
            [PowerShellGac.GlobalAssemblyCache]::GetInstallReferences($assmName)
        }
    }
}