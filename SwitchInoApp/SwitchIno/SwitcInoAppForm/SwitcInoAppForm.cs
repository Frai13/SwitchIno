using EasInoAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace SwitchInoApp
{
    internal partial class SwitcInoAppForm : Form
    {
        Dictionary<GpioType, OutputVisualizer> outputs;

        public SwitcInoAppForm()
        {
            InitializeComponent();
        }

        private void SwitcInoAppForm_Load(object sender, EventArgs e)
        {
            if (!OpenEasIno())
            {
                this.Close();
            }

            outputs = new Dictionary<GpioType, OutputVisualizer>()
            {
                { GpioType.NC1, out1.Init(GpioType.NC1) },
                { GpioType.NC2, out2.Init(GpioType.NC2) },
                { GpioType.NC3, out3.Init(GpioType.NC3) },
                { GpioType.NC4, out4.Init(GpioType.NC4) },
                { GpioType.NO1, out5.Init(GpioType.NO1) },
                { GpioType.NO2, out6.Init(GpioType.NO2) },
                { GpioType.NO3, out7.Init(GpioType.NO3) },
                { GpioType.NO4, out8.Init(GpioType.NO4) }
            };
        }

        private bool OpenEasIno()
        {
            ConfigurationWindow configWdw = new ConfigurationWindow();
            configWdw.ShowDialog();
            return configWdw.Connected;
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            OpenEasIno();
        }
    }
}
