using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeepAlive
{

    public class Program : Form
    {
        private NotifyIcon icon;
        private System.Threading.Timer timer;
        private readonly string[] processes = { "TTREngine", "fellowship", "relived", "ppython", "ods" };

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public Program()
        {
            ContextMenu menu = new ContextMenu();

            menu.MenuItems.Add("Exit", OnExit);

            icon = new NotifyIcon();
            icon.Text = "KeepAlive";
            icon.Icon = Properties.Resources.Icon;
            icon.ContextMenu = menu;
            icon.Visible = true;

            timer = new System.Threading.Timer(RunKeepAlive, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }
        
        public static void Main()
        {
            Application.Run(new Program());
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                icon.Dispose();
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            base.Dispose(disposing);
        }

        private void RunKeepAlive(object state)
        {
            foreach (string name in processes) {
                foreach (Process process in Process.GetProcessesByName(name)) {
                    PostMessage(process.MainWindowHandle, 0x0100, (IntPtr) Keys.End, (IntPtr) 0);
                    PostMessage(process.MainWindowHandle, 0x0101, (IntPtr) Keys.End, (IntPtr) 0);
                }
            }
        }
    }
}
