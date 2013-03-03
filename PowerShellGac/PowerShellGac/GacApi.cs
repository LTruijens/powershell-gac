using System;
using System.Collections.Generic;
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

    internal enum QueryAssemblyInfoFlags
    {
        Default = 0,
        Validate = 1,
        GetSize = 2
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
    internal enum AssemblyCompareFlags
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

    internal enum AssemblyNameProperty
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
    internal class InstallReferenceGuid
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
}
