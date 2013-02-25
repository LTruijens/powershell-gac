using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Reflection;

namespace PowerShellGac
{
    [Cmdlet(VerbsCommon.Get, "GacAssemblyInstallReference")]
    [OutputType(typeof(InstallReference))]
    public class GetGacAssemblyInstallReferenceCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        [ValidateFullySpecifiedAssemblyName]
        public AssemblyName AssemblyName { get; set; }

        protected override void ProcessRecord()
        {
            string displayName = AssemblyCache.GetDisplayName(AssemblyName);

            var references = new AssemblyCacheInstallReferenceEnum(displayName);
            InstallReference reference = null;

            while ((reference = references.GetNextReference()) != null)
                WriteObject(reference);

        }
    }
}
