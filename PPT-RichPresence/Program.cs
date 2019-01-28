using System;
using System.Threading;

using DiscordRPC;

namespace PPT_RichPresence {
    static class Program {
        static ProcessMemory PPT = new ProcessMemory("puyopuyotetris");
        static DiscordRpcClient Presence;

        static System.Windows.Forms.NotifyIcon tray = new System.Windows.Forms.NotifyIcon {
            Icon = Properties.Resources.TrayIcon,
            Text = "Puyo Puyo Tetris Rich Presence",
            Visible = true
        };

        static Timer ScanTimer = new Timer(new TimerCallback(Loop), null, Timeout.Infinite, 1000);

        static void Loop(object e) {
            Presence.Invoke();

            Presence.SetPresence(new RichPresence() {
                Details = "In Match",
                State = "Puzzle League",
                Assets = new Assets() {
                    LargeImageKey = "tetris",
                    LargeImageText = "Versus (Tetris)",
                }
            });

            if (PPT.CheckProcess()) {
                PPT.TrustProcess = true;
                
                // Game reads, update presence

                PPT.TrustProcess = false;
            }
        }

        [STAThread]
        static void Main() {
            Presence = new DiscordRpcClient("539426896841277440");

            Presence.OnReady += (sender, e) => {
                tray.ShowBalloonTip(2, "Ready", $"User {e.User.Username}", System.Windows.Forms.ToolTipIcon.None);
            };

            Presence.Initialize();
            ScanTimer.Change(0, 1000);

            new ManualResetEvent(false).WaitOne();
        }
    }
}
