using System;
using System.Runtime.InteropServices;

namespace Tetrapol.SdrSharp
{
    internal static class TetrapolNative
    {
        private const string DllName = "tetrapol.dll";

        internal const int TetrapolBandVhf = 1;
        internal const int TetrapolBandUhf = 2;
        internal const int RadioChannelTypeControl = 1;
        internal const int RadioChannelTypeTraffic = 2;
        internal const int ScrDetect = -1;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void LogCallback(int level, IntPtr message, IntPtr userData);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr tetrapol_decoder_create(int band, int radioChannelType);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void tetrapol_decoder_destroy(IntPtr decoder);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int tetrapol_decoder_feed_bits(IntPtr decoder, byte[] bits, int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int tetrapol_decoder_process(IntPtr decoder);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void tetrapol_decoder_set_scr(IntPtr decoder, int scr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int tetrapol_decoder_get_scr(IntPtr decoder);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void tetrapol_decoder_set_scr_confidence(IntPtr decoder, int scrConfidence);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int tetrapol_decoder_get_scr_confidence(IntPtr decoder);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void tetrapol_log_set_callback(LogCallback callback, IntPtr userData);
    }
}
