using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    internal static class InstallReferenceGuid
    {
        public readonly static Guid UninstallSubkeyGuid = new Guid("8cedc215-ac4b-488b-93c0-a50a49cb2fb8");
        public readonly static Guid FilePathGuid = new Guid("b02f9d65-fb77-4f7a-afa5-b391309f11c9");
        public readonly static Guid OpaqueGuid = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");
        public readonly static Guid MsiGuid = new Guid("25df0fc1-7f97-4070-add7-4b13bbfd7cb8");
        public readonly static Guid OsInstallGuid = new Guid("d16d444c-56d8-11d5-882d-0080c847b195");

        public static Guid FromType(InstallReferenceType type)
        {
            switch (type)
            {
                case InstallReferenceType.WindowsInstaller:
                    return InstallReferenceGuid.MsiGuid;
                case InstallReferenceType.Installer:
                    return InstallReferenceGuid.UninstallSubkeyGuid;
                case InstallReferenceType.FilePath:
                    return InstallReferenceGuid.FilePathGuid;
                case InstallReferenceType.Opaque:
                    return InstallReferenceGuid.OpaqueGuid;
                case InstallReferenceType.OsInstall:
                    return InstallReferenceGuid.OsInstallGuid;
                default:
                    throw new InvalidOperationException(String.Format("Unknown InstallReferencGuid for {0}", type));
            }
        }

        public static InstallReferenceType ToType(Guid guid)
        {
            if (guid == InstallReferenceGuid.MsiGuid)
                return InstallReferenceType.WindowsInstaller;
            else if (guid == InstallReferenceGuid.UninstallSubkeyGuid)
                return InstallReferenceType.Installer;
            else if (guid == InstallReferenceGuid.FilePathGuid)
                return InstallReferenceType.FilePath;
            else if (guid == InstallReferenceGuid.OpaqueGuid)
                return InstallReferenceType.Opaque;
            else if (guid == InstallReferenceGuid.OsInstallGuid)
                return InstallReferenceType.OsInstall;
            else
                throw new InvalidOperationException(String.Format("Unknown InstallReferencType for {0}", guid));
        }
    }
}
