using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Diagnostics;

namespace BarcodeGen
{
    public partial class SettingsPane : UserControl
    {
        internal Settings.ForWriting ControlForSettings { get; private set; }

        public SettingsPane()
        {
            InitializeComponent();
        }

        private void SettingsPane_Load(object sender, EventArgs e)
        {
            var ehost = new ElementHost { Dock = DockStyle.Fill };
            ControlForSettings = new Settings.ForWriting();
            ehost.Child = ControlForSettings; 
            Controls.Add(ehost);
            var vm = ControlForSettings.DataContext as Settings.ForWritingViewModel;
            vm.ApplyCommand.Subscribe(_ => Debug.WriteLine(""));
        }
    }
}
