using System;

namespace Tetrapol.SdrSharp
{
    internal sealed class SimpleGmskSlicer
    {
        private readonly int _samplesPerSymbol;
        private readonly float _gain;
        private float _integrator;
        private int _sampleCursor;

        public SimpleGmskSlicer(int sampleRate, int symbolRate = 8000)
        {
            _samplesPerSymbol = Math.Max(1, sampleRate / symbolRate);
            _gain = 1.0f / _samplesPerSymbol;
        }

        public int Demodulate(float[] samples, int length, byte[] output)
        {
            var written = 0;
            for (var i = 0; i < length; i++)
            {
                _integrator += samples[i];
                _sampleCursor++;
                if (_sampleCursor < _samplesPerSymbol)
                {
                    continue;
                }

                output[written++] = _integrator >= 0 ? (byte)1 : (byte)0;
                _integrator = 0;
                _sampleCursor = 0;
            }

            return written;
        }
    }
}
