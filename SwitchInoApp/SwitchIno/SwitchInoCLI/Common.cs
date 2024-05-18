using EasInoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchIno.SwitchInoCLI
{
    internal static class Common
    {
        internal static SwitchIno SwitchIno;

        public delegate void ActionArgs(IEnumerable<string> ArgsProvided);

        internal static string Version
        {
            get { return "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3); }
        }

        internal static IEnumerable<ICommand> GetCommands()
        {
            var type = typeof(ICommand);
            IEnumerable<ICommand> commandsNull = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type)
                .Select(s => (ICommand)Activator.CreateInstance(s));

            IList<ICommand> commands = new List<ICommand>();
            foreach (var command in commandsNull)
            {
                if (command != null) commands.Add(command);
            }
            return commands;
        }

        internal static bool CheckArgsProvided(IDictionary<string, string> args, IEnumerable<string> argsProvided)
        {
            for (int i = 1; i <= args.Count(); i++)
            {
                if (argsProvided.Count() < i)
                {
                    Console.WriteLine($"ERROR: mandatory option {args.ElementAt(i - 1).Key} not provided");
                    return false;
                }
            }

            return true;
        }
    }
}
