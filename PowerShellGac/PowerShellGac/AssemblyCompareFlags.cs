using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellGac
{
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
}
