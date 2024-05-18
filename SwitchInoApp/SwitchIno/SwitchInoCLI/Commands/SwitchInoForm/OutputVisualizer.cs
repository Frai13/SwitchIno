using SwitchIno;
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
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1
{
    public partial class OutputVisualizer : UserControl
    {
        private OutputType Output = OutputType.UNSETTED;
        private bool State = false;

        public OutputVisualizer()
        {
            InitializeComponent();
        }

        public OutputVisualizer Init(OutputType output)
        {
            Output = output;
            State = Common.SwitchIno.GetOutput(Output);
            lbl_name.Text = Output.ToString();
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
            try
            {
                Common.SwitchIno.SetOutput(Output, !State);
                State = !State;
                ChangeUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting output value: {ex.Message}", "Error");
            }
        }
    }
}
