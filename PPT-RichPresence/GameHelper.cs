using System;
using System.Collections.Generic;

namespace PPT_RichPresence {
    static class GameHelper {
        public static int LobbySize() => Program.PPT.ReadInt32(new IntPtr(
            Program.PPT.ReadInt32(new IntPtr(
                Program.PPT.ReadInt32(new IntPtr(
                    0x140473760
                )) + 0x20
            )) + 0xB4
        ));

        public static int LobbyMax() => Program.PPT.ReadInt32(new IntPtr(
            Program.PPT.ReadInt32(new IntPtr(
                Program.PPT.ReadInt32(new IntPtr(
                    0x140473760
                )) + 0x20
            )) + 0xB8
        ));

        public static int SteamID() => Program.PPT.ReadInt32(new IntPtr(
            0x1405A2010
        ));

        public static int PlayerSteamID(int index) => Program.PPT.ReadInt32(new IntPtr(
            Program.PPT.ReadInt32(new IntPtr(
                Program.PPT.ReadInt32(new IntPtr(
                    0x140473760
                )) + 0x20
            )) + 0x118 + index * 0x50
        ));

        public static int FindPlayer() {
            int players = LobbySize();

            if (players < 2)
                return 0;

            int steam = SteamID();

            for (int i = 0; i < players; i++)
                if (steam == PlayerSteamID(i))
                    return i;

            return 0;
        }

        public static string LobbyInvite() {
            long lobbyID = Program.PPT.ReadInt64(new IntPtr(
                Program.PPT.ReadInt32(new IntPtr(
                    Program.PPT.ReadInt32(new IntPtr(
                        0x140473760
                    )) + 0x20
                )) + 0x378
            ));

            long steamID = SteamID() + 76561197960265728;

            return $"steam://joinlobby/546050/{lobbyID}/{steamID}";
        }

        public static string GetScore() {
            List<int> scores = new List<int>();
            int size = LobbySize();
            int me = FindPlayer();

            for (int i = 0; i < size; i++) {
                int score = Program.PPT.ReadInt32(new IntPtr(
                    Program.PPT.ReadInt32(new IntPtr(
                        0x14057F048
                    )) + 0x38 + i * 0x04
                ));

                if (i == me && size > 2) {
                    scores.Insert(0, score);
                } else scores.Add(score);
            }

            return string.Join(" - ", scores);
        }

        public static string MenuToStringTop(int id) {
            switch (id) {
                case 0:
                case 12: return "Loading";
                case 1: return "Main Menu";
                case 2: return "Adventure";
                case 3:
                case 18: return "Solo Arcade";
                case 4: return "Multiplayer Arcade";
                case 5:
                case 6:
                case 7:
                case 8:
                case 9: return "Options & Data";
                case 10:
                case 32:
                case 33:
                case 37: return "Online";
                case 11: return "Lessons";
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 19: return "Character Select";
                case 20:
                case 25:
                case 27:
                case 34: return "Online - Puzzle League";
                case 21:
                case 23:
                case 24:
                case 26:
                case 28: return "Online - Free Play";
                
            }
            return "Illegal Menu";
        }

        public static string MenuToStringBottom(int id) {
            switch (id) {
                case 3:
                case 4:
                case 23: return "Mode Select";
                case 6: return "Stats";
                case 7: return "Options";
                case 8: return "Theatre";
                case 9: return "Shop";
                case 18: return "Mode Select (Challenge)";
                case 20: return "Standby";
                case 21: return "Room Select";
                case 24: return "Room Creation";
                case 25:
                case 27: return "Matchmaking";
                case 26:
                case 28: return "In Lobby";
                case 32: return "Replays";
                case 33:
                case 37: return "Replay Upload";
                case 34: return "Rankings";
            }
            return "";
        }

        public static int? GetMenu() {
            int menuBase = Program.PPT.ReadInt32(new IntPtr(
                0x140573A78
            ));

            if (menuBase == 0x0) return null;

            return Program.PPT.ReadInt32(new IntPtr(
                menuBase + 0xA4 + Program.PPT.ReadInt32(new IntPtr(
                    menuBase + 0xE8
                )) * 0x04
            ));
        }

        public static string MajorToString(int id) {
            switch (id) {
                case 0: return "Adventure";
                case 1: return "Solo Arcade";
                case 2: return "Multiplayer Arcade";
                case 3: return "Options & Data";
                case 4: return $"Online - {OnlineTypeToString(GetOnlineType())}";
                case 5: return "Lessons";
            }

            return "";
        }

        public static byte GetMajor() => Program.PPT.ReadByte(new IntPtr(
            0x140573854
        ));

        public static int GetMajorFromFlag() {
            int online = Program.PPT.ReadByte(new IntPtr(
                0x14059894C
            )) & 0b00000001;

            if (online > 0) {
                return 4;
            }

            int flags = Program.PPT.ReadByte(new IntPtr(
                0x140451C50
            )) & 0b00010000;

            if (flags > 0) {
                return 2;
            }

            return 1;
        }

        public static string ModeToString(int id) {
            switch (id) {
                case 0: return "Versus";
                case 1: return "Fusion";
                case 2: return "Swap";
                case 3: return "Party";
                case 4: return "Big Bang";
                case 5: return "Versus (Endurance)";
                case 6: return "Fusion (Endurance)";
                case 7: return "Swap (Endurance)";
                case 8: return "Party (Endurance)";
                case 9: return "Big Bang (Endurance)";
                case 10: return "Endless Fever";
                case 11: return "Tiny Puyo";
                case 12: return "Endless Puyo";
                case 13: return "Sprint";
                case 14: return "Marathon";
                case 15: return "Ultra";
            }

            return "";
        }

        public static string ModeToImage(int id) {
            switch (id) {
                case 0:
                case 5: return "versus";
                case 1:
                case 6: return "fusion";
                case 2:
                case 7: return "swap";
                case 3:
                case 8: return "party";
                case 4:
                case 9: return "bigbang";
                case 10: return "endlessfever";
                case 11: return "tinypuyo";
                case 12: return "endlesspuyo";
                case 13: return "sprint";
                case 14: return "marathon";
                case 15: return "ultra";
            }

            return "";
        }

        public static int GetMode(int major) {
            switch (major) {
                case 1:
                case 2: return (Program.PPT.ReadByte(new IntPtr(
                    0x140451C50
                )) & 0b11101111) - 2;

                case 4: return (Program.PPT.ReadByte(new IntPtr(
                    0x1404385C4
                )) > 0)
                    ? Program.PPT.ReadByte(new IntPtr(
                        0x140438584
                    )) - 1
                    : Program.PPT.ReadByte(new IntPtr(
                        0x140573794
                    ));
            }
            return 0;
        }

        public static string TypeToString(int id) {
            switch (id) {
                case 0: return "Puyo";
                case 1: return "Tetris";
            }

            return "";
        }

        public static int GetType(int index) => (
            Program.PPT.ReadByte(new IntPtr(
                0x140598C27 + index * 0x68
            )) & 0b01000000
        ) >> 6;

        public static string OnlineTypeToString(int id) {
            switch (id) {
                case 0: return "Puzzle League";
                case 1: return "Free Play";
            }

            return "";
        }

        public static int GetOnlineType() => Program.PPT.ReadByte(new IntPtr(
            0x140573797
        )) & 0b00000001;

        public static bool IsInitial() => (
            Program.PPT.ReadByte(new IntPtr(
                0x1404640C2
            )) & 0b00100000
        ) == 0b00100000;

        public static bool IsAdventure() => Program.PPT.ReadByte(new IntPtr(
            0x140451C50
        )) == 0b00000001 && GetMajor() == 0;

        public static bool IsCharacterSelect() {
            int P1State = Program.PPT.ReadByte(new IntPtr(
                Program.PPT.ReadInt32(new IntPtr(
                    0x140460690
                )) + 0x274
            ));

            return P1State > 0 && P1State < 16;
        }

        public static string CharacterToString(int id) {
            switch (id) {
                case 0: return "Ringo";
                case 1: return "Risukuma";
                case 2: return "Maguro";
                case 3: return "Amitie";
                case 4: return "Klug";
                case 5: return "Sig";
                case 6: return "Arle & Carbuncle";
                case 7: return "Suketoudara";
                case 8: return "Schezo";
                case 9: return "Draco Centauros";
                case 10: return "Witch";
                case 11: return "Lemres";
                case 12: return "Jay & Elle";
                case 13: return "Ai";
                case 14: return "Ess";
                case 15: return "Zed";
                case 16: return "O";
                case 17: return "Tee";
                case 18: return "Raffina";
                case 19: return "Rulue";
                case 20: return "Feli";
                case 21: return "Dark Prince";
                case 22: return "Ecolo";
                case 23: return "Ex";
            }

            return "";
        }

        public static string CharacterToImage(int id) {
            switch (id) {
                case 0: return "ringo";
                case 1: return "risukuma";
                case 2: return "maguro";
                case 3: return "amitie";
                case 4: return "klug";
                case 5: return "sig";
                case 6: return "arle";
                case 7: return "suketoudara";
                case 8: return "schezo";
                case 9: return "draco";
                case 10: return "witch";
                case 11: return "lemres";
                case 12: return "jayelle";
                case 13: return "ai";
                case 14: return "ess";
                case 15: return "zed";
                case 16: return "o";
                case 17: return "tee";
                case 18: return "raffina";
                case 19: return "rulue";
                case 20: return "feli";
                case 21: return "satan";
                case 22: return "ecolo";
                case 23: return "ex";
            }

            return "";
        }

        public static int GetCharacter(int index) => (
            Program.PPT.ReadByte(new IntPtr(
                0x140598C28 + index * 0x68
            ))
        );

        public static bool IsPregame() {
            int pointer = Program.PPT.ReadByte(new IntPtr(
                Program.PPT.ReadInt32(new IntPtr(
                    0x140460690
                )) + 0x280
            ));

            return pointer != 0 && pointer != -1;
        }

        public static bool IsMatch() => Program.PPT.ReadInt32(new IntPtr(
            0x140461B20
        )) != 0;

        public static string MatchPlayerName(int index) => Program.PPT.ReadStringUnicode(new IntPtr(
            0x140598BD4 + index * 0x68
        ), 32).Split('\0')[0];

        public static bool IsLoading() => Program.PPT.ReadByte(new IntPtr(
            0x14059140F
        )) > 0;

        public static bool IsReplay() => Program.PPT.ReadByte(new IntPtr(
            0x140598BC8
        )) != 0;
    }
}
