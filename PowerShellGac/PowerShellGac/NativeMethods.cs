using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    internal static class NativeMethods
    {
        [DllImport("fusion.dll")]
        internal static extern int CreateAssemblyEnum(
                out IAssemblyEnum ppEnum,
                IntPtr pUnkReserved,
                IAssemblyName pName,
                AssemblyCacheFlags flags,
                IntPtr pvReserved);

        [DllImport("fusion.dll")]
        internal static extern int CreateAssemblyNameObject(
                out IAssemblyName ppAssemblyNameObj,
                [MarshalAs(UnmanagedType.LPWStr)]
                 String szAssemblyName,
                CreateAssemblyNameObjectFlags flags,
                IntPtr pvReserved);

        [DllImport("fusion.dll")]
        internal static extern int CreateAssemblyCache(
                out IAssemblyCache ppAsmCache,
                int reserved);

        [DllImport("fusion.dll")]
        internal static extern int CreateInstallReferenceEnum(
                out IInstallReferenceEnum ppRefEnum,
                IAssemblyName pName,
                int dwFlags,
                IntPtr pvReserved);

        [DllImport("fusion.dll")]
        internal static extern int GetCachePath(
            AssemblyCacheFlags assemblyCacheFlags,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder cachePath,
            ref int cachePathSize);
    }
}
