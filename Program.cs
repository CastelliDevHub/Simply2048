using System;
using System.Windows.Forms;

namespace Game2048
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Game2048Form());
        }
    }
}
