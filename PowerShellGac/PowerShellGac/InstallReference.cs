﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    // TODO : Why StructLayout and MarshalAs?
    // Better display
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
}
