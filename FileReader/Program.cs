using System;
using System.IO;

namespace FileReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fileContent = File.ReadAllText("lala.txt");
            Console.Write(fileContent);
        }
    }
}
