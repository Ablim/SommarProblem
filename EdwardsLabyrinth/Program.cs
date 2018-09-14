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
            Console.WriteLine(lab.Solve());
        }

        private static IEnumerable<string> ReadInput()
        {
            var result = new List<string>();

            while (true)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                result.Add(line);
            }

            return result;
        }
    }
}
