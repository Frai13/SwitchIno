using SwitchInoApp;
using SwitchInoApp.SwitchInoAppCLI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1
{
    public partial class OutputVisualizer : UserControl
    {
        private GpioType Gpio = GpioType.UNSETTED;
        private bool State = false;

        public OutputVisualizer()
        {
            InitializeComponent();
        }

        public OutputVisualizer Init(GpioType gpio)
        {
            Gpio = gpio;
            State = Common.SwitchIno.GetGpio(Gpio);
            lbl_name.Text = Gpio.ToString();
            ChangeUi();

            return this;
        }

        private void ChangeUi()
        {
            lbl_state.Text = State ? "CONN." : "DISCON.";
            lbl_state.BackColor = State ? Color.Green : Color.Red;
        }

        private void btn_change_Click(object sender, EventArgs e)
        {
            Common.SwitchIno.SetGpio(Gpio, !State);
            State = !State;
            ChangeUi();
        }
    }
}
