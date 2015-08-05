<#
.SYNOPSIS
    Adds the assembly to the GAC
.DESCRIPTION
    Adds the assembly to the GAC. An assembly is only added to the GAC if the assembly is not already added to
	the GAC or if the file version is higher than the assembly already in the GAC. Can only be called from an 
	elevated prompt.

	 It must be a strong named/signed assembly (PublicKeyToken must be set).
.PARAMETER Path
    Specifies the path to the assembly. Wildcards are permitted.
.PARAMETER LiteralPath
    Specifies the path to the assembly. Unlike Path, the value of the LiteralPath parameter is used exactly as it is typed. No characters are interpreted as wildcards.
.PARAMETER InstallReference
	Specifies the InstallReference to add the assembly to the GAC
.PARAMETER Force
	Force the addition to the GAC even if the file version of the assembly already in the GAC is higher
.PARAMETER PassThru
	The AssemblyName added is returned as output
.INPUTS
	[string]
.EXAMPLE
    C:\PS> Add-GacAssembly .\SomeAssembly.dll

    This example adds the SomeAssembly.dll assembly to the GAC.
.LINK
	http://powershellgac.codeplex.com
#>
function Add-GacAssembly
{
	[CmdletBinding(SupportsShouldProcess = $true, DefaultParameterSetName = 'PathSet')]
	[OutputType('System.Reflection.AssemblyName')]
	param
    (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'PathSet')]
        [ValidateNotNullOrEmpty()]
        [string[]] $Path,

        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'LiteralPathSet')]
        [ValidateNotNullOrEmpty()]
        [Alias('PSPath')]
        [string[]] $LiteralPath,

        [Parameter(Position = 1)]
		[ValidateNotNullOrEmpty()]
        [ValidateScript( { Test-GacAssemblyInstallReferenceCanBeUsed $_ } )]
        [PowerShellGac.InstallReference] $InstallReference,

        [Switch] $Force,

        [Switch] $PassThru
	)

    process
    {
        if ($PsCmdlet.ParameterSetName -eq 'PathSet')
        {
            $paths = @()
            foreach ($p in $Path)
            {
                $paths += (Resolve-Path $p).ProviderPath
            }    
        }
        else
        {
            $paths = (Resolve-Path -LiteralPath $LiteralPath).ProviderPath
        }

        foreach ($p in $paths)
        {
            if (!$PSCmdLet.ShouldProcess($p))
            {
                continue
            }

            [PowerShellGac.GlobalAssemblyCache]::InstallAssembly($p, $InstallReference, $Force)
            Write-Verbose "Installed $p into the GAC"

            if ($PassThru)
            {
                [System.Reflection.AssemblyName]::GetAssemblyName($p)
            }
        }    
    }
}