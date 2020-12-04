using System;
using System.Net.Http;

namespace AdventOfCode
{
    static class Program
    {
        readonly public static HttpClient http = new HttpClient();

        static void Main()
        {
            Exercise_2020_4 exersize = new Exercise_2020_4(part: 2);

            string answer = exersize.Run();

            string problem = exersize.year + "-" + exersize.day + " part " + exersize.part;

            if (answer.Length > 0)
            Console.WriteLine("\nYour answer to " + problem + " is: " + answer);

            else
            Console.WriteLine("\nNo answer was produced for " + problem);
        }
    }
}
