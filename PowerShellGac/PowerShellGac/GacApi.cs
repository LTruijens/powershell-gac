using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace PowerShellGac
{
    //-------------------------------------------------------------
    // Interfaces defined by fusion
    //-------------------------------------------------------------
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
    internal interface IAssemblyCache
    {
        [PreserveSig()]
        int UninstallAssembly(
                            int flags,
                            [MarshalAs(UnmanagedType.LPWStr)]
                             String assemblyName,
                            InstallReference refData,
                            out AssemblyCacheUninstallDisposition disposition);

        [PreserveSig()]
        int QueryAssemblyInfo(
                            QueryAssemblyInfoFlags flags,
                            [MarshalAs(UnmanagedType.LPWStr)]
                             String assemblyName,
                            ref AssemblyInfo assemblyInfo);
        [PreserveSig()]
        int Reserved(
                            int flags,
                            IntPtr pvReserved,
                            out Object ppAsmItem,
                            [MarshalAs(UnmanagedType.LPWStr)]
                             String assemblyName);
        [PreserveSig()]
        int Reserved(out Object ppAsmScavenger);

        [PreserveSig()]
        int InstallAssembly(
                            int flags,
                            [MarshalAs(UnmanagedType.LPWStr)]
                             String assemblyFilePath,
                            InstallReference refData);
    }// IAssemblyCache

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
    internal interface IAssemblyName
    {
        [PreserveSig()]
        int SetProperty(
                int PropertyId,
                IntPtr pvProperty,
                int cbProperty);

        [PreserveSig()]
        int GetProperty(
                AssemblyNameProperty PropertyId,
                IntPtr pvProperty,
                ref int pcbProperty);

        [PreserveSig()]
        int Finalize();

        [PreserveSig()]
        int GetDisplayName(
                StringBuilder pDisplayName,
                ref int pccDisplayName,
                AssemblyNameDisplayFlags displayFlags);

        [PreserveSig()]
        int Reserved(ref Guid guid,
            Object obj1,
            Object obj2,
            String string1,
            Int64 llFlags,
            IntPtr pvReserved,
            int cbReserved,
            out IntPtr ppv);

        [PreserveSig()]
        int GetName(
                ref int pccBuffer,
                StringBuilder pwzName);

        [PreserveSig()]
        int GetVersion(
                out int versionHi,
                out int versionLow);
        [PreserveSig()]
        int IsEqual(
                IAssemblyName pAsmName,
                AssemblyCompareFlags cmpFlags);

        [PreserveSig()]
        int Clone(out IAssemblyName pAsmName);
    }// IAssemblyName

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
    internal interface IAssemblyEnum
    {
        [PreserveSig()]
        int GetNextAssembly(
                IntPtr pvReserved,
                out IAssemblyName ppName,
                int flags);
        [PreserveSig()]
        int Reset();
        [PreserveSig()]
        int Clone(out IAssemblyEnum ppEnum);
    }// IAssemblyEnum

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("582dac66-e678-449f-aba6-6faaec8a9394")]
    internal interface IInstallReferenceItem
    {
        // A pointer to a FUSION_INSTALL_REFERENCE structure.
        // The memory is allocated by the GetReference method and is freed when
        // IInstallReferenceItem is released. Callers must not hold a reference to this
        // buffer after the IInstallReferenceItem object is released.
        // This uses the InstallReferenceOutput object to avoid allocation
        // issues with the interop layer.
        // This cannot be marshaled directly - must use IntPtr
        [PreserveSig()]
        int GetReference(
                out IntPtr pRefData,
                int flags,
                IntPtr pvReserced);
    }// IInstallReferenceItem

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("56b1a988-7c0c-4aa2-8639-c3eb5a90226f")]
    internal interface IInstallReferenceEnum
    {
        [PreserveSig()]
        int GetNextInstallReferenceItem(
                out IInstallReferenceItem ppRefItem,
                int flags,
                IntPtr pvReserced);
    }// IInstallReferenceEnum

    public enum QueryAssemblyInfoFlags
    {
        Default = 0,
        Validate = 1,
        GetSize = 2
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
    public enum AssemblyCommitFlags
    {
        Refresh = 1,
        ForceRefresh = 2
    }

    public enum AssemblyCacheUninstallDisposition
    {
        Unknown = 0,
        Uninstalled = 1,
        StillInUse = 2,
        AlreadyUninstalled = 3,
        DeletePending = 4,
        HasInstallReference = 5,
        ReferenceNotFound = 6
    }

    [Flags]
    internal enum AssemblyCacheFlags
    {
        Zap = 0x1,
        Gac = 0x2,
        Download = 0x4,
        Root = 0x8,
        RootEx = 0x80
    }

    internal enum CreateAssemblyNameObjectFlags
    {
        Default = 0,
        ParseDisplayName = 0x1,
        SetDefaultValues = 0x2,
        VerifyFriendAssemblyName = 0x4,

        ParseFriendDisplayName = ParseDisplayName | VerifyFriendAssemblyName
    }

    [Flags]
    internal enum AssemblyNameDisplayFlags
    {
        Version = 0x01,
        Culture = 0x02,
        PublicKeyToken = 0x04,
        PublicKey = 0x08,
        Custom = 0x10,
        ProcessArchitecture = 0x20,
        LanguageID = 0x40,      
        Retarget = 0x80,
        ConfigMask = 0x100,
        Mvid = 0x200,

        Full = Version | Culture | PublicKeyToken | Retarget | ProcessArchitecture
    }

    [Flags]
    public enum AssemblyCompareFlags
    {
        Name = 0x01,
        MajorVersion = 0x02,
        MinorVersion = 0x04,
        BuildNumber = 0x08,
        RevisionNumber = 0x10,
        PublicKeyToken = 0x20,
        Culture = 0x40,
        Custom = 0x80,
        Default = 0x100,
        Retarget = 0x200,
        Architecture = 0x400,
        ConfigMask = 0x800,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mvid")]
        Mvid = 0x1000,
        Signature = 0x2000,

        Version = MajorVersion | MinorVersion | BuildNumber | RevisionNumber,
        IlAll = Name | Version | PublicKeyToken | Culture,     
        IlNoVersion = Name | PublicKeyToken | Culture
    }

    public enum AssemblyNameProperty
    {
        PublicKey = 0,
        PublicKeyToken,
        HashValue,
        Name,
        MajorVersion,
        MinorVersion,
        BuildNumber,
        RevisionNumber,
        Culture,
        ProcessorIdArray,
        OSInfoArray,
        HashAlgId,
        Alias,
        CodebaseUrl,
        CodebaseLastMod,
        NullPublicKey,
        NullPublicKeyToken,
        Custom,
        NullCustom,
        Mvid,
        FileMajroVersion,
        FileMinorVersion,
        FileBuildNumber,
        FileRevisionNumber,
        Retarget,
        SignatureBlob,
        ConfigMask,
        Architecture,
        MaxParams
    }

    [StructLayout(LayoutKind.Sequential)]
    public class InstallReference
    {
        public InstallReference(Guid guid, String id, String data)
        {
            cbSize = (int)(2 * IntPtr.Size + 16 + (id.Length + data.Length) * 2);
            flags = 0;
            // quiet compiler warning
            if (flags == 0) { }
            guidScheme = guid;
            identifier = id;
            description = data;
        }

        public Guid GuidScheme
        {
            get { return guidScheme; }
        }

        public String Identifier
        {
            get { return identifier; }
        }

        public String Description
        {
            get { return description; }
        }

        int cbSize;
        int flags;
        Guid guidScheme;
        [MarshalAs(UnmanagedType.LPWStr)]
        String identifier;
        [MarshalAs(UnmanagedType.LPWStr)]
        String description;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AssemblyInfo
    {
        public int cbAssemblyInfo; // size of this structure for future expansion
        public int assemblyFlags;
        public long assemblySizeInKB;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String currentAssemblyPath;
        public int cchBuf; // size of path buf.
    }

    [ComVisible(false)]
    public class InstallReferenceGuid
    {
        public static bool IsValidGuidScheme(Guid guid)
        {
            return (guid.Equals(UninstallSubkeyGuid) ||
                    guid.Equals(FilePathGuid) ||
                    guid.Equals(OpaqueGuid) ||
                    guid.Equals(Guid.Empty));
        }

        public readonly static Guid UninstallSubkeyGuid = new Guid("8cedc215-ac4b-488b-93c0-a50a49cb2fb8");
        public readonly static Guid FilePathGuid = new Guid("b02f9d65-fb77-4f7a-afa5-b391309f11c9");
        public readonly static Guid OpaqueGuid = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");
        // these GUIDs cannot be used for installing into GAC.
        public readonly static Guid MsiGuid = new Guid("25df0fc1-7f97-4070-add7-4b13bbfd7cb8");
        public readonly static Guid OsInstallGuid = new Guid("d16d444c-56d8-11d5-882d-0080c847b195");
    }

    [ComVisible(false)]
    public static class AssemblyCache
    {
        public static void InstallAssembly(String assemblyPath, InstallReference reference, AssemblyCommitFlags flags)
        {
            if (reference != null)
            {
                if (!InstallReferenceGuid.IsValidGuidScheme(reference.GuidScheme))
                    throw new ArgumentException("Invalid reference guid.", "guid");
            }

            IAssemblyCache ac = null;

            int hr = 0;

            hr = FusionWrapper.CreateAssemblyCache(out ac, 0);
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
        public static void UninstallAssembly(String assemblyName, InstallReference reference, out AssemblyCacheUninstallDisposition disp)
        {
            AssemblyCacheUninstallDisposition dispResult = AssemblyCacheUninstallDisposition.Uninstalled;
            if (reference != null)
            {
                if (!InstallReferenceGuid.IsValidGuidScheme(reference.GuidScheme))
                    throw new ArgumentException("Invalid reference guid.", "guid");
            }

            IAssemblyCache ac = null;

            int hr = FusionWrapper.CreateAssemblyCache(out ac, 0);
            if (hr >= 0)
            {
                hr = ac.UninstallAssembly(0, assemblyName, reference, out dispResult);
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            disp = dispResult;
        }

        // See comments in UninstallAssembly
        // TODO: Check if fully specified
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
            int hr = FusionWrapper.CreateAssemblyCache(out ac, 0);
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

        public static String GetGacPath()
        {
            int bufferSize = 512;
            StringBuilder buffer = new StringBuilder(bufferSize);

            int hr = FusionWrapper.GetCachePath(AssemblyCacheFlags.Gac, buffer, ref bufferSize);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return buffer.ToString();
        }

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

    [ComVisible(false)]
    internal class AssemblyCacheEnum
    {
        // null means enumerate all the assemblies
        public AssemblyCacheEnum(String assemblyName)
        {
            IAssemblyName fusionName = null;
            int hr = 0;

            if (assemblyName != null)
            {
                hr = FusionWrapper.CreateAssemblyNameObject(
                        out fusionName,
                        assemblyName,
                        CreateAssemblyNameObjectFlags.ParseDisplayName,
                        IntPtr.Zero);
            }

            if (hr >= 0)
            {
                hr = FusionWrapper.CreateAssemblyEnum(
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

        public IAssemblyName GetNextAssembly()
        {
            int hr = 0;
            IAssemblyName fusionName = null;

            if (done)
            {
                return null;
            }

            // Now get next IAssemblyName from m_AssemblyEnum
            hr = m_AssemblyEnum.GetNextAssembly((IntPtr)0, out fusionName, 0);

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            done = fusionName == null;

            return fusionName;
        }

        private IAssemblyEnum m_AssemblyEnum = null;
        private bool done = false;
    }// class AssemblyCacheEnum

    public class AssemblyCacheInstallReferenceEnum
    {
        public AssemblyCacheInstallReferenceEnum(String assemblyName)
        {
            IAssemblyName fusionName = null;

            int hr = FusionWrapper.CreateAssemblyNameObject(
                        out fusionName,
                        assemblyName,
                        CreateAssemblyNameObjectFlags.ParseDisplayName,
                        IntPtr.Zero);

            if (hr >= 0)
            {
                hr = FusionWrapper.CreateInstallReferenceEnum(out refEnum, fusionName, 0, IntPtr.Zero);
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        public InstallReference GetNextReference()
        {
            IInstallReferenceItem item = null;
            int hr = refEnum.GetNextInstallReferenceItem(out item, 0, IntPtr.Zero);
            if ((uint)hr == 0x80070103)
            {   // ERROR_NO_MORE_ITEMS
                return null;
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            IntPtr refData;
            InstallReference instRef = new InstallReference(Guid.Empty, String.Empty, String.Empty);

            hr = item.GetReference(out refData, 0, IntPtr.Zero);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            Marshal.PtrToStructure(refData, instRef);
            return instRef;
        }

        private IInstallReferenceEnum refEnum;
    }

    internal class FusionWrapper
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
