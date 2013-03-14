#requires -version 2.0

<#
.SYNOPSIS
    Determines whether the assembly name if fully qualified
.DESCRIPTION
    Determines whether the assembly name if fully qualified. An assembly name is fully qualified if
	it contains all the following parts:
	* Name
	* Version
	* Culture
	* PublicKeyToken
	* ProcessorArchitecture
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
.NOTES
	Note that the ProcessorArchitecture always has a valid default value of None. Test-AssemblyNameFullyQualified 
	returns True if only the ProcessorArchitecture is not specified.
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

    foreach ($assmName in $AssemblyName)
    {
	    [PowerShellGac.GlobalAssemblyCache]::IsFullyQualifiedAssemblyName($assmName)
    }
}

<#
.SYNOPSIS
    Gets the assemblies in the GAC
.DESCRIPTION
    Gets the assemblies in the GAC. Assemblies can be filterd by Name, Version, Culture, PublicKeyToken or ProcessArchitecture.
.PARAMETER Name
    Filter on the Name part. Supports wildcards.
.PARAMETER Version
    Filter on the Version part. Supports wildcards.
.PARAMETER Culture
    Filter on the Culture part. 'neutral' or '' can be used to filter on assemblies without cultures. Supports wildcards.
.PARAMETER PublicKeyToken
    Filter on the PublicKeyToken part. Supports wildcards.	
.PARAMETER ProcessorArchitecture
    Filter on the ProcessorArchitecture part. Supports wildcards.	
.PARAMETER AssemblyName
    Filter on AssemblyName. Must be fully qualified. See Test-AssemblyNameFullyQualified.
.INPUTS
	[System.Reflection.AssemblyName[]]
.EXAMPLE
    C:\PS> Get-GacAssembly

    This example returns all assemblies in the GAC.
.EXAMPLE
    C:\PS> Get-GacAssembly -Name System* -Version 2.0.0.0

    This example returns all assemblies in the GAC with a name starting with System and version 2.0.0.0.
.LINK
	Test-AssemblyNameFullyQualified
.LINK
	http://powershellgac.codeplex.com
#>
function Get-GacAssembly
{
	[CmdletBinding(DefaultParameterSetName = 'PartsSet')]
    [OutputType('System.Reflection.AssemblyName')]
	param
    (
        [Parameter(Position = 0, ParameterSetName = 'PartsSet')]
        [ValidateNotNullOrEmpty()] 
        [string[]] $Name,

        [Parameter(Position = 1, ParameterSetName = 'PartsSet')]
        [ValidateNotNullOrEmpty()]
        [string[]] $Version,

        [Parameter(Position = 2, ParameterSetName = 'PartsSet')]
        [ValidateNotNull()]
        [string[]] $Culture,

        [Parameter(Position = 3, ParameterSetName = 'PartsSet')]
        [ValidateNotNullOrEmpty()]
        [string[]] $PublicKeyToken,

        [Parameter(Position = 4, ParameterSetName = 'PartsSet')]
        [System.Reflection.ProcessorArchitecture[]] $ProcessorArchitecture,

        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'AssemblyNameSet')]
        [ValidateScript( { Test-AssemblyNameFullyQualified $_ } )]
        [System.Reflection.AssemblyName[]] $AssemblyName
	)

    process
    {
        if ($PsCmdlet.ParameterSetName -eq 'AssemblyNameSet')
        {
            $fullNames = @()
            foreach ($assmName in $AssemblyName)
            {
                $fullNames += $assmName.FullyQualifiedName
            }

            foreach ($assembly in [PowerShellGac.GlobalAssemblyCache]::GetAssemblies())
            {
                $fullyQualifiedAssemblyName = $assembly.FullyQualifiedName
                foreach ($fullName in $fullNames)
                {
                    if ($fullyQualifiedAssemblyName -eq $fullName)
                    {
                        $assembly
                        break
                    } 
                }
            }
        }
        else
        {
            foreach ($assembly in [PowerShellGac.GlobalAssemblyCache]::GetAssemblies())
            {
                $hit = $false
                foreach ($n in $Name)
                {
                    if ($assembly.Name -like $n)
                    {
                        $hit = $true
                        break
                    }
                }
                if ($Name -and -not $hit)
                {
                    continue
                }

                $hit = $false
                foreach ($v in $Version)
                {
                    if ($assembly.Version -like $v)
                    {
                        $hit = $true
                        break
                    }
                }
                if ($Version -and -not $hit)
                {
                    continue
                }

                $hit = $false
                foreach ($c in $Culture)
                {
                    if ($c -eq 'neutral' -and $assembly.CultureInfo.Equals([System.Globalization.CultureInfo]::InvariantCulture))
                    {
                        $hit = $true
                        break
                    }
                    if ($c -ne 'neutral' -and $assembly.CultureInfo -like $c)
                    {
                        $hit = $true
                        break
                    }
                }
                if ($Culture -and -not $hit)
                {
                    continue
                }

                $hit = $false
                foreach ($p in $PublicKeyToken)
                {
                    if ($assembly.PublicKeyToken -like $p)
                    {
                        $hit = $true
                        break
                    }
                }
                if ($PublicKeyToken -and -not $hit)
                {
                    continue
                }    

                $hit = $false
                foreach ($p in $ProcessorArchitecture)
                {
                    if ($assembly.ProcessorArchitecture -eq $p)
                    {
                        $hit = $true
                        break
                    }
                }
                if ($ProcessorArchitecture -and -not $hit)
                {
                    continue
                }           

                $assembly
            }
        }
    }
}

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
            $path = [PowerShellGac.GlobalAssemblyCache]::GetAssemblyPath($assmName);
            [System.IO.FileInfo] $path
        }
    }
}

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
	
	GuidScheme                              Identifier                              Description
	----------                              ----------                              -----------
	2ec93463-b0c3-45e1-8364-327e96aea856    {71F8EFBF-09AF-418D-91F1-52707CDFA274}  .NET Framework Redist Setup
	2ec93463-b0c3-45e1-8364-327e96aea856    {71F8EFBF-09AF-418D-91F1-52707CDFA274}  .NET Framework Redist Setup

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
            [PowerShellGac.GlobalAssemblyCache]::GetInstallReferences($AssemblyName)
        }
    }
}

<#
.SYNOPSIS
    Adds the assembly to the GAC
.DESCRIPTION
    Adds the assembly to the GAC. An assembly is only added to the GAC if the assembly is not already added to
	the GAC or if the file version is higher than the assembly already in the GAC. Can only be called from an 
	elevated prompt.
.PARAMETER Path
    Specifies the path to the assembly. It must be a strong named/signed assembly (PublicKeyToken must be set).
.PARAMETER InstallReference
	Specifies the InstallReference to add the assembly to the GAC
.PARAMETER Force
	Force the addition to the GAC even if the file version of the assembly already in the GAC is higher
.PARAMETER PassThru
	The path of the assembly added is used as the output
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
    [OutputType('string')]
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
        [PowerShellGac.InstallReference] $InstallReference,

        [Switch] $Force,

        [Switch] $PassThru
	)

    process
    {
        if ($Force)
        {
            $flags = 'ForceRefresh'
        }
        else
        {
            $flags = 'Refresh'
        }

        if ($PsCmdlet.ParameterSetName -eq 'PathSet')
        {
            $paths = @()
            foreach ($p in $Path)
            {
                $paths += Resolve-Path $p
            }    
        }
        else
        {
            $paths = $LiteralPath
        }

        foreach ($p in $paths)
        {
            if (!$PSCmdLet.ShouldProcess($p))
            {
                continue;
            }

            [PowerShellGac.GlobalAssemblyCache]::InstallAssembly($p, $InstallReference, $flags);
            Write-Verbose "Installed $p into the GAC"

            if ($PassThru)
            {
                # TODO : AssemblyName
                $p
            }
        }    
    }
}

<#
.SYNOPSIS
    Removes the assembly from the GAC
.DESCRIPTION
    Removes the assembly from the GAC. Can only be called from an elevated prompt.
.PARAMETER AssemblyName
    Specifies the assembly name. Must be fully qualified. See Test-AssemblyNameFullyQualified.
.PARAMETER InstallReference
	Specifies the InstallReference used to add the assembly to the GAC
.PARAMETER PassThru
	The AssemblyName removed is used as the output
.INPUTS
	[System.Reflection.AssemblyName[]]
.EXAMPLE
    C:\PS> Get-GacAssembly -Name SomeAssembly | Remove-GacAssembly

    This example removes all assemblies with the name SomeAssembly from the GAC.
.LINK
	Test-AssemblyNameFullyQualified
.LINK
	http://powershellgac.codeplex.com
#>
function Remove-GacAssembly
{
	[CmdletBinding(SupportsShouldProcess = $true)]
    [OutputType('System.Reflection.AssemblyName')]
	param
    (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [ValidateScript( { Test-AssemblyNameFullyQualified $_ } )]
        [System.Reflection.AssemblyName[]] $AssemblyName,

        [Parameter(Position = 1)]
        [PowerShellGac.InstallReference] $InstallReference,

        [Switch] $PassThru
	)

    process
    {
        foreach ($assmName in $AssemblyName)
        {
			$fullyQualifiedAssemblyName = $assmName.FullyQualifiedName

			if (!$PSCmdLet.ShouldProcess($fullyQualifiedAssemblyName))
			{
				continue;
			}

			$disp = [PowerShellGac.GlobalAssemblyCache]::UninstallAssembly($assmName, $InstallReference) 
        
			switch ($disp)
			{
				Unknown
				{
					Write-Error -Message 'Unknown Error' -Category InvalidResult -TargetObject $assmName
				}
				Uninstalled
				{
					Write-Verbose "Removed $fullyQualifiedAssemblyName from the GAC"
				}
				StillInUse
				{
					Write-Error -Message 'Still in use' -Category PermissionDenied -TargetObject $assmName
				}
				AlreadyUninstalled
				{
					Write-Error -Message 'Already uninstalled' -Category NotInstalled -TargetObject $assmName
				}
				DeletePending
				{
					Write-Error -Message 'Delete pending' -Category ResourceBusy -TargetObject $assmName
				}
				HasInstallReference
				{
					Write-Error -Message 'Has install reference' -Category PermissionDenied -TargetObject $assmName
				}
				ReferenceNotFound
				{
					Write-Error -Message 'Reference not found' -Category ObjectNotFound -TargetObject $assmName
				}
				default 
				{
					Write-Error -Message "Unknown Error: $disp" -Category InvalidResult -TargetObject $assmName
				}
			}

			if ($PassThru)
			{
				$assmName
			}
        }
    }
}

Set-StrictMode -Version Latest

Update-FormatData -PrependPath (Join-Path $PSScriptRoot 'Gac.Format.ps1xml')

Set-Alias -Name gga -Value Get-GacAssembly
Export-ModuleMember -Function Get-GacAssembly -Alias gga
Export-ModuleMember -Function Test-AssemblyNameFullyQualified, Get-GacAssemblyFile, Get-GacAssemblyInstallReference, Add-GacAssembly, Remove-GacAssembly
