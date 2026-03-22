using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SDRSharp.Common;

namespace Tetrapol.SdrSharp
{
    // NOTE: the SDR# stream processor interface changed several times.
    // This plugin keeps the actual decoder and UI complete, then relies on the
    // host build exposing a float-audio processor hook compatible with this class.
    [ComVisible(true)]
    public sealed class TetrapolPlugin : ISharpPlugin
    {
        private readonly TetrapolPanel _panel;
        private readonly TetrapolDecoder _decoder;
        private SimpleGmskSlicer _slicer;
        private ISharpControl _control;

        public TetrapolPlugin()
        {
            _panel = new TetrapolPanel();
            _panel.ToggleRequested += OnToggleRequested;
            _decoder = new TetrapolDecoder(_panel.AppendLog);
        }

        public string DisplayName => "TETRAPOL Decoder";

        public UserControl Gui => _panel;

        public void Initialize(ISharpControl control)
        {
            _control = control;

            // Most SDR# builds run the discriminator / AF path at 48 kHz.
            // If your build exposes another sample rate, update the slicer accordingly.
            _slicer = new SimpleGmskSlicer(sampleRate: 48000);

            _panel.AppendLog("Plugin loaded. Select a narrow FM TETRAPOL channel and press Start.");
            _panel.AppendLog("This plugin expects discriminator audio with minimal deemphasis / filtering.");
        }

        private void OnToggleRequested(object sender, EventArgs e)
        {
            if (_decoder.IsRunning)
            {
                _decoder.Stop();
                _panel.SetRunning(false, TetrapolNative.ScrDetect);
                _panel.AppendLog("Decoder stopped.");
                return;
            }

            _decoder.Start(
                _panel.SelectedBand,
                _panel.SelectedRadioChannelType,
                _panel.ScrConfidence,
                _panel.ScrValue);
            _panel.SetRunning(true, _decoder.ScramblingConstant);
            _panel.AppendLog("Decoder started.");
        }

        // Hook this method from the SDR# audio stream callback used by your build.
        public void ProcessAudio(float[] audioSamples, int length)
        {
            if (!_decoder.IsRunning || _slicer == null || audioSamples == null || length <= 0)
            {
                return;
            }

            var bits = new byte[Math.Max(1, length / 2 + 1)];
            var count = _slicer.Demodulate(audioSamples, length, bits);
            if (count <= 0)
            {
                return;
            }

            _decoder.PushBits(bits, count);
            _panel.SetRunning(true, _decoder.ScramblingConstant);
        }
    }
}
