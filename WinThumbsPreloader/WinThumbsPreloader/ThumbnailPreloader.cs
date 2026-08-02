using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using static WinThumbsPreloader.Logger;

#nullable enable

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    sealed class ThumbnailPreloader
    {
        const int E_ACCESSDENIED = unchecked((int)0x80070005);
        const int RPC_E_DISCONNECTED = unchecked((int)0x80010108);

        private static readonly Guid CLSID_LocalThumbnailCache =
            new Guid("50ef4544-ac9f-4a8e-b21b-8a26180db13f");

        private static readonly Guid IID_IShellItem =
            new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");

        private static readonly Lazy<Type?> ThumbnailCacheComType = new(() =>
            Type.GetTypeFromCLSID(CLSID_LocalThumbnailCache, throwOnError: false));

        // Per-thread base IShellItem for SHCreateItemFromRelativeName (reused when base path matches)
        private static readonly ThreadLocal<(string? basePath, IShellItem? baseItem)> ThreadLocalBaseItem =
            new ThreadLocal<(string?, IShellItem?)>(() => (null, null));

        // One thumbnail cache instance per thread (reused across calls)
        private static readonly ThreadLocal<IThumbnailCache?> ThreadLocalCache =
            new ThreadLocal<IThumbnailCache?>(() =>
            {
                var type = ThumbnailCacheComType.Value;
                if (type == null) 
                    return null;

                try
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance == null)
                    {
                        WriteLine("Activator.CreateInstance returned null for IThumbnailCache", LoggingFrequency.DebugLogging);
                        return null;
                    }
                    return (IThumbnailCache)instance;
                }
                catch (Exception ex)
                {
                    WriteLine("Failed to create IThumbnailCache: " + ex.Message, LoggingFrequency.DebugLogging);
                    return null;
                }
            });

        /// <summary>
        /// Preloads thumbnails for a given file path and a set of sizes.
        /// </summary>
        public static void PreloadThumbnail(string filePath, uint[] sizes)
        {
            if (string.IsNullOrWhiteSpace(filePath) || sizes == null || sizes.Length == 0)
                return;

            IThumbnailCache? tbCache = ThreadLocalCache.Value;
            if (tbCache == null)
                return;

            if (!TryCreateShellItem(filePath, out IShellItem? shellItem, out int hrCreate))
            {
                if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                {
                    string msg = Marshal.GetExceptionForHR(hrCreate)?.Message ?? "Unknown COM error";
                    WriteLine($"SHCreateItemFromParsingName failed (0x{hrCreate:X8}) for '{filePath}': {msg}", LoggingFrequency.DebugLogging);
                }
                return;
            }

            // Prevents compiler null warning, shouldn't run as it's handled by TryCreateShellItem
            if (shellItem == null)
                return;

            try
            {
                foreach (var size in sizes)
                {
                    if (size <= 0)
                        continue;

                    // Extract and cache thumbnail
                    int hrThumb = tbCache.GetThumbnail(
                        shellItem,
                        size,
                        WTS_FLAGS.WTS_EXTRACTINPROC,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        IntPtr.Zero
                    );

                    if (hrThumb < 0)
                    {
                        if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                        {
                            WriteLine($"GetThumbnail failed (0x{hrThumb:X8}) on path {filePath} for size {size}", LoggingFrequency.DebugLogging);
                        }

                        // Fatal errors, don't continue trying other sizes
                        if (hrThumb == E_ACCESSDENIED || hrThumb == RPC_E_DISCONNECTED)
                            break;
                    }
                }
            }
            finally
            {
                if (shellItem != null)
                    Marshal.ReleaseComObject(shellItem);
            }
        }

        /// <summary>
        /// Preloads thumbnails using a base folder path and a relative file name.
        /// Each thread caches the base IShellItem so that only the relative name need to be resolved per file, 
        /// which is faster than parsing the full path and creating a new IShellItem for each file.
        /// </summary>
        /// <param name="basePath">The base folder parsing name (e.g. "C:\Users\Photos").</param>
        /// <param name="relativePath">The relative file path from the base folder (e.g. "subfolder\image.jpg").</param>
        /// <param name="sizes">The thumbnail sizes to preload.</param>
        public static void PreloadThumbnail(string basePath, string relativePath, uint[] sizes)
        {
            if (string.IsNullOrWhiteSpace(basePath) || string.IsNullOrWhiteSpace(relativePath) || sizes == null || sizes.Length == 0)
                return;

            IThumbnailCache? tbCache = ThreadLocalCache.Value;
            if (tbCache == null)
                return;

            IShellItem? baseItem = GetOrCreateBaseShellItem(basePath);
            if (baseItem == null)
                return;

            if (!TryCreateShellItemFromRelativeName(baseItem, relativePath, out IShellItem? shellItem, out int hrCreate))
            {
                if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                {
                    string msg = Marshal.GetExceptionForHR(hrCreate)?.Message ?? "Unknown COM error";
                    WriteLine($"SHCreateItemFromRelativeName failed (0x{hrCreate:X8}) for '{relativePath}': {msg}", LoggingFrequency.DebugLogging);
                }
                return;
            }

            if (shellItem == null)
                return;

            try
            {
                foreach (var size in sizes)
                {
                    if (size <= 0)
                        continue;

                    int hrThumb = tbCache.GetThumbnail(
                        shellItem,
                        size,
                        WTS_FLAGS.WTS_EXTRACTINPROC,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        IntPtr.Zero
                    );

                    if (hrThumb < 0)
                    {
                        if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                        {
                            WriteLine($"GetThumbnail failed (0x{hrThumb:X8}) on relative path {relativePath} for size {size}", LoggingFrequency.DebugLogging);
                        }

                        if (hrThumb == E_ACCESSDENIED || hrThumb == RPC_E_DISCONNECTED)
                            break;
                    }
                }
            }
            finally
            {
                if (shellItem != null)
                    Marshal.ReleaseComObject(shellItem);
            }
        }
            
        private static bool TryCreateShellItem(string filePath, out IShellItem? shellItem, out int hr)
        {
            shellItem = null;
            hr = SHCreateItemFromParsingName(filePath, IntPtr.Zero, IID_IShellItem, out shellItem);
            return hr >= 0 && shellItem != null;
        }

        private static bool TryCreateShellItemFromRelativeName(IShellItem baseItem, string relativePath, out IShellItem? shellItem, out int hr)
        {
            shellItem = null;
            hr = SHCreateItemFromRelativeName(baseItem, relativePath, IntPtr.Zero, IID_IShellItem, out shellItem);
            return hr >= 0 && shellItem != null;
        }

        /// <summary>
        /// Returns (and caches per-thread) a base IShellItem for the given folder path.
        /// If the base path changes, the previous base item is released and a new one is created.
        /// </summary>
        private static IShellItem? GetOrCreateBaseShellItem(string basePath)
        {
            var (currentPath, currentItem) = ThreadLocalBaseItem.Value;

            if (string.Equals(currentPath, basePath, StringComparison.OrdinalIgnoreCase) && currentItem != null)
                return currentItem;

            if (currentItem != null)
            {
                try { Marshal.ReleaseComObject(currentItem); } catch { }
            }

            if (!TryCreateShellItem(basePath, out IShellItem? newItem, out int hr))
            {
                if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                {
                    string msg = Marshal.GetExceptionForHR(hr)?.Message ?? "Unknown COM error";
                    WriteLine($"SHCreateItemFromParsingName failed for base path (0x{hr:X8}) '{basePath}': {msg}", LoggingFrequency.DebugLogging);
                }
                ThreadLocalBaseItem.Value = (basePath, null);
                return null;
            }

            ThreadLocalBaseItem.Value = (basePath, newItem);
            return newItem;
        }

        // Import native functions
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHCreateItemFromParsingName(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            [In] IntPtr pbc,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem? ppv);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHCreateItemFromRelativeName(
            [In][MarshalAs(UnmanagedType.Interface)] IShellItem psiParent,
            [In][MarshalAs(UnmanagedType.LPWStr)] string pszName,
            [In] IntPtr pbc,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out][MarshalAs(UnmanagedType.Interface)] out IShellItem? ppv);

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("F676C15D-596A-4ce2-8234-33996F445DB1")]
        private interface IThumbnailCache
        {
            [PreserveSig]
            int GetThumbnail(
                [In] IShellItem pShellItem,
                [In] uint cxyRequestedThumbSize,
                [In] WTS_FLAGS flags,
                [In] IntPtr ppvThumb,
                [In] IntPtr pOutFlags,
                [In] IntPtr pThumbnailID
            );
        }

        [Flags]
        private enum WTS_FLAGS : uint
        {
            WTS_EXTRACT = 0x00000000,
            WTS_INCACHEONLY = 0x00000001,
            WTS_FASTEXTRACT = 0x00000002,
            WTS_FORCEEXTRACTION = 0x00000004,
            WTS_SLOWRECLAIM = 0x00000008,
            WTS_EXTRACTDONOTCACHE = 0x00000020,
            WTS_SCALETOREQUESTEDSIZE = 0x00000040,
            WTS_SKIPFASTEXTRACT = 0x00000080,
            WTS_EXTRACTINPROC = 0x00000100
        }

        [Flags]
        private enum WTS_CACHEFLAGS : uint
        {
            WTS_DEFAULT = 0x00000000,
            WTS_LOWQUALITY = 0x00000001,
            WTS_CACHED = 0x00000002
        }

        [InlineArray(16)]
        private struct Byte16
        {
            private byte _element0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_THUMBNAILID
        {
            public Byte16 rgbKey;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
        public interface IShellItem
        {
            void BindToHandler(
                IntPtr pbc,
                [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
                [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
                out IntPtr ppv);

            void GetParent(out IShellItem ppsi);

            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);

            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);

            void Compare(IShellItem psi, uint hint, out int piOrder);
        }

        public enum SIGDN : uint
        {
            NORMALDISPLAY = 0,
            PARENTRELATIVEPARSING = 0x80018001,
            PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
            DESKTOPABSOLUTEPARSING = 0x80028000,
            PARENTRELATIVEEDITING = 0x80031001,
            DESKTOPABSOLUTEEDITING = 0x8004c000,
            FILESYSPATH = 0x80058000,
            URL = 0x80068000
        }

        [ComImport]
        [Guid("091162a4-bc96-411f-aae8-c5122cd03363")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ISharedBitmap
        {
            uint Detach([Out] out IntPtr phbm);
            uint GetFormat([Out] out WTS_ALPHATYPE pat);
            uint GetSharedBitmap([Out] out IntPtr phbm);
            uint GetSize([Out, MarshalAs(UnmanagedType.Struct)] out SIZE pSize);
            uint InitializeBitmap([In] IntPtr hbm, [In] WTS_ALPHATYPE wtsAT);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;

            public SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        public enum WTS_ALPHATYPE : uint
        {
            WTSAT_UNKNOWN = 0,
            WTSAT_RGB = 1,
            WTSAT_ARGB = 2
        }
    }
}