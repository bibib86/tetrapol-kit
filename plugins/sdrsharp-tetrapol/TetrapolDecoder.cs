using System;
using System.Runtime.InteropServices;

namespace Tetrapol.SdrSharp
{
    internal sealed class TetrapolDecoder : IDisposable
    {
        private readonly TetrapolNative.LogCallback _nativeLogCallback;
        private IntPtr _decoderHandle;
        private bool _disposed;

        public TetrapolDecoder(Action<string> logSink)
        {
            _nativeLogCallback = (_, message, __) =>
            {
                if (message != IntPtr.Zero)
                {
                    logSink?.Invoke(Marshal.PtrToStringAnsi(message) ?? string.Empty);
                }
            };

            TetrapolNative.tetrapol_log_set_callback(_nativeLogCallback, IntPtr.Zero);
        }

        public bool IsRunning => _decoderHandle != IntPtr.Zero;

        public int ScramblingConstant => IsRunning
            ? TetrapolNative.tetrapol_decoder_get_scr(_decoderHandle)
            : TetrapolNative.ScrDetect;

        public void Start(int band, int radioChannelType, int scrConfidence, int scr)
        {
            Stop();

            _decoderHandle = TetrapolNative.tetrapol_decoder_create(band, radioChannelType);
            if (_decoderHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create the native TETRAPOL decoder.");
            }

            TetrapolNative.tetrapol_decoder_set_scr_confidence(_decoderHandle, scrConfidence);
            TetrapolNative.tetrapol_decoder_set_scr(_decoderHandle, scr);
        }

        public void Stop()
        {
            if (_decoderHandle == IntPtr.Zero)
            {
                return;
            }

            TetrapolNative.tetrapol_decoder_destroy(_decoderHandle);
            _decoderHandle = IntPtr.Zero;
        }

        public void PushBits(byte[] bits, int length)
        {
            if (_decoderHandle == IntPtr.Zero || bits == null || length <= 0)
            {
                return;
            }

            TetrapolNative.tetrapol_decoder_feed_bits(_decoderHandle, bits, length);
            TetrapolNative.tetrapol_decoder_process(_decoderHandle);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Stop();
            _disposed = true;
        }
    }
}
