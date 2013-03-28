using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellGac
{
    public class InstallReference
    {
        public InstallReference(InstallReferenceType type, string identifier, string description)
        {
            Type = type;
            Identifier = identifier;
            Description = description;
        }

        public InstallReferenceType Type { get; private set; }
        public string Identifier { get; private set; }
        public string Description { get; private set; }

        public bool CanBeUsed()
        {
            return Type == InstallReferenceType.Installer || Type == InstallReferenceType.FilePath || Type == InstallReferenceType.Opaque;
        }
    }
}
