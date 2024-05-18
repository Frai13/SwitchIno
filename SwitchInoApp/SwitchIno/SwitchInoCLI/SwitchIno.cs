using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace SwitchIno.SwitchInoCLI
{
    public enum OutputType { UNSETTED, NO1, NO2, NO3, NO4, NC1, NC2, NC3, NC4 }

    internal class SwitchIno
    {
        private EasIno Easino;
        private SwitchInoConfig Config;

        private bool connected = false;
        public bool Connected
        {
            get { return connected; }
        }


        public SwitchIno() { }

        public void Start(SwitchInoConfig config)
        {
            Config = config;
            connected = false;

            if (Config.Method == PortConnectMethod.FIX)
            {
                Connect();
            }
            else if (Config.Method == PortConnectMethod.AUTO)
            {
                List<string> availablePorts = System.IO.Ports.SerialPort.GetPortNames().OrderByDescending(a => a).ToList();

                string defaultComPort = ((SerialComConfiguration)Config.Configuration).ComPort;
                if (availablePorts.Contains(defaultComPort))
                {
                    availablePorts.Remove(defaultComPort);
                    availablePorts.Insert(0, defaultComPort);
                }

                foreach (var port in availablePorts)
                {
                    try
                    {
                        ((SerialComConfiguration)Config.Configuration).ComPort = port;
                        Connect();
                        break;
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                throw new Exception("Error not supported configuration Type");
            }
        }

        private void Connect()
        {
            if (Config.Configuration.ComType == GenericConfiguration.CommunicationType.SERIAL)
            {
                Easino = new SerialCom((SerialComConfiguration)Config.Configuration);
                Easino.Start();

                System.Threading.Thread.Sleep(500);

                GetSwitchInoVersion();
            }
            else
            {
                Easino.Stop();
                throw new Exception("Error not supported communication Type");
            }
            connected = true;
        }

        public void Stop()
        {
            Easino.Stop();
            connected = false;
        }

        public string GetSwitchInoVersion()
        {
            DataCom dataReceive = Easino.TrySendAndReceive(new DataCom()
            {
                Operation = "SI_VER",
                Args = new List<string>()
            }, 3);

            return dataReceive.Args.ElementAt(0);
        }

        public string GetEasinoVersion()
        {
            DataCom dataReceive = Easino.TrySendAndReceive(new DataCom()
            {
                Operation = "VERSION",
                Args = new List<string>()
            }, 3);

            return dataReceive.Args.ElementAt(0);
        }

        public void SetOutput(OutputType output, bool value)
        {
            string value_send = value ? "1" : "0";
            DataCom dataReceive = Easino.TrySendAndReceive(new DataCom()
            {
                Operation = "SET",
                Args = new List<string>() { $"{output}", value_send }
            }, 3);

            string output_recv = dataReceive.Args.ElementAt(0);
            string value_recv = dataReceive.Args.ElementAt(1);

            if (dataReceive.Operation != "SET")
            {
                throw new Exception("Error: response does not match request");
            }

            if (output_recv != $"{output}")
            {
                throw new Exception("Error: could not set output");
            }

            if (value_recv != value_send)
            {
                throw new Exception("Error: wrong value");
            }
        }

        public bool GetOutput(OutputType output)
        {
            DataCom dataReceive = Easino.TrySendAndReceive(new DataCom()
            {
                Operation = "GET",
                Args = new List<string>() { $"{output}" }
            }, 3);

            string output_recv = dataReceive.Args.ElementAt(0);
            string value_recv = dataReceive.Args.ElementAt(1);

            if (dataReceive.Operation != "GET")
            {
                throw new Exception("Error: response does not match request");
            }

            if (output_recv != $"{output}")
            {
                throw new Exception("Error: could not get output");
            }

            if (value_recv != "0" && value_recv != "1")
            {
                throw new Exception("Error: wrong value");
            }

            return int.Parse(value_recv) == 1;
        }
    }
}
