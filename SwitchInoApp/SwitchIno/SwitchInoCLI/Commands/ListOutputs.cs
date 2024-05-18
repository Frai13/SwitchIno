using EasInoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchIno.SwitchInoCLI
{
    internal class ListOutputs : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-l", "--list" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "List all available outputs";

        public Common.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            Console.WriteLine($"Available outputs list:");
            foreach (var item in Enum.GetValues(typeof(OutputType)))
            {
                Console.WriteLine($"\t- {item}");
            }
        };
    }
}
