using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetrapol.SdrSharp
{
    internal sealed class TetrapolPanel : UserControl
    {
        private readonly ComboBox _bandCombo = new ComboBox();
        private readonly ComboBox _channelCombo = new ComboBox();
        private readonly NumericUpDown _scrConfidence = new NumericUpDown();
        private readonly NumericUpDown _scrValue = new NumericUpDown();
        private readonly Button _toggleButton = new Button();
        private readonly Label _statusLabel = new Label();
        private readonly TextBox _logBox = new TextBox();

        public TetrapolPanel()
        {
            Dock = DockStyle.Fill;

            _bandCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _bandCombo.Items.AddRange(new object[] { "UHF", "VHF" });
            _bandCombo.SelectedIndex = 0;

            _channelCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _channelCombo.Items.AddRange(new object[] { "Control channel", "Traffic channel" });
            _channelCombo.SelectedIndex = 0;

            _scrConfidence.Minimum = 1;
            _scrConfidence.Maximum = 200;
            _scrConfidence.Value = 50;

            _scrValue.Minimum = -1;
            _scrValue.Maximum = 126;
            _scrValue.Value = -1;

            _toggleButton.Text = "Start";
            _toggleButton.AutoSize = true;

            _statusLabel.AutoSize = true;
            _statusLabel.Text = "Idle";

            _logBox.Multiline = true;
            _logBox.ScrollBars = ScrollBars.Vertical;
            _logBox.ReadOnly = true;
            _logBox.Dock = DockStyle.Fill;

            var settings = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                Padding = new Padding(4)
            };

            settings.Controls.Add(new Label { Text = "Band", AutoSize = true }, 0, 0);
            settings.Controls.Add(_bandCombo, 1, 0);
            settings.Controls.Add(new Label { Text = "Channel type", AutoSize = true }, 0, 1);
            settings.Controls.Add(_channelCombo, 1, 1);
            settings.Controls.Add(new Label { Text = "SCR confidence", AutoSize = true }, 0, 2);
            settings.Controls.Add(_scrConfidence, 1, 2);
            settings.Controls.Add(new Label { Text = "SCR (-1 auto)", AutoSize = true }, 0, 3);
            settings.Controls.Add(_scrValue, 1, 3);
            settings.Controls.Add(_toggleButton, 0, 4);
            settings.Controls.Add(_statusLabel, 1, 4);

            Controls.Add(_logBox);
            Controls.Add(settings);
        }

        public event EventHandler ToggleRequested;

        public int SelectedBand => _bandCombo.SelectedIndex == 0
            ? TetrapolNative.TetrapolBandUhf
            : TetrapolNative.TetrapolBandVhf;

        public int SelectedRadioChannelType => _channelCombo.SelectedIndex == 0
            ? TetrapolNative.RadioChannelTypeControl
            : TetrapolNative.RadioChannelTypeTraffic;

        public int ScrConfidence => (int)_scrConfidence.Value;
        public int ScrValue => (int)_scrValue.Value;

        public void AppendLog(string line)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendLog), line);
                return;
            }

            _logBox.AppendText(line + Environment.NewLine);
        }

        public void SetRunning(bool running, int scr)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool, int>(SetRunning), running, scr);
                return;
            }

            _toggleButton.Text = running ? "Stop" : "Start";
            _statusLabel.Text = running
                ? $"Running (SCR={scr})"
                : "Idle";
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _toggleButton.Click += (_, __) => ToggleRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
