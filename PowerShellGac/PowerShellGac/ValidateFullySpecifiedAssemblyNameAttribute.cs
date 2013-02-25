using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;

namespace PowerShellGac
{
    [AttributeUsageAttribute(AttributeTargets.Property|AttributeTargets.Field)] 
    public class ValidateFullySpecifiedAssemblyNameAttribute : ValidateEnumeratedArgumentsAttribute
    {
        protected override void ValidateElement(object element)
        {
            var assemblyName = element as AssemblyName;

            if (assemblyName == null)
            {
                throw new ValidationMetadataException("Not an AssemblyName");
            }

            if (String.IsNullOrWhiteSpace(assemblyName.Name))
            {
                throw new ValidationMetadataException("Not a fully specified AssemblyName. Name is missing");
            }

            if (assemblyName.Version == null)
            {
                throw new ValidationMetadataException("Not a fully specified AssemblyName. Version is missing");
            }

            if (assemblyName.CultureInfo == null)
            {
                throw new ValidationMetadataException("Not a fully specified AssemblyName. Culture is missing");
            }

            if (assemblyName.GetPublicKeyToken() == null)
            {
                throw new ValidationMetadataException("Not a fully specified AssemblyName. PublicKeyToken is missing");
            }
        }

        public override string ToString()
        {
            return "ValidateFullySpecifiedAssemblyNameAttribute";
        }
    }
}
