using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasInoAPI;
using EasInoAPI.Configuration;
using SwitchIno.SwitchInoCLI;

namespace SwitchIno.SwitchInoCLI
{
    public partial class ConfigurationWindow : Form
    {
        private SwitchInoConfig sInoConfig;
        public bool ConnectOnLoad = false;

        public ConfigurationWindow(bool connectOnLoad)
        {
            InitializeComponent();
            ConnectOnLoad = connectOnLoad;
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            UpdateSwitchInoVersion(false);

            sInoConfig = new SwitchInoConfig();
            sInoConfig = sInoConfig.Deserialize();
            int index = 0;
            foreach (var item in GetConfigs())
            {
                cb_config.Items.Add(item.GetType().Name);
                if (item.GetType().Name == sInoConfig.Configuration.ComType.GetType().Name)
                {
                    index = cb_config.Items.Count-1;
                }
            }
            cb_config.SelectedIndex = index;

            if (Common.SwitchIno.Connected)
            {
                UpdateSwitchInoVersion(true);
            }

            if (ConnectOnLoad)
            {
                Connect();
                if (Common.SwitchIno.Connected)
                {
                    this.Close();
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            Connect();
            
            if (Common.SwitchIno.Connected)
            {
                UpdateSwitchInoVersion(true);
                MessageBox.Show($"Successfully connected to EasIno board");
                this.Close();
            }
        }

        private void Connect()
        {
            try
            {
                if (Common.SwitchIno.Connected)
                {
                    Common.SwitchIno.Stop();
                }
            } catch (Exception) { }

            try
            {
                GenericConfiguration config = GetConfigs().Where(c => c.GetType().Name == cb_config.Text).FirstOrDefault();
                if (config.GetType() == typeof(SerialComConfiguration))
                {
                    SerialComConfiguration serialConfig = new SerialComConfiguration()
                    {
                        ComPort = GetPropValue("ComPort"),
                        BaudRate = Convert.ToInt32(GetPropValue("BaudRate")),
                        Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), GetPropValue("Parity")),
                        DataBits = Convert.ToInt32(GetPropValue("DataBits")),
                        StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), GetPropValue("StopBits")),
                        Timeout = Convert.ToInt32(GetPropValue("Timeout"))
                    };
                    PortConnectMethod method = (PortConnectMethod)Enum.Parse(typeof(PortConnectMethod), GetPropValue("PortConnectMethod"));

                    SwitchInoConfig sInoConfig = new SwitchInoConfig(serialConfig, method);
                    Common.SwitchIno.Start(sInoConfig);

                    sInoConfig.Serialize();
                }
            }
            catch (Exception ex)
            {
                Common.SwitchIno.Stop();
                MessageBox.Show($"Error while trying to connect: {ex.Message}", "Error");
            }
        }

        private void UpdateSwitchInoVersion(bool getBoardVersion)
        {
            string boardVersion = "????";
            string switchinoVersion = "????";
            if (getBoardVersion)
            {
                boardVersion = Common.SwitchIno.GetEasinoVersion();
                switchinoVersion = Common.SwitchIno.GetSwitchInoVersion();
            }

            lbl_easino.Text = $"Easino: API: [ {EasIno.GetVersion()} ] Board: [ {boardVersion} ]";
            lbl_switchino.Text = $"SwitchIno: API: [ {Common.Version} ] Board: [ {switchinoVersion} ]";
        }

        #region UI
        private void dgv_properties_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private static IEnumerable<GenericConfiguration> GetConfigs()
        {
            var type = typeof(GenericConfiguration);
            IEnumerable<GenericConfiguration> configsNull = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type)
                .Select(s => (GenericConfiguration)Activator.CreateInstance(s));

            IList<GenericConfiguration> configs = new List<GenericConfiguration>();
            foreach (var config in configsNull)
            {
                if (config != null) configs.Add(config);
            }
            return configs;
        }

        private void AddComboBoxCell(string propName, string defaultValue, IEnumerable<string> values)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell cell_tb = new DataGridViewTextBoxCell();
            cell_tb.Value = propName;
            DataGridViewComboBoxCell cell_cb = new DataGridViewComboBoxCell();
            foreach (var v in values)
            {
                cell_cb.Items.Add(v.ToString());
            }
            cell_cb.Value = defaultValue == "" ? cell_cb.Items[0] : defaultValue;
            row.Cells.Add(cell_tb);
            row.Cells.Add(cell_cb);
            dgv_properties.Rows.Add(row);
        }

        private void cb_config_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_properties.Rows.Clear();
            GenericConfiguration config = GetConfigs().Where(c => c.GetType().Name == cb_config.Text).FirstOrDefault();
            if (config.GetType() == typeof(SerialComConfiguration))
            {
                if (sInoConfig.Configuration.GetType() == typeof(GenericConfiguration))
                {
                    sInoConfig.Configuration = new SerialComConfiguration();
                }

                SerialComConfiguration serialConf = (SerialComConfiguration)sInoConfig.Configuration;
                AddComboBoxCell("PortConnectMethod", sInoConfig.Method.ToString(), Enum.GetNames(typeof(PortConnectMethod)));
                AddComboBoxCell("ComPort", serialConf.ComPort, System.IO.Ports.SerialPort.GetPortNames().OrderByDescending(a => a));
                dgv_properties.Rows.Add("BaudRate", serialConf.BaudRate);
                AddComboBoxCell("Parity", serialConf.Parity.ToString(), Enum.GetValues(serialConf.Parity.GetType()).OfType<System.IO.Ports.Parity>().Select(a => a.ToString()));
                dgv_properties.Rows.Add("DataBits", serialConf.DataBits);
                AddComboBoxCell("StopBits", serialConf.StopBits.ToString(), Enum.GetValues(serialConf.StopBits.GetType()).OfType<System.IO.Ports.StopBits>().Select(a => a.ToString()));
                dgv_properties.Rows.Add("Timeout", serialConf.Timeout);
            }
        }

        private string GetPropValue(string prop)
        {
            DataGridViewRow row = dgv_properties.Rows.OfType<DataGridViewRow>().Where(r => prop == r.Cells[0].Value.ToString()).FirstOrDefault();
            return row.Cells[1].Value.ToString();
        }
        #endregion
    }
}
