using EasInoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchIno.SwitchInoCLI
{
    internal class SwitchInoVersion : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-v", "--version" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Get SwitchIno versions";

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

            try
            {
                Common.SwitchIno.Start(sInoConfig);
                string switchinoVersion = Common.SwitchIno.GetSwitchInoVersion();
                Console.WriteLine($"SwitchIno: API: [ {Common.Version} ] Board: [ {switchinoVersion} ]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };
    }
}
