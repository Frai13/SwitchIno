using EasInoAPI;
using EasInoAPI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchIno.SwitchInoCLI
{
    internal class Connect : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-c", "--connect" };
        public IDictionary<string, string> Args => new Dictionary<string, string>()
        {
            { "Output", "Output to be connected. Use --list command to see all available outputs." }
        };
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Connect SwitchIno output";

        public Common.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            if (!Common.CheckArgsProvided(Args, ArgsProvided))
            {
                return;
            }

            string output = ArgsProvided.ElementAt(0);
            OutputType outputType = OutputType.UNSETTED;
            if (!Enum.TryParse<OutputType>(output, out outputType))
            {
                Console.WriteLine($"ERROR: output does not exists");
            }

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

            try
            {
                Common.SwitchIno.Start(sInoConfig);
                Common.SwitchIno.SetOutput(outputType, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }

            Console.WriteLine($"Successfully connected {output}.");
        };
    }
}
