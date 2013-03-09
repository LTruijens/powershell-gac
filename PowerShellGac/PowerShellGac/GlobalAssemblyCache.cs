using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    public static class GlobalAssemblyCache
    {
        public static IEnumerable<AssemblyName> GetAssemblies()
        {
            IAssemblyEnum assemblyEnum = null;
            ComCheck(FusionApi.CreateAssemblyEnum(out assemblyEnum, IntPtr.Zero, null, AssemblyCacheFlags.Gac, IntPtr.Zero));

            IAssemblyName fusionAssemblyName = null;
            do
            {
                ComCheck(assemblyEnum.GetNextAssembly(IntPtr.Zero, out fusionAssemblyName, 0));
                if (fusionAssemblyName != null)
                {
                    yield return new AssemblyName(GlobalAssemblyCache.GetDisplayName(fusionAssemblyName));
                }
            } while (fusionAssemblyName != null);
        }

        public static String GetAssemblyCachePath()
        {
            int bufferSize = 512;
            StringBuilder buffer = new StringBuilder(bufferSize);

            // TODO: Get better length
            ComCheck(FusionApi.GetCachePath(AssemblyCacheFlags.Gac, buffer, ref bufferSize));

            return buffer.ToString();
        }

        public static void InstallAssembly(String path, InstallReference reference, AssemblyCommitFlags flags)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (reference != null)
            {
                if (!InstallReferenceGuid.IsValidGuidScheme(reference.GuidScheme))
                {
                    throw new ArgumentException("Invalid reference guid.", "guid");
                }
            }

            var assemblyCache = GetAssemblyCache();

            ComCheck(assemblyCache.InstallAssembly((int)flags, path, reference));
        }

        public static AssemblyCacheUninstallDisposition UninstallAssembly(AssemblyName assemblyName, InstallReference reference)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (!assemblyName.IsFullyQualified())
            {
                throw new ArgumentOutOfRangeException("assemblyName", assemblyName, "Must be a fully qualified assembly name");
            }

            if (reference != null)
            {
                if (!InstallReferenceGuid.IsValidGuidScheme(reference.GuidScheme))
                {
                    throw new ArgumentException("Invalid reference guid.", "guid");
                }
            }

            var assemblyCache = GetAssemblyCache();

            AssemblyCacheUninstallDisposition dispResult = AssemblyCacheUninstallDisposition.Uninstalled;
            ComCheck(assemblyCache.UninstallAssembly(0, assemblyName.GetFullyQualifiedName(), reference, out dispResult));

            return dispResult;
        }

        public static IEnumerable<InstallReference> GetInstallReferences(AssemblyName assemblyName)
        {
            IAssemblyName fusionAssemblyName = null;
            ComCheck(FusionApi.CreateAssemblyNameObject(out fusionAssemblyName, assemblyName.GetFullyQualifiedName(), CreateAssemblyNameObjectFlags.ParseDisplayName, IntPtr.Zero));

            IInstallReferenceEnum installReferenceEnum = null;
            ComCheck(FusionApi.CreateInstallReferenceEnum(out installReferenceEnum, fusionAssemblyName, 0, IntPtr.Zero));

            IInstallReferenceItem item = null;
            do
            {
                int hr = installReferenceEnum.GetNextInstallReferenceItem(out item, 0, IntPtr.Zero);
                if ((uint)hr == 0x80070103)  // ERROR_NO_MORE_ITEMS
                {
                    yield break;
                }
                ComCheck(hr);

                IntPtr refData;
                ComCheck(item.GetReference(out refData, 0, IntPtr.Zero));

                InstallReference installReference = new InstallReference(Guid.Empty, String.Empty, String.Empty);
                Marshal.PtrToStructure(refData, installReference);
                yield return installReference;
            } while (true);
        }

        public static String GetAssemblyPath(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (!assemblyName.IsFullyQualified())
            {
                throw new ArgumentOutOfRangeException("assemblyName", assemblyName, "Must be a fully qualified assembly name");
            }

            AssemblyInfo aInfo = new AssemblyInfo();
            // TODO: better length https://sandcastle.svn.codeplex.com/svn/Development/Source/CCI/AssemblyCache.cs
            aInfo.cchBuf = 1024;
            // Get a string with the desired length
            aInfo.currentAssemblyPath = new String('\0', aInfo.cchBuf);

            var assemblyCache = GetAssemblyCache();

            ComCheck(assemblyCache.QueryAssemblyInfo(0, assemblyName.GetFullyQualifiedName(), ref aInfo));

            return aInfo.currentAssemblyPath;
        }

        public static string GetFullyQualifiedAssemblyName(AssemblyName assemblyName)
        {
            return assemblyName.GetFullyQualifiedName();
        }

        public static bool IsFullyQualifiedAssemblyName(AssemblyName assemblyName)
        {
            return assemblyName.IsFullyQualified();
        }

        private static String GetDisplayName(IAssemblyName assemblyName)
        {
            StringBuilder sDisplayName = new StringBuilder(1024);
            int iLen = 1024;

            // TODO: Get better length
            ComCheck(assemblyName.GetDisplayName(sDisplayName, ref iLen, AssemblyNameDisplayFlags.Full));

            return sDisplayName.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private static int ComCheck(int errorCode)
        {
            if (errorCode != 0) // S_OK
            {
                Marshal.ThrowExceptionForHR(errorCode);
            }

            return errorCode;
        }

        private static IAssemblyCache GetAssemblyCache()
        {
            IAssemblyCache assemblyCache = null;
            ComCheck(FusionApi.CreateAssemblyCache(out assemblyCache, 0));
            return assemblyCache;
        }
    }
}
