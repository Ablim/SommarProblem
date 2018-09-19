using System;
using System.Collections.Generic;
using System.Text;

namespace EdwardsLabyrinth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = ReadInput();
            var lab = new Labyrinth(input);
            //lab.PrintMap();
            Console.WriteLine(lab.Solve());
        }

        /// <summary>
        /// Read input from STDIN. Line by line until no more to read.
        /// </summary>
        public static IEnumerable<string> ReadInput()
        {
            var result = new List<string>();

            while (true)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                var cleanedInput = CleanInput(line);

                if (cleanedInput.Length > 0)
                    result.Add(cleanedInput);
            }

            return result;
        }

        /// <summary>
        /// Clean input of invalid characters. The result contains only valid characters or an empty string. 
        /// Valid characters are: 0-9, *, ' ', S and E.
        /// </summary>
        public static string CleanInput(string line)
        {
            var result = new StringBuilder();

            foreach (var c in line)
            {
                if (Utils.IsWalkableCell(c) || Utils.IsWall(c))
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}
