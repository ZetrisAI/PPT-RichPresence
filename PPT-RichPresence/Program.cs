using System;
using System.Threading;

using DiscordRPC;

namespace PPT_RichPresence {
    static class Program {
        public static ProcessMemory PPT = new ProcessMemory("puyopuyotetris");
        static DiscordRpcClient Presence;

        static System.Windows.Forms.NotifyIcon tray = new System.Windows.Forms.NotifyIcon {
            Icon = Properties.Resources.TrayIcon,
            Text = "Puyo Puyo Tetris Rich Presence",
            Visible = true
        };

        static Timer ScanTimer = new Timer(new TimerCallback(Loop), null, Timeout.Infinite, 1000);

        static RichPresence GetState() {
            int? menuId = GameHelper.GetMenu();
            if (menuId.HasValue) {
                return new RichPresence() {
                    Details = "In Menu",
                    State = GameHelper.MenuToString(menuId.Value),
                    Assets = new Assets() {
                        LargeImageKey = "menu"
                    }
                };

            } else if (GameHelper.IsAdventure()) {
                return new RichPresence() {
                    Details = "Adventure",
                    Assets = new Assets() {
                        LargeImageKey = "adventure"
                    }
                };

            } else if (GameHelper.IsInitial()) {
                return new RichPresence() {
                    Details = "Splash Screen",
                    Assets = new Assets() {
                        LargeImageKey = "menu"
                    }
                };
            
            
            } else if (GameHelper.IsCharacterSelect()) {
                return new RichPresence() {
                    Details = "Character Select",
                    Assets = new Assets() {
                        LargeImageKey = "menu"
                    }
                };
            }

            return new RichPresence() {
                Details = "Unknown",
                Assets = new Assets() {
                    LargeImageKey = "menu"
                }
            };
        }

        static void Loop(object e) {
            Presence.Invoke();
            
            if (PPT.CheckProcess()) {
                PPT.TrustProcess = true;
                Presence.SetPresence(GetState());
                PPT.TrustProcess = false;
            }
        }

        [STAThread]
        static void Main() {
            Presence = new DiscordRpcClient("539426896841277440");
            //Presence.OnReady += (sender, e) => {};
            Presence.Initialize();
            ScanTimer.Change(0, 1000);

            new ManualResetEvent(false).WaitOne();
        }
    }
}
