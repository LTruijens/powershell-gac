using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PowerShellGac
{
    internal static class FusionApi
    {
        internal static CreateAssemblyEnumMethod CreateAssemblyEnum { get; private set; }

        internal static CreateAssemblyNameObjectMethod CreateAssemblyNameObject { get; private set; }

        internal static CreateAssemblyCacheMethod CreateAssemblyCache { get; private set; }

        internal static CreateInstallReferenceEnumMethod CreateInstallReferenceEnum { get; private set; }

        internal static GetCachePathMethod GetCachePath { get; private set; } 

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate int CreateAssemblyEnumMethod(
                out IAssemblyEnum ppEnum,
                IntPtr pUnkReserved,
                IAssemblyName pName,
                AssemblyCacheFlags flags,
                IntPtr pvReserved);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate int CreateAssemblyNameObjectMethod(
                out IAssemblyName ppAssemblyNameObj,
                [MarshalAs(UnmanagedType.LPWStr)]
                 String szAssemblyName,
                CreateAssemblyNameObjectFlags flags,
                IntPtr pvReserved);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate int CreateAssemblyCacheMethod(
                out IAssemblyCache ppAsmCache,
                int reserved);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate int CreateInstallReferenceEnumMethod(
                out IInstallReferenceEnum ppRefEnum,
                IAssemblyName pName,
                int dwFlags,
                IntPtr pvReserved);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate int GetCachePathMethod(
            AssemblyCacheFlags assemblyCacheFlags,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder cachePath,
            ref int cachePathSize);
        
        static FusionApi()
        {
            var path = GetLatestFrameworkPath();

            Initialize(path);
        }

        private static string GetLatestFrameworkPath()
        {
            RegistryKey netFramework = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NetFramework", false);

            string installRoot = netFramework.GetValue("InstallRoot").ToString();

            string path = Path.Combine(installRoot, "v4.0.30319");
            if (!Directory.Exists(path))
            {
                path = Path.Combine(installRoot, "v2.0.50727");
            }

            return path;
        }

        private static void Initialize(string path)
        {
            IntPtr dll = Win32Check(NativeMethods.LoadLibrary(Path.Combine(path, "fusion.dll")));

            CreateAssemblyEnum = ImportNativeMethod<CreateAssemblyEnumMethod>(dll, "CreateAssemblyEnum");
            CreateAssemblyNameObject = ImportNativeMethod<CreateAssemblyNameObjectMethod>(dll, "CreateAssemblyNameObject");
            CreateAssemblyCache = ImportNativeMethod<CreateAssemblyCacheMethod>(dll, "CreateAssemblyCache");
            CreateInstallReferenceEnum = ImportNativeMethod<CreateInstallReferenceEnumMethod>(dll, "CreateInstallReferenceEnum");
            GetCachePath = ImportNativeMethod<GetCachePathMethod>(dll, "GetCachePath");
        }

        private static IntPtr Win32Check(IntPtr result)
        {
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return result;
        }

        private static T ImportNativeMethod<T>(IntPtr dll, string name) where T : class
        {
            IntPtr function = Win32Check(NativeMethods.GetProcAddress(dll, name));
            return Marshal.GetDelegateForFunctionPointer(function, typeof(T)) as T;
        }
    }
}
