using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PowerShellGac
{
    public static class AssemblyNameExtensions
    {
        public static string GetFullyQualifiedName(this AssemblyName assemblyName)
        {
            if (assemblyName.ProcessorArchitecture == ProcessorArchitecture.None)
                return assemblyName.FullName;
            else
                return assemblyName.FullName + ", ProcessorArchitecture=" + assemblyName.ProcessorArchitecture.ToString().ToLower();
        }

         public static bool IsFullyQualified(this AssemblyName assemblyName)
        {
            return !String.IsNullOrEmpty(assemblyName.Name) && assemblyName.Version != null && assemblyName.CultureInfo != null && assemblyName.GetPublicKeyToken() != null;
        }
    }
}
