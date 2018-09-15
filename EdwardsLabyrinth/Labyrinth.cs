using System;
using System.Collections.Generic;
using System.Linq;

namespace EdwardsLabyrinth
{
    public class Labyrinth
    {
        private int _width;
        private int _height;
        private char[,] _map;
        private List<(int row, int col)>[,] _graph;
        private List<(int row, int col, char id)> _teleporters;
        private int _startID;
        private int _endID;
        private Dictionary<int, (int row, int col)> _coordinateLookup;
        private Dictionary<(int row, int col), int> _idLookup;

        public Labyrinth(IEnumerable<string> mapLines)
        {
            _width = mapLines.OrderByDescending(x => x.Length).First().Length;
            _height = mapLines.Count();
            _map = new char[_height, _width];
            _graph = new List<(int, int)>[_height, _width];
            _teleporters = new List<(int, int, char)>();
            _coordinateLookup = new Dictionary<int, (int row, int col)>();
            _idLookup = new Dictionary<(int row, int col), int>();

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

            var counter = 0;
            
            //Create traversal graph
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    var cell = _map[row, col];
                    
                    if (IsWalkableCell(cell))
                    {
                        if (IsStart(cell))
                            _startID = counter;
                        else if (IsEnd(cell))
                            _endID = counter;

                        _coordinateLookup.Add(counter, (row, col));
                        _idLookup.Add((row, col), counter);
                        counter++;

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
            return IsTeleporter(cell) || cell == ' ' || IsStart(cell) || IsEnd(cell);
        }

        private bool IsTeleporter(char cell)
        {
            return ((int)cell >= 48 && (int)cell <= 57);
        }

        private bool IsEnd(char cell)
        {
            return cell == 'E';
        }

        private bool IsStart(char cell)
        {
            return cell == 'S';
        }

        //Find shortest path from S to E.
        public string Solve()
        {
            //PrintMap();
            CreateGraph();

            var nodesToTest = new HashSet<int>();
            nodesToTest.Add(_startID);

            var costLookup = new Dictionary<int, (int from, int cost)>();
            costLookup.Add(_startID, (_startID, 0));

            var visitedNodes = new HashSet<int>();

            while (nodesToTest.Count > 0)
            {
                var nodeID = nodesToTest.First();
                nodesToTest.Remove(nodeID);
                var nodeCoordinates = _coordinateLookup[nodeID];
                var nodeCost = costLookup[nodeID].cost;
                var nodeIsTeleporter = IsTeleporter(_map[nodeCoordinates.row, nodeCoordinates.col]);
                var adjacentNodes = _graph[nodeCoordinates.row, nodeCoordinates.col];

                foreach (var neighbor in adjacentNodes)
                {
                    var neighborID = _idLookup[neighbor];

                    if (visitedNodes.Contains(neighborID))
                        continue;

                    var neighborCoordinates = _coordinateLookup[neighborID];
                    var neighborIsTeleporter = IsTeleporter(_map[neighborCoordinates.row, neighborCoordinates.col]);
                    var exists = costLookup.TryGetValue(neighborID, out (int from, int cost) neighborCost);

                    if (!exists)
                    {
                        if (nodeIsTeleporter && neighborIsTeleporter)
                            costLookup.Add(neighborID, (nodeID, nodeCost));
                        else
                            costLookup.Add(neighborID, (nodeID, nodeCost + 1));

                        if (!nodesToTest.Contains(neighborID))
                            nodesToTest.Add(neighborID);
                    }
                    else if (nodeCost < neighborCost.cost)
                    {
                        costLookup[neighborID] = nodeIsTeleporter && neighborIsTeleporter ? (nodeID, nodeCost) : (nodeID, nodeCost + 1);

                        if (!nodesToTest.Contains(neighborID))
                            nodesToTest.Add(neighborID);
                    }

                    visitedNodes.Add(nodeID);
                }
            }

            return PrintPath(costLookup);
        }

        //Get path by backtracking.
        private string PrintPath(Dictionary<int, (int from, int cost)> costLookup)
        {
            var id = _endID;
            var coordinates = _coordinateLookup[id];
            var isTeleporter = IsTeleporter(_map[coordinates.row, coordinates.col]);
            var result = new List<string>();

            while (id != _startID)
            {
                var nextID = costLookup[id].from;
                var nextCoordinates = _coordinateLookup[nextID];
                var nextIsTeleporter = IsTeleporter(_map[nextCoordinates.row, nextCoordinates.col]);

                if (!(nextIsTeleporter && isTeleporter))
                    result.Insert(0, GetDirection(nextCoordinates, coordinates));

                id = nextID;
                coordinates = nextCoordinates;
                isTeleporter = nextIsTeleporter;
            }

            return string.Join(" ", result);
        }

        private string GetDirection((int row, int col) from, (int row, int col) to)
        {
            if (from.row > to.row)
                return "N";
            else if (from.row < to.row)
                return "S";
            else if (from.col < to.col)
                return "E";
            else
                return "W";
        }
    }
}
