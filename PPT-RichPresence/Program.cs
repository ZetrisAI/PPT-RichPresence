using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using DiscordRPC;

namespace PPT_RichPresence {
    static class Program {
        public static ProcessMemory PPT = new ProcessMemory("puyopuyotetris");
        static DiscordRpcClient Presence;

        static readonly string ShortcutPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Startup)}\\PPT-RichPresence.lnk";

        static NotifyIcon tray = new NotifyIcon {
            ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Copy Invite Link", new EventHandler(CopyInviteLink)),
                new MenuItem("Unsoftlock", new EventHandler(Unsoftlock)),
                new MenuItem("-"),
                new MenuItem("Run on Startup", new EventHandler(Shortcut)) {
                    Checked = File.Exists(ShortcutPath)
                },
                new MenuItem("Exit", new EventHandler(Close))
            }),
            Icon = Properties.Resources.TrayIcon,
            Text = "Puyo Puyo Tetris Rich Presence"
        };

        static void CheckFreePlayLobby(object sender, EventArgs e) => ((ContextMenu)sender).MenuItems[0].Enabled = GameHelper.GetMenu() == 28;

        static void CopyInviteLink(object sender, EventArgs e) {
            int? menuId = GameHelper.GetMenu();

            if (menuId.HasValue)
                if (menuId == 28)
                    Clipboard.SetText(GameHelper.LobbyInvite());
        }

        static void Unsoftlock(object sender, EventArgs e) => Process.Start("steam://joinlobby/546050/109775241058543776/76561198802063829");

        static void Shortcut(object sender, EventArgs e) {
            if (((MenuItem)sender).Checked) {
                File.Delete(ShortcutPath);
                ((MenuItem)sender).Checked = false;

            } else if (Path.GetFullPath(Application.ExecutablePath).StartsWith(Path.GetTempPath()))
                MessageBox.Show(
                    "It appears you're running PPT-RichPresence from an archive file. Please extract the program to use this feature.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning
                );

            else {
                IWshRuntimeLibrary.IWshShortcut shortcut = new IWshRuntimeLibrary.WshShell().CreateShortcut(ShortcutPath);

                shortcut.Description = "Discord Rich Presence for Puyo Puyo Tetris.";
                shortcut.TargetPath = Path.GetFullPath(Application.ExecutablePath);
                shortcut.Save();

                ((MenuItem)sender).Checked = true;
            }
        }

        static void Close(object sender, EventArgs e) => Application.Exit();

        static System.Threading.Timer ScanTimer = new System.Threading.Timer(new System.Threading.TimerCallback(Loop), null, System.Threading.Timeout.Infinite, 1000);

        static RichPresence GetState(out bool success) {
            success = true;

            RichPresence ret = new RichPresence() {
                Assets = new Assets() {
                    LargeImageKey = "menu"
                }
            };

            int? menuId = GameHelper.GetMenu();

            if (menuId.HasValue) {
                ret.Details = GameHelper.MenuToStringTop(menuId.Value);
                ret.State = GameHelper.MenuToStringBottom(menuId.Value);

                if (menuId == 28)
                    ret.Details += $" ({GameHelper.LobbySize()} / {GameHelper.LobbyMax()})";

                return ret;
            }

            if (GameHelper.IsAdventure()) {
                ret.Details = "Adventure";
                ret.Assets.LargeImageKey = "adventure";
                return ret;
            }

            if (GameHelper.IsInitial()) {
                ret.Details = "Splash Screen";
                return ret;
            }

            if (GameHelper.IsReplay()) {
                ret.Details = (GameHelper.IsLocalReplay())? "Watching a Replay": "Watching an Online Replay";
                return ret;
            }

            int majorId = GameHelper.GetMajorFromFlag();
            int modeId = GameHelper.GetMode(majorId);
            ret.Details = GameHelper.MajorToString(majorId);
            ret.Assets.LargeImageText = GameHelper.ModeToString(modeId);
            ret.Assets.LargeImageKey = GameHelper.ModeToImage(modeId);

            if (GameHelper.GetOnlineType() == 1)
                ret.Details += $" ({GameHelper.LobbySize()} / {GameHelper.LobbyMax()})";

            if (GameHelper.IsCharacterSelect()) {
                ret.State = "Character Select";
                return ret;
            }

            // Disabled until better understood
            /*if (GameHelper.IsLoading()) {
                ret.State = "Loading";
                return ret;
            }*/

            int playerId = GameHelper.FindPlayer();

            string type = (modeId == 0 || modeId == 5 || modeId == 3 || modeId == 8 || modeId == 4 || modeId == 9)
                ? $" - {GameHelper.TypeToString(GameHelper.GetType(playerId))}"
                : "";

            int characterId = GameHelper.GetCharacter(playerId);

            ret.Assets.SmallImageText = GameHelper.CharacterToString(characterId);
            ret.Assets.SmallImageKey = GameHelper.CharacterToImage(characterId);

            if (GameHelper.IsPregame()) {
                ret.State = "Pregame";
                ret.Assets.LargeImageText += type;
                return ret;
            }

            if (GameHelper.IsMatch()) {
                ret.State = (GameHelper.LobbySize() == 2)
                    ? $"vs. {GameHelper.MatchPlayerName(1 - playerId)}"
                    : "Match";

                if (majorId == 4)
                    ret.State += $" ({GameHelper.GetScore()})";

                ret.Assets.LargeImageText += type;

                return ret;
            }

            success = false;

            return new RichPresence() {
                Assets = new Assets() {
                    LargeImageKey = "menu"
                }
            };
        }

        static void Loop(object e) {
            Presence.Invoke();

            if (PPT.CheckProcess()) {
                PPT.TrustProcess = true;

                RichPresence GameState = GetState(out bool success);
                if (success) Presence.SetPresence(GameState);

                PPT.TrustProcess = false;

            } else Presence.ClearPresence();
        }

        [STAThread]
        static void Main() {
            if (Process.GetProcessesByName("PPT-RichPresence").Length > 1) {
                MessageBox.Show(
                    "Another instance of PPT-RichPresence is already running. Check your tray.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return;
            }

            Presence = new DiscordRpcClient("539426896841277440");
            Presence.Initialize();

            tray.Visible = true;
            tray.ContextMenu.Popup += CheckFreePlayLobby;

            ScanTimer.Change(0, 1000);

            Application.Run();

            ScanTimer.Dispose();
            
            tray.Dispose();

            Presence.ClearPresence();
            Presence.Dispose();
        }
    }
}
