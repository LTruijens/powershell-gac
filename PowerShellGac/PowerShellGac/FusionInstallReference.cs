using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    [StructLayout(LayoutKind.Sequential)]
    internal class FusionInstallReference
    {
        public FusionInstallReference()
            : this(Guid.Empty, null, null)
        {
        }

        public FusionInstallReference(InstallReferenceType type, String identifier, String nonCanonicalData)
            : this(InstallReferenceGuid.FromType(type), identifier, nonCanonicalData)
        {
        }

        public FusionInstallReference(Guid guidScheme, String identifier, String nonCanonicalData)
        {
            int idLength = identifier == null ? 0 : identifier.Length;
            int dataLength = nonCanonicalData == null ? 0 : nonCanonicalData.Length;

            cbSize = (int)(2 * IntPtr.Size + 16 + (idLength + dataLength) * 2);
            flags = 0;
            // quiet compiler warning
            if (flags == 0) { }
            this.guidScheme = guidScheme;
            this.identifier = identifier;
            this.nonCanonicalData = nonCanonicalData;
        }

        public Guid GuidScheme
        {
            get { return guidScheme; }
        }

        public String Identifier
        {
            get { return identifier; }
        }

        public String NonCanonicalData
        {
            get { return nonCanonicalData; }
        }

        int cbSize;
        int flags;
        Guid guidScheme;
        [MarshalAs(UnmanagedType.LPWStr)]
        String identifier;
        [MarshalAs(UnmanagedType.LPWStr)]
        String nonCanonicalData;
    }
}
