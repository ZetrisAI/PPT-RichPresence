using System;
using System.Windows.Forms;

namespace PPT_RichPresence {
    static class Program {
        static NotifyIcon tray = new NotifyIcon {
            Icon = Properties.Resources.TrayIcon,
            Text = "Puyo Puyo Tetris Rich Presence",
            Visible = true
        };

        [STAThread]
        static void Main() {            
            while ( true) ;
        }
    }
}
