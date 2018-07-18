using System;
using System.Collections.Generic;
using System.Text;

namespace EdwardsLabyrinth
{
    public class Program
    {
        //Read map from STDIN
        //Calculate fastest route from S to E
        //Print steps to STDOUT
        public static void Main(string[] args)
        {
            while (true)
            {
                var s = Console.ReadLine();

                if (string.IsNullOrEmpty(s))
                    break;

                Console.WriteLine(s.Length);
            }
        }
    }
}
