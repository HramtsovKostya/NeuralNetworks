using System;
using System.Windows.Forms;

namespace MedicalSystem
{
    internal static class Program
    {
        public static SystemController Controller { get; private set; }

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}