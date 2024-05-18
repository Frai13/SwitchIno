using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace SwitchInoApp
{
    public enum GpioType { UNSETTED, NO1, NO2, NO3, NO4, NC1, NC2, NC3, NC4 }

    internal class SwitchIno
    {
        private EasIno Easino;
        private bool Connected = false;

        public SwitchIno(GenericConfiguration config)
        {
            if (config.ComType == GenericConfiguration.CommunicationType.SERIAL)
            {
                Easino = new SerialCom((SerialComConfiguration)config);
            }
            else
            {
                throw new Exception("Error: ComType does not exist");
            }
        }

        public void Start()
        {
            Easino.Start();
            Connected = true;
        }

        public void Stop()
        {
            Easino.Stop();
            Connected = false;
        }

        public string GetSwitchInoVersion()
        {
            Easino.Send(new DataCom()
            {
                Operation = "SI_VER",
                Args = new List<string>()
            });

            DataCom dataReceive = Easino.Receive();
            return dataReceive.Args.ElementAt(0);
        }

        public void SetGpio(GpioType gpio, bool value)
        {
            string value_send = value ? "1" : "0";
            Easino.Send(new DataCom()
            {
                Operation = "SET",
                Args = new List<string>() { $"{gpio}", value_send }
            });

            DataCom dataReceive = Easino.Receive();
            string gpio_recv = dataReceive.Args.ElementAt(0);
            string value_recv = dataReceive.Args.ElementAt(1);

            if (dataReceive.Operation != "SET")
            {
                throw new Exception("Error: response does not match request");
            }

            if (gpio_recv != $"{gpio}")
            {
                throw new Exception("Error: could not set GPIO");
            }

            if (value_recv != value_send)
            {
                throw new Exception("Error: wrong value");
            }
        }

        public bool GetGpio(GpioType gpio)
        {
            Easino.Send(new DataCom()
            {
                Operation = "GET",
                Args = new List<string>() { $"{gpio}" }
            });

            DataCom dataReceive = Easino.Receive();
            string gpio_recv = dataReceive.Args.ElementAt(0);
            string value_recv = dataReceive.Args.ElementAt(1);

            if (dataReceive.Operation != "GET")
            {
                throw new Exception("Error: response does not match request");
            }

            if (gpio_recv != $"{gpio}")
            {
                throw new Exception("Error: could not get GPIO");
            }

            if (value_recv != "0" && value_recv != "1")
            {
                throw new Exception("Error: wrong value");
            }

            return int.Parse(value_recv) == 1;
        }
    }
}
