using System;
using System.Collections.Generic;
using System.Text;

namespace EdwardsLabyrinth
{
    public static class Utils
    {
        public static bool IsWalkableCell(char cell)
        {
            return IsTeleporter(cell) || IsNormal(cell) || IsStart(cell) || IsEnd(cell);
        }

        public static bool IsTeleporter(char cell)
        {
            return ((int)cell >= 48 && (int)cell <= 57);
        }

        public static bool IsEnd(char cell)
        {
            return cell == 'E';
        }

        public static bool IsStart(char cell)
        {
            return cell == 'S';
        }

        public static bool IsNormal(char cell)
        {
            return cell == ' ';
        }

        public static bool IsWall(char cell)
        {
            return cell == '*';
        }
    }
}
