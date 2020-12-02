using System;
using System.Net.Http;

namespace AdventOfCode
{
    static class Program
    {
        readonly public static HttpClient http = new HttpClient();

        static void Main()
        {
            string year = "2020";
            string day  = "1";
            byte   part = 1;

            Exersize.SetExersize(year, day);

            string problem = year + "-" + day + " part " + part;
            string answer  = Exersize.Run(part);

            if (answer.Length > 0)
            Console.WriteLine("Your answer to " + problem + " is: " + answer);

            else
            Console.WriteLine("No answer was produced for " + problem);
        }
    }
}
