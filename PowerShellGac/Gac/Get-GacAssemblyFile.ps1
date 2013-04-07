<#
.SYNOPSIS
    Gets the FileInfo of assemblies in the GAC
.DESCRIPTION
    Gets the FileInfo of assemblies in the GAC. FileInfo can be used as input for Copy-Item to 
	extract an assembly from the GAC or to get the VersionInfo from the assembly.
.PARAMETER AssemblyName
    Specifies the assembly name. Must be fully qualified. See Test-AssemblyNameFullyQualified.
.INPUTS
	[System.Reflection.AssemblyName[]]
.EXAMPLE
    C:\PS> Get-GacAssembly -Name System -Version 4.0.0.0 | Get-GacAssemblyFile | Copy-Item -Destination C:\Temp

    This example extracts the System assembly with version 4.0.0.0 from the GAC to the C:\Temp path.
.EXAMPLE
    C:\PS> (Get-GacAssembly -Name System | Get-GacAssemblyFile).VersionInfo

    ProductVersion   FileVersion      FileName
    --------------   -----------      --------
    2.0.50727.6401   2.0.50727.640... C:\Windows\assembly\GAC_MSIL\System\2.0.0.0__b77a5c561934e089\System.dll
	4.0.30319.18033  4.0.30319.180... C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System\v4.0_4.0.0.0__b77a5c561934e089\S...

    This example returns the VersionInfo all System assemblies in the GAC.
.LINK
	Test-AssemblyNameFullyQualified
.LINK
	http://powershellgac.codeplex.com
#>
function Get-GacAssemblyFile
{
	[CmdletBinding()]
    [OutputType('System.IO.FileInfo')]
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
            $path = [PowerShellGac.GlobalAssemblyCache]::GetAssemblyPath($assmName)
            [System.IO.FileInfo] $path
        }
    }
}