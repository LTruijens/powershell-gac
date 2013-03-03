using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    public class AssemblyCacheEnum
    {
        public AssemblyCacheEnum() : this(null)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public AssemblyCacheEnum(string assemblyName)
        {
            IAssemblyName fusionName = null;
            int hr = 0;

            if (assemblyName != null)
            {
                hr = NativeMethods.CreateAssemblyNameObject(
                        out fusionName,
                        assemblyName,
                        CreateAssemblyNameObjectFlags.ParseDisplayName,
                        IntPtr.Zero);
            }

            if (hr >= 0)
            {
                hr = NativeMethods.CreateAssemblyEnum(
                        out m_AssemblyEnum,
                        IntPtr.Zero,
                        fusionName,
                        AssemblyCacheFlags.Gac,
                        IntPtr.Zero);
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public AssemblyName GetNextAssembly()
        {
            if (done)
            {
                return null;
            }

            IAssemblyName fusionName = null;
            int hr = m_AssemblyEnum.GetNextAssembly((IntPtr)0, out fusionName, 0);

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            done = fusionName == null;

            if (fusionName == null)
            {
                return null;
            }
            else
            {
                return new AssemblyName(AssemblyCache.GetDisplayName(fusionName));
            }
        }

        private IAssemblyEnum m_AssemblyEnum = null;
        private bool done = false;
    }
}
