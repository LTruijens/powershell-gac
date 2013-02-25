using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Reflection;
using System.IO;

namespace PowerShellGac
{
    [Cmdlet(VerbsCommon.Get, "GacAssemblyFile")]
    [OutputType(typeof(FileInfo))]
    public class GetGacAssemblyFileCmdlet : PSCmdlet
    {
        // TODO: AssemblyName[]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        [ValidateFullySpecifiedAssemblyName]
        public AssemblyName AssemblyName { get; set; }

        protected override void ProcessRecord()
        {
            string name = AssemblyCache.GetDisplayName(AssemblyName);
            string path = AssemblyCache.QueryAssemblyInfo(name);
            WriteObject(new FileInfo(path));
        }
    }
}
