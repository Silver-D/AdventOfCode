using System;
using System.Linq;

namespace AdventOfCode
{
    static class Exersize_2020_1
    {
        private static string[] data;

        public static void ParseInput(string input)
        {
            data = input.Split('\n');
        }

        public static string Part_1()
        {
            foreach(string d in data)
            {
                int num     = int.Parse(d);
                string find = (2020 - num).ToString();

                if (data.Contains(find))
                return (num * int.Parse(find)).ToString();
            }

            return "";
        }
    }
}