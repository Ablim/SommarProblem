using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdwardsLabyrinth
{
    public class Labyrinth
    {
        private int _width;
        private int _height;
        private char[,] _map;
        private List<(int row, int col)>[,] _graph;
        private List<(int row, int col, char id)> _teleporters;

        public Labyrinth(IEnumerable<string> mapLines)
        {
            _width = mapLines.OrderByDescending(x => x.Length).First().Length;
            _height = mapLines.Count();
            _map = new char[_height, _width];
            _graph = new List<(int, int)>[_height, _width];
            _teleporters = new List<(int, int, char)>();

            ParseLinesToMap(mapLines);
        }

        private void ParseLinesToMap(IEnumerable<string> lines)
        {
            var row = 0;

            foreach (var line in lines)
            {
                for (int col = 0; col < line.Length; col++)
                {
                    _map[row, col] = ParseCell(line[col]);
                }

                row++;
            }
        }

        private char ParseCell(char cell)
        {
            if (IsWalkableCell(cell))
            {
                return cell;
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
                    var cell = _map[row, col];

                    if (IsTeleporter(cell))
                        _teleporters.Add((row: row, col: col, id: cell));
                }
            }

            //Create traversal graph
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    var cell = _map[row, col];
                    
                    if (IsWalkableCell(cell))
                    {
                        _graph[row, col] = new List<(int row, int col)>();

                        //North
                        if (IsWithinBounds(row - 1, col) && IsWalkableCell(_map[row - 1, col]))
                            _graph[row, col].Add((row - 1, col));

                        //South
                        if (IsWithinBounds(row + 1, col) && IsWalkableCell(_map[row + 1, col]))
                            _graph[row, col].Add((row + 1, col));
                        
                        //East
                        if (IsWithinBounds(row, col + 1) && IsWalkableCell(_map[row, col + 1]))
                            _graph[row, col].Add((row, col + 1));
                        
                        //West
                        if (IsWithinBounds(row, col - 1) && IsWalkableCell(_map[row, col - 1]))
                            _graph[row, col].Add((row, col - 1));

                        if (IsTeleporter(cell))
                        {
                            var otherTeleporters = _teleporters.Where(t => t.id == cell && (t.row != row || t.col != col));

                            if (otherTeleporters.Any())
                            {
                                var other = otherTeleporters.First();
                                _graph[row, col].Add((other.row, other.col));
                            }
                        }
                    }
                }
            }
        }

        private bool IsWithinBounds(int row, int col)
        {
            return (row >= 0 && row < _height && col >= 0 && col < _width);
        }

        private bool IsWalkableCell(char cell)
        {
            return IsTeleporter(cell) || cell == ' ' || cell == 'S' || IsEnd(cell);
        }

        private bool IsTeleporter(char cell)
        {
            return ((int)cell >= 48 && (int)cell <= 57);
        }

        private bool IsEnd(char cell)
        {
            return cell == 'E';
        }

        public string Solve()
        {
            //Find shortest path from S to E
            PrintMap();
            CreateGraph();
            return "";
        }
    }
}
