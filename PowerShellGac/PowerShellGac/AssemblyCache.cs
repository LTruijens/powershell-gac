using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    public static class AssemblyCache
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static void InstallAssembly(String assemblyPath, InstallReference reference, AssemblyCommitFlags flags)
        {
            if (reference != null)
            {
                if (!InstallReferenceGuid.IsValidGuidScheme(reference.GuidScheme))
                    throw new ArgumentException("Invalid reference guid.", "guid");
            }

            IAssemblyCache ac = null;

            int hr = 0;

            hr = GacApi.CreateAssemblyCache(out ac, 0);
            if (hr >= 0)
            {
                hr = ac.InstallAssembly((int)flags, assemblyPath, reference);
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        // assemblyName has to be fully specified name.
        // A.k.a, for v1.0/v1.1 assemblies, it should be "name, Version=xx, Culture=xx, PublicKeyToken=xx".
        // For v2.0 assemblies, it should be "name, Version=xx, Culture=xx, PublicKeyToken=xx, ProcessorArchitecture=xx".
        // If assemblyName is not fully specified, a random matching assembly will be uninstalled.
        // TODO: Check if fully specified
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static AssemblyCacheUninstallDisposition UninstallAssembly(String assemblyName, InstallReference reference)
        {
            AssemblyCacheUninstallDisposition dispResult = AssemblyCacheUninstallDisposition.Uninstalled;
            if (reference != null)
            {
                if (!InstallReferenceGuid.IsValidGuidScheme(reference.GuidScheme))
                    throw new ArgumentException("Invalid reference guid.", "guid");
            }

            IAssemblyCache ac = null;

            int hr = GacApi.CreateAssemblyCache(out ac, 0);
            if (hr >= 0)
            {
                hr = ac.UninstallAssembly(0, assemblyName, reference, out dispResult);
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return dispResult;
        }

        // See comments in UninstallAssembly
        // TODO: Check if fully specified
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static String QueryAssemblyInfo(String assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentException("Invalid name", "assemblyName");
            }

            AssemblyInfo aInfo = new AssemblyInfo();
            // TODO: better length https://sandcastle.svn.codeplex.com/svn/Development/Source/CCI/AssemblyCache.cs
            aInfo.cchBuf = 1024;
            // Get a string with the desired length
            aInfo.currentAssemblyPath = new String('\0', aInfo.cchBuf);

            IAssemblyCache ac = null;
            int hr = GacApi.CreateAssemblyCache(out ac, 0);
            if (hr >= 0)
            {
                hr = ac.QueryAssemblyInfo(0, assemblyName, ref aInfo);
            }
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return aInfo.currentAssemblyPath;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static String GetGacPath()
        {
            int bufferSize = 512;
            StringBuilder buffer = new StringBuilder(bufferSize);

            int hr = GacApi.GetCachePath(AssemblyCacheFlags.Gac, buffer, ref bufferSize);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return buffer.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        internal static String GetDisplayName(IAssemblyName assemblyName)
        {
            StringBuilder sDisplayName = new StringBuilder(1024);
            int iLen = 1024;

            int hr = assemblyName.GetDisplayName(sDisplayName, ref iLen, AssemblyNameDisplayFlags.Full);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return sDisplayName.ToString();
        }

        internal static byte[] GetPublicKeyToken(IAssemblyName assemblyName)
        {
            int bufferSize = 8;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
            try
            {
                int hr = assemblyName.GetProperty(AssemblyNameProperty.PublicKeyToken, buffer, ref bufferSize);
                if (hr < 0)
                {
                    Marshal.ThrowExceptionForHR(hr);
                }

                byte[] publicKeyToken = new byte[bufferSize];

                Marshal.Copy(buffer, publicKeyToken, 0, bufferSize);

                return publicKeyToken;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        internal static byte[] GetPublicKey(IAssemblyName assemblyName)
        {
            int bufferSize = 512;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
            try
            {
                int hr = assemblyName.GetProperty(AssemblyNameProperty.PublicKey, buffer, ref bufferSize);
                if (hr < 0)
                {
                    Marshal.ThrowExceptionForHR(hr);
                }

                byte[] publicKey = new byte[bufferSize];

                Marshal.Copy(buffer, publicKey, 0, bufferSize);

                return publicKey;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        internal static CultureInfo GetCulture(IAssemblyName assemblyName)
        {
            int bufferSize = 512;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
            try
            {
                int hr = assemblyName.GetProperty(AssemblyNameProperty.Culture, buffer, ref bufferSize);
                if (hr < 0)
                {
                    Marshal.ThrowExceptionForHR(hr);
                }

                string culture = Marshal.PtrToStringAuto(buffer);

                return new CultureInfo(culture);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static string GetDisplayName(System.Reflection.AssemblyName assemblyName)
        {
            if (assemblyName.ProcessorArchitecture == System.Reflection.ProcessorArchitecture.None)
                return assemblyName.FullName;
            else
                return assemblyName.FullName + ", ProcessorArchitecture=" + assemblyName.ProcessorArchitecture.ToString().ToLower();
        }
    }
}
