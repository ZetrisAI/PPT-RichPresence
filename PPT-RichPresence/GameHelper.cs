using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPT_RichPresence {
    static class GameHelper {
        public static string MenuToString(int id) {
            switch (id) {
                case 0: return "Initial Screen";
                case 1: return "Main Menu";
                case 2: return "Adventure";
                case 3: return "Solo Arcade";
                case 4: return "Multiplayer Arcade";
                case 5: return "Options & Data";
                case 6: return "Stats";
                case 7: return "Options";
                case 8: return "Theatre";
                case 9: return "Shop";
                case 10: return "Online";
                case 11: return "Lessons";
                case 12: return "Loading";
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 19: return "Character Select";
                case 18: return "Solo Arcade (Challenge)";
                case 20: return "Puzzle League (Standby)";
                case 21: return "Free Play (Room Select)";
                case 23: return "Free Play (Mode Select)";
                case 24: return "Free Play (Room Creation)";
                case 25:
                case 27: return "Puzzle League (Matchmaking)";
                case 26: return "Free Play (Matchmaking)";
                case 28: return "Free Play(In Lobby)";
                case 32: return "Online (Replays)";
                case 33:
                case 37: return "Online (Replay Upload)";
                case 35: return "Puzzle League (Rankings)";
            }
            return "Illegal Menu";
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

        private static byte GetMenuMajorMode() => Program.PPT.ReadByte(new IntPtr(
            0x140573854
        ));

        public static bool IsInitial() => (
            Program.PPT.ReadByte(new IntPtr(
                0x1404640C2
            )) & 0b00100000
        ) == 0b00100000;

        public static bool IsAdventure() => (
            Program.PPT.ReadByte(new IntPtr(
                0x140451C50
            )) & 0b00000001
        ) == 0b00000001 && GetMenuMajorMode() == 0;

        public static bool IsCharacterSelect() => Program.PPT.ReadByte(new IntPtr(
            Program.PPT.ReadInt32(new IntPtr(
                0x140460690
            )) + 0x274
        )) > 0;
    }
}
