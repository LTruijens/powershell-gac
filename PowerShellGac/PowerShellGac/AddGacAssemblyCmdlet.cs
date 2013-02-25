using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Reflection;

namespace PowerShellGac
{
    [Cmdlet(VerbsCommon.Add, "GacAssembly", SupportsShouldProcess = true)]
    [OutputType(typeof(string))]
    public class AddGacAssemblyCmdlet : PSCmdlet
    {
        // TODO: string[]
        // TODO: http://stackoverflow.com/questions/8505294/how-do-i-deal-with-paths-when-writing-a-powershell-cmdlet
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        [Parameter]
        public InstallReference InstallReference { get; set; }

        // The files of an existing assembly are overwritten regardless of their file version number.
        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            AssemblyCommitFlags flags;
            
            if (Force.IsPresent)
                flags = AssemblyCommitFlags.ForceRefresh;
            else
                flags = AssemblyCommitFlags.Refresh;

            if (!ShouldProcess(Path))
            {
                return;
            }

            AssemblyCache.InstallAssembly(Path, InstallReference, flags);
            WriteVerbose(string.Format("Installed {0} into the GAC", Path));

            if (PassThru.IsPresent)
            {
                // TODO: of AssemblyName?
                WriteObject(Path);
            }
        }
    }
}
