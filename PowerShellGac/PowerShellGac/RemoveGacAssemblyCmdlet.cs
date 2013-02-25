using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Reflection;

namespace PowerShellGac
{
    [Cmdlet(VerbsCommon.Remove, "GacAssembly", SupportsShouldProcess = true)]
    [OutputType(typeof(AssemblyName))]
    public class RemoveGacAssemblyCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        [ValidateFullySpecifiedAssemblyName]
        public AssemblyName AssemblyName { get; set; }

        [Parameter]
        public InstallReference InstallReference { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            string displayName = AssemblyCache.GetDisplayName(AssemblyName);

            if (!ShouldProcess(displayName))
            {
                return;
            }

            AssemblyCacheUninstallDisposition disp;
            AssemblyCache.UninstallAssembly(displayName, InstallReference, out disp);
            switch (disp)
            {
                case AssemblyCacheUninstallDisposition.Unknown:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Unknown Error."),
                            "Error",
                            ErrorCategory.InvalidResult,
                            AssemblyName));                    
                    break;
                case AssemblyCacheUninstallDisposition.Uninstalled:
                    WriteVerbose(string.Format("Removed {0} from the GAC", AssemblyName));
                    break;
                case AssemblyCacheUninstallDisposition.StillInUse:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Still in use."),
                            "Error",
                            ErrorCategory.PermissionDenied,
                            AssemblyName));                    
                    break;
                case AssemblyCacheUninstallDisposition.AlreadyUninstalled:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Already uninstalled."),
                            "Error",
                            ErrorCategory.NotInstalled,
                            AssemblyName));                    
                    break;
                case AssemblyCacheUninstallDisposition.DeletePending:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Delete pending."),
                            "Error",
                            ErrorCategory.ResourceBusy,
                            AssemblyName));                    
                    break;
                case AssemblyCacheUninstallDisposition.HasInstallReference:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Has install reference."),
                            "Error",
                            ErrorCategory.PermissionDenied,
                            AssemblyName));                    
                    break;
                case AssemblyCacheUninstallDisposition.ReferenceNotFound:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Reference not found."),
                            "Error",
                            ErrorCategory.ObjectNotFound,
                            AssemblyName));                    
                    break;
                default:
                    WriteError(new ErrorRecord(
                        new Exception(
                            "Unknown result."),
                            "Error",
                            ErrorCategory.InvalidResult,
                            AssemblyName));                    
                    break;
            }

            if (PassThru.IsPresent)
                WriteObject(AssemblyName);
        }
    }
}
