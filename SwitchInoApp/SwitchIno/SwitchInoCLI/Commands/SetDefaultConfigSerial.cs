using EasInoAPI;
using EasInoAPI.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SwitchIno.SwitchInoCLI
{
    internal class SetDefaultConfigSerial : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-sdcs", "--setdefaultconfigserial" };
        public IDictionary<string, string> Args => new Dictionary<string, string>()
        {
            { "Method", "Connection method to be used. Typical values are: AUTO and FIX" },
            { "Port", "Name of the serial port" },
            { "BaudRate", "Baud rate to be used. Typical values are: 9600 and 115200" },
            { "Parity", "Parity to be used. Typical values are: Odd, None and Even" },
            { "DataBits", "DataBits to be used. Typical values are: 7 and 8" },
            { "StopBits", "StopBits to be used. Typical values are: None, One and Two" },
        };
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>()
        {
            { "Timeout", "Timeout of the response received" }
        };

        public string Description => "Set serial communication default configuration";

        public Common.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            if (!Common.CheckArgsProvided(Args, ArgsProvided))
            {
                return;
            }

            SwitchInoConfig sInoConfig;
            try
            {
                sInoConfig = new SwitchInoConfig(GenericConfiguration.CommunicationType.SERIAL, ArgsProvided);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }

            sInoConfig.Serialize();

            Console.WriteLine($"{sInoConfig}");
            Console.WriteLine($"Serial configuration set to default successfully");
        };
    }
}
