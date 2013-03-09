using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellGac
{
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
}
