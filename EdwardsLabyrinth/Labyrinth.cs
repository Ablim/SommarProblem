using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdwardsLabyrinth
{
    public class Labyrinth
    {
        private char[,] _map = null;
        private int _width = 0;
        private int _height = 0;
        private List<(int row, int col)>[,] _graph = null;
        private List<(int row, int col, int id)> _teleporters = new List<(int, int, int)>();

        private Labyrinth(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new char[height, width];
            _graph = new List<(int, int)>[height, width];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    _map[h, w] = '*';
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
                    _map[row, col] = GetChar(c);
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
                return '*';
            }
        }

        private void PrintMap()
        {
            for (int h = 0; h < _height; h++)
            {
                for (int w = 0; w < _width; w++)
                {
                    Console.Write(_map[h, w]);
                }

                Console.WriteLine();
            }
        }

        private void CreateGraph()
        {
            //Find teleports
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    var cell = _map[row, col].ToString();
                    var isTeleporter = int.TryParse(cell, out int teleporter);

                    if (isTeleporter)
                    {
                        //Console.WriteLine($"{row}, {col}: {teleporter}");
                        _teleporters.Add((row: row, col: col, id: teleporter));
                    }
                }
            }

            //Create traversal graph
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    var cell = _map[row, col];
                    
                    if (cell == ' ' || cell == 'S')
                    {
                        //Is normal or start cell
                    }
                    else if (int.TryParse(cell.ToString(), out int dontCare))
                    {
                        //Is teleporter
                    }
                }
            }
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
            //No we have something to work with


            lab.CreateGraph();
            lab.Solve();
            lab.PrintMap();



            return "";
        }
    }
}
