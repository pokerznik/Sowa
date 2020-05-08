using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZanP.OrderBooks
{
    class Program
    {

        private static string[] ReadFile()
        {
            return File.ReadAllLines("data");
        }

        static void Main(string[] args)
        {
            var lines = ReadFile();
            Console.WriteLine("Hello World!");
        }
    }
}
