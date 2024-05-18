using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SwitchIno.SwitchInoCLI;

namespace SwitchIno.SwitchInoCLI
{
    internal class Program
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [STAThread]
        static void Main(string[] args)
        {
            Common.SwitchIno = new SwitchIno();

            if (args.Length == 0)
            {
                ConsoleExtension.Hide();
                Application.Run(new SwitchInoForm());
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string cmd = args[0];
            IEnumerable<string> cmd_args = args.Skip(1);

            IEnumerable<ICommand> command = Common.GetCommands().Where(c => c.Command.Contains(cmd));

            if (!command.Any())
            {
                Console.WriteLine($"No match for command: {cmd}");
                return;
            }

            command.First().Run(cmd_args);
        }
    }

    static class ConsoleExtension
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        readonly static IntPtr handle = GetConsoleWindow();
        [DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void Hide()
        {
            ShowWindow(handle, SW_HIDE); //hide the console
        }
        public static void Show()
        {
            ShowWindow(handle, SW_SHOW); //show the console
        }
    }
}
