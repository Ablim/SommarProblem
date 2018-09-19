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

        /// <summary>
        /// Create a Labyrinth.
        /// </summary>
        public Labyrinth(IEnumerable<string> mapLines)
        {
            _width = mapLines.OrderByDescending(x => x.Length).First().Length;
            _height = mapLines.Count();
            _map = new char[_height, _width];
            _graph = new List<(int, int)>[_height, _width];
            _teleporters = new List<(int, int, char)>();
            _coordinateLookup = new Dictionary<int, (int row, int col)>();
            _idLookup = new Dictionary<(int row, int col), int>();

            CreateMap(mapLines);
        }

        private void CreateMap(IEnumerable<string> lines)
        {
            var row = 0;

            foreach (var line in lines)
            {
                for (int col = 0; col < line.Length; col++)
                {
                    var cell = line[col];
                    _map[row, col] = cell;

                    if (Utils.IsTeleporter(cell))
                        _teleporters.Add((row: row, col: col, id: cell));
                }

                row++;
            }
        }

        /// <summary>
        /// Print the internal map representation of the Labyrinth.
        /// </summary>
        public void PrintMap()
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
            var counter = 0;
            
            //Create traversal graph
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    var cell = _map[row, col];
                    
                    if (Utils.IsWalkableCell(cell))
                    {
                        if (Utils.IsStart(cell))
                            _startID = counter;
                        else if (Utils.IsEnd(cell))
                            _endID = counter;

                        _coordinateLookup.Add(counter, (row, col));
                        _idLookup.Add((row, col), counter);
                        counter++;

                        _graph[row, col] = new List<(int row, int col)>();

                        //North
                        if (IsWithinBounds(row - 1, col) && Utils.IsWalkableCell(_map[row - 1, col]))
                            _graph[row, col].Add((row - 1, col));

                        //South
                        if (IsWithinBounds(row + 1, col) && Utils.IsWalkableCell(_map[row + 1, col]))
                            _graph[row, col].Add((row + 1, col));
                        
                        //East
                        if (IsWithinBounds(row, col + 1) && Utils.IsWalkableCell(_map[row, col + 1]))
                            _graph[row, col].Add((row, col + 1));
                        
                        //West
                        if (IsWithinBounds(row, col - 1) && Utils.IsWalkableCell(_map[row, col - 1]))
                            _graph[row, col].Add((row, col - 1));

                        if (Utils.IsTeleporter(cell))
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

        /// <summary>
        /// Find and return the path from S to E.
        /// </summary>
        public string Solve()
        {
            CreateGraph();

            var nodesToTest = new List<(int id, int cost)>();
            nodesToTest.Add((_startID, 0));

            var costLookup = new Dictionary<int, (int from, int cost)>();
            costLookup.Add(_startID, (_startID, 0));

            var visitedNodes = new HashSet<int>();

            while (nodesToTest.Count > 0)
            {
                var nodeToTest = nodesToTest.First();
                var nodeID = nodeToTest.id;
                nodesToTest.Remove(nodeToTest);
                visitedNodes.Add(nodeID);
                var nodeCoordinates = _coordinateLookup[nodeID];
                var nodeCost = costLookup[nodeID].cost;
                var nodeIsTeleporter = Utils.IsTeleporter(_map[nodeCoordinates.row, nodeCoordinates.col]);
                var adjacentNodes = _graph[nodeCoordinates.row, nodeCoordinates.col];

                foreach (var neighbor in adjacentNodes)
                {
                    var neighborID = _idLookup[neighbor];
                    var neighborCoordinates = _coordinateLookup[neighborID];
                    var neighborIsTeleporter = Utils.IsTeleporter(_map[neighborCoordinates.row, neighborCoordinates.col]);
                    var exists = costLookup.TryGetValue(neighborID, out (int from, int cost) neighborCost);

                    if (!exists)
                    {
                        if (nodeIsTeleporter && neighborIsTeleporter)
                            costLookup.Add(neighborID, (nodeID, nodeCost));
                        else
                            costLookup.Add(neighborID, (nodeID, nodeCost + 1));

                    }
                    else if (nodeCost < neighborCost.cost)
                    {
                        costLookup[neighborID] = nodeIsTeleporter && neighborIsTeleporter ? (nodeID, nodeCost) : (nodeID, nodeCost + 1);
                    }

                    if (!visitedNodes.Contains(neighborID))
                    {
                        var temp = nodesToTest.Where(x => x.id == neighborID);

                        if (!temp.Any())
                        {
                            nodesToTest.Add((neighborID, costLookup[neighborID].cost));
                        }
                        else
                        {
                            var temp2 = temp.First();
                            temp2.cost = costLookup[neighborID].cost;
                        }
                    }
                }

                nodesToTest.Sort((x, y) => x.cost.CompareTo(y.cost));
            }

            return GetPath(costLookup);
        }

        private string GetPath(Dictionary<int, (int from, int cost)> costLookup)
        {
            var id = _endID;
            var coordinates = _coordinateLookup[id];
            var isTeleporter = Utils.IsTeleporter(_map[coordinates.row, coordinates.col]);
            var result = new List<string>();

            while (id != _startID)
            {
                var nextID = costLookup[id].from;
                var nextCoordinates = _coordinateLookup[nextID];
                var nextIsTeleporter = Utils.IsTeleporter(_map[nextCoordinates.row, nextCoordinates.col]);

                if (!nextIsTeleporter || !isTeleporter)
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
