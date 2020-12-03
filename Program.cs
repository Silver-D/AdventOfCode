using System;
using System.Net.Http;

namespace AdventOfCode
{
    static class Program
    {
        readonly public static HttpClient http = new HttpClient();

        static void Main()
        {
            Exersize_2020_1 exersize = new Exersize_2020_1(part: 2);

            string answer = exersize.Run();

            string problem = exersize.year + "-" + exersize.day + " part " + exersize.part;

            if (answer.Length > 0)
            Console.WriteLine("Your answer to " + problem + " is: " + answer);

            else
            Console.WriteLine("No answer was produced for " + problem);
        }
    }
}
