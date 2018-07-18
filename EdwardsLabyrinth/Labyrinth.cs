using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdwardsLabyrinth
{
    public class Labyrinth
    {
        private char[,] map;
        private int width;
        private int height;

        private Labyrinth(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new char[height, width];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    map[h, w] = 'X';
                }
            }
        }

        private void ParseLinesToMap(IEnumerable<string> lines)
        {
            var row = 0;

            foreach (var line in lines)
            {
                for (int col = 0; col < line.Length; col++)
                {
                    var c = line[col];
                    map[row, col] = GetChar(c);
                }

                row++;
            }
        }

        private char GetChar(char c)
        {
            /*
             * Valid:
             * -Numbers
             * -'*'
             * -' '
             * -'S'
             * -'E'
             */
            if (((int)c >= 48 && (int)c <= 57) || c == ' ' || c == 'S' || c == 'E')
            {
                return c;
            }
            else
            {
                return 'X';
            }
        }

        private void PrintMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Console.Write(map[h, w]);
                }

                Console.WriteLine();
            }
        }

        private void CreateGraph()
        {

        }

        private void Solve()
        {

        }

        public static string GetStepsToExit(IEnumerable<string> mapLines)
        {
            var width = mapLines
                .OrderByDescending(x => x.Length)
                .First()
                .Length;
            var height = mapLines.Count();

            var lab = new Labyrinth(width, height);
            lab.ParseLinesToMap(mapLines);
            lab.CreateGraph();
            lab.Solve();
            lab.PrintMap();



            return "";
        }
    }
}
