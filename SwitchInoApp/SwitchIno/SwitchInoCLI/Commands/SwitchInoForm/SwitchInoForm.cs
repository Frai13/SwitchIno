using EasInoAPI;
using SwitchIno.SwitchInoCLI;
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

namespace SwitchIno.SwitchInoCLI
{
    internal partial class SwitchInoForm : Form
    {
        Dictionary<OutputType, OutputVisualizer> outputs;

        public SwitchInoForm()
        {
            InitializeComponent();
        }

        private void SwitchInoForm_Load(object sender, EventArgs e)
        {
            if (!OpenEasIno(true))
            {
                this.Close();
                return;
            }
        }

        private bool OpenEasIno(bool connect)
        {
            ConfigurationWindow configWdw = new ConfigurationWindow(connect);
            configWdw.ShowDialog();

            if (!Common.SwitchIno.Connected)
            {
                return false;
            }

            try
            {
                outputs = new Dictionary<OutputType, OutputVisualizer>()
                {
                    { OutputType.NC1, out1.Init(OutputType.NC1) },
                    { OutputType.NC2, out2.Init(OutputType.NC2) },
                    { OutputType.NC3, out3.Init(OutputType.NC3) },
                    { OutputType.NC4, out4.Init(OutputType.NC4) },
                    { OutputType.NO1, out5.Init(OutputType.NO1) },
                    { OutputType.NO2, out6.Init(OutputType.NO2) },
                    { OutputType.NO3, out7.Init(OutputType.NO3) },
                    { OutputType.NO4, out8.Init(OutputType.NO4) }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting output value: {ex.Message}", "Error");
                return false;
            }

            return Common.SwitchIno.Connected;
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            OpenEasIno(false);
        }
    }
}
