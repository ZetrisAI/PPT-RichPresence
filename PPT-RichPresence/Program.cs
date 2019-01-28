using System;
using System.Windows.Forms;

namespace PPT_RichPresence {
    static class Program {
        static ProcessMemory PPT = new ProcessMemory("puyopuyotetris");

        static NotifyIcon tray = new NotifyIcon {
            Icon = Properties.Resources.TrayIcon,
            Text = "Puyo Puyo Tetris Rich Presence",
            Visible = true
        };

        static Timer ScanTimer = new Timer {
            Enabled = true,
            Interval = 1000
        };

        static void Loop(object sender, EventArgs e) {
            if (PPT.CheckProcess()) {
                PPT.TrustProcess = true;

                // Game reads, update presence

                PPT.TrustProcess = false;
            }
        }

        [STAThread]
        static void Main() {
            ScanTimer.Tick += new EventHandler(Loop);
        }
    }
}
