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
    internal class GetDefaultConfig : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-gdc", "--getdefaultconfig" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Get communication default configuration";

        public Common.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            SwitchInoConfig sInoConfig = new SwitchInoConfig();
            try
            {
                sInoConfig = sInoConfig.Deserialize();
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: no default configuration saved");
                return;
            }

            Console.WriteLine($"{sInoConfig}");
            Console.WriteLine($"Default configuration read successfully");
        };
    }
}
