using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellGac
{
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
}
