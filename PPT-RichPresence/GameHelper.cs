using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPT_RichPresence {
    static class GameHelper {
        public static string MenuToStringTop(int id) {
            switch (id) {
                case 0: return "Initial Screen";
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
                case 12: return "Loading";
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 19: return "Character Select";
                case 20:
                case 25:
                case 27:
                case 35: return "Online - Puzzle League";
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
                case 28: return "Free Play";
                case 32: return "Replays";
                case 33:
                case 37: return "Replay Upload";
                case 35: return "Rankings";
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
                case 4: return "Online";
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

        public static bool IsLoading() => Program.PPT.ReadByte(new IntPtr(
            0x14059140F
        )) > 0;
    }
}
