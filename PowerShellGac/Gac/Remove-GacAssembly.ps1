<#
.SYNOPSIS
    Removes the assembly from the GAC
.DESCRIPTION
    Removes the assembly from the GAC. Can only be called from an elevated prompt.
.PARAMETER AssemblyName
    Specifies the assembly name. Must be fully qualified. See Test-AssemblyNameFullyQualified.
.PARAMETER InstallReference
	Specifies the InstallReference used to remove the assembly from the GAC.
.PARAMETER PassThru
	The AssemblyName removed is returned as output
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
        [ValidateNotNullOrEmpty()]
        [ValidateScript( { Test-GacAssemblyInstallReferenceCanBeUsed $_ } )]
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
				continue
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
					Write-Error -Message 'Still in use. An application is using the assembly.' -Category PermissionDenied -TargetObject $assmName
				}
				AlreadyUninstalled
				{
					Write-Error -Message 'Already uninstalled. The assembly does not exist in the GAC.' -Category NotInstalled -TargetObject $assmName
				}
				DeletePending
				{
					Write-Error -Message 'Delete pending' -Category ResourceBusy -TargetObject $assmName
				}
				HasInstallReference
				{
					Write-Error -Message 'Has install reference. The assembly has not been removed from the GAC because another install reference exists.' -Category PermissionDenied -TargetObject $assmName
				}
				ReferenceNotFound
				{
					Write-Error -Message 'Reference not found. The reference that is specified is not found in the GAC' -Category ObjectNotFound -TargetObject $assmName
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