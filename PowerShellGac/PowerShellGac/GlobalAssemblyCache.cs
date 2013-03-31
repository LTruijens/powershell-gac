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

        public static String GetAssemblyCacheClr2Path()
        {
            int bufferSize = 512;
            StringBuilder buffer = new StringBuilder(bufferSize);

            int hResult = FusionApi.GetCachePath(AssemblyCacheFlags.Root, buffer, ref bufferSize);
            if ((uint)hResult == 0x8007007A)  // ERROR_INSUFFICIENT_BUFFER
            {
                buffer = new StringBuilder(bufferSize);
                ComCheck(FusionApi.GetCachePath(AssemblyCacheFlags.Root, buffer, ref bufferSize));
            }
            else
            {
                ComCheck(hResult);
            }

            return buffer.ToString();
        }

        public static String GetAssemblyCacheClr4Path()
        {
            int bufferSize = 512;
            StringBuilder buffer = new StringBuilder(bufferSize);

            int hResult = FusionApi.GetCachePath(AssemblyCacheFlags.RootEx, buffer, ref bufferSize);
            if ((uint)hResult == 0x8007007A)  // ERROR_INSUFFICIENT_BUFFER
            {
                buffer = new StringBuilder(bufferSize);
                ComCheck(FusionApi.GetCachePath(AssemblyCacheFlags.RootEx, buffer, ref bufferSize));
            }
            else
            {
                ComCheck(hResult);
            }

            return buffer.ToString();
        }

        public static void InstallAssembly(String path, InstallReference reference, bool force)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            AssemblyCommitFlags flags;
            if (force)
            {
                flags = AssemblyCommitFlags.ForceRefresh;
            }
            else
            {
                flags = AssemblyCommitFlags.Refresh;
            }

            FusionInstallReference fusionReference = null;
            if (reference != null)
            {
                if (!reference.CanBeUsed())
                {
                    throw new ArgumentException("InstallReferenceType can not be used", "reference");
                }

                fusionReference = new FusionInstallReference(reference.Type, reference.Identifier, reference.Description);
            }

            var assemblyCache = GetAssemblyCache();

            ComCheck(assemblyCache.InstallAssembly((int)flags, path, fusionReference));
        }

        public static UninstallResult UninstallAssembly(AssemblyName assemblyName, InstallReference reference)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (!assemblyName.IsFullyQualified())
            {
                throw new ArgumentOutOfRangeException("assemblyName", assemblyName, "Must be a fully qualified assembly name");
            }

            FusionInstallReference fusionReference = null;
            if (reference != null)
            {
                if (!reference.CanBeUsed())
                {
                    throw new ArgumentException("InstallReferenceType can not be used", "reference");
                }

                fusionReference = new FusionInstallReference(reference.Type, reference.Identifier, reference.Description);
            }

            var assemblyCache = GetAssemblyCache();

            AssemblyCacheUninstallDisposition disposition = AssemblyCacheUninstallDisposition.Uninstalled;
            ComCheck(assemblyCache.UninstallAssembly(0, assemblyName.GetFullyQualifiedName(), fusionReference, out disposition));

            return (UninstallResult)disposition;
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
                int hResult = installReferenceEnum.GetNextInstallReferenceItem(out item, 0, IntPtr.Zero);
                if ((uint)hResult == 0x80070103)  // ERROR_NO_MORE_ITEMS
                {
                    yield break;
                }
                ComCheck(hResult);

                IntPtr refData;
                ComCheck(item.GetReference(out refData, 0, IntPtr.Zero));

                FusionInstallReference fusionReference = new FusionInstallReference();
                Marshal.PtrToStructure(refData, fusionReference);

                var reference = new InstallReference(InstallReferenceGuid.ToType(fusionReference.GuidScheme), fusionReference.Identifier,
                    fusionReference.NonCanonicalData);

                yield return reference;
            } while (true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
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

            var assemblyCache = GetAssemblyCache();

            AssemblyInfo info = new AssemblyInfo();
            info.cbAssemblyInfo = Marshal.SizeOf(typeof(AssemblyInfo));
            info.cchBuf = 1024;
            info.currentAssemblyPath = new String('\0', info.cchBuf);

            int hResult = assemblyCache.QueryAssemblyInfo(QueryAssemblyInfoFlags.Default, assemblyName.GetFullyQualifiedName(), ref info);
            if ((uint)hResult == 0x8007007A)  // ERROR_INSUFFICIENT_BUFFER
            {
                info.currentAssemblyPath = new String('\0', info.cchBuf);
                ComCheck(assemblyCache.QueryAssemblyInfo(QueryAssemblyInfoFlags.Default, assemblyName.GetFullyQualifiedName(), ref info));
            }
            else
            {
                ComCheck(hResult);
            }

            return info.currentAssemblyPath;
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
            int bufferSize = 1024;
            StringBuilder buffer = new StringBuilder(bufferSize);

            int hResult = assemblyName.GetDisplayName(buffer, ref bufferSize, AssemblyNameDisplayFlags.Full);
            if ((uint)hResult == 0x8007007A)  // ERROR_INSUFFICIENT_BUFFER
            {
                buffer = new StringBuilder(bufferSize);
                ComCheck(assemblyName.GetDisplayName(buffer, ref bufferSize, AssemblyNameDisplayFlags.Full));
            }
            else
            {
                ComCheck(hResult);
            }

            return buffer.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private static int ComCheck(int hResult)
        {
            if (hResult != 0) // S_OK
            {
                Marshal.ThrowExceptionForHR(hResult);
            }

            return hResult;
        }

        private static IAssemblyCache GetAssemblyCache()
        {
            IAssemblyCache assemblyCache = null;
            ComCheck(FusionApi.CreateAssemblyCache(out assemblyCache, 0));
            return assemblyCache;
        }
    }
}
