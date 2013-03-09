using System;
using System.Collections.Generic;
using System.Text;

// To support extension methods in .Net 2.0 compiler
// http://stackoverflow.com/questions/1522605/using-extension-methods-in-net-2-0
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}
