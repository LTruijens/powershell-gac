using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Reflection;
using System.Globalization;

namespace PowerShellGac
{
    [Cmdlet(VerbsCommon.Get, "GacAssembly", DefaultParameterSetName = "Parts")]
    [OutputType(typeof(AssemblyName))]
    public class GetGacAssemblyCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Parts")]
        [ValidateNotNullOrEmpty()] 
        public string Name { get; set; }

        [Parameter(Position = 1, ParameterSetName = "Parts")]
        [ValidateNotNullOrEmpty()]
        public string Version { get; set; }

        [Parameter(Position = 2, ParameterSetName = "Parts")]
        [ValidateNotNullOrEmpty()]
        public string Culture { get; set; }

        [Parameter(Position = 3, ParameterSetName = "Parts")]
        [ValidateNotNullOrEmpty()]
        public string PublicKeyToken { get; set; }

        [Parameter(Position = 4, ParameterSetName = "Parts")]
        public ProcessorArchitecture ProcessorArchitecture { get; set; }

        // TODO: AssemblyName[]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "AssemblyName")]
        [ValidateNotNullOrEmpty]
        public AssemblyName AssemblyName { get; set; }

        protected override void ProcessRecord()
        {
            WildcardPattern assemblyNamePattern = null;
            if (AssemblyName != null)
            {
                assemblyNamePattern = new WildcardPattern(AssemblyName.ToString(), WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
            }
            WildcardPattern namePattern = null;
            if (!String.IsNullOrEmpty(Name))
            {
                namePattern = new WildcardPattern(Name, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
            }
            WildcardPattern versionPattern = null;
            if (!String.IsNullOrEmpty(Version))
            {
                versionPattern = new WildcardPattern(Version, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
            }
            WildcardPattern culturePattern = null;
            if (!String.IsNullOrEmpty(Culture))
            {
                culturePattern = new WildcardPattern(Culture, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
            }
            WildcardPattern publicKeyTokenPattern = null;
            if (!String.IsNullOrEmpty(PublicKeyToken))
            {
                publicKeyTokenPattern = new WildcardPattern(PublicKeyToken, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
            }

            var assemblies = new AssemblyCacheEnum(null);
            IAssemblyName assm = null;
            while ((assm = assemblies.GetNextAssembly()) != null)
            {
                var displayName = AssemblyCache.GetDisplayName(assm);

                if (assemblyNamePattern != null && !assemblyNamePattern.IsMatch(displayName)) continue;

                var assemblyName = new AssemblyName(displayName);

                if (namePattern != null && !namePattern.IsMatch(assemblyName.Name)) continue;

                if (versionPattern != null && !versionPattern.IsMatch(assemblyName.Version.ToString())) continue;

                if (culturePattern != null && !culturePattern.IsMatch(GetCultureName(assemblyName))) continue;

                if (publicKeyTokenPattern != null && !publicKeyTokenPattern.IsMatch(GetPublicKeyTokenAsHex(assemblyName))) continue;

                if (MyInvocation.BoundParameters.ContainsKey("ProcessorArchitecture") && assemblyName.ProcessorArchitecture != ProcessorArchitecture) continue;
                
                WriteObject(assemblyName);
            }
        }

        private string GetCultureName(AssemblyName assemblyName)
        {
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture))
                return "neutral";
            else
                return assemblyName.CultureInfo.ToString();
        }

        private string GetPublicKeyTokenAsHex(AssemblyName assemblyName)
        {
            var result = new StringBuilder(16);
            foreach (var b in assemblyName.GetPublicKeyToken())
            {
                result.AppendFormat("{0:x2}", b);
            }
            return result.ToString();
        }
    }
}
