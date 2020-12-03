using System;
using System.Linq;

namespace AdventOfCode
{
    class Exersize_2020_1 : Exersize
    {
        private static string[] data;

        public Exersize_2020_1(byte part) : base("2020", "1", part)
        {
            ParseInput();
        }

        private void ParseInput()
        {
            data = input.Split('\n');
        }

        override protected string Part_1()
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

        override protected string Part_2()
        {
            int[] numbers = Array.ConvertAll(data, item => int.Parse(item));
            int   needed  = 2020;

            for (int a = 0; a < numbers.Length; a++)
            {
                for (int b = a; b < numbers.Length; b++)
                {
                    if (numbers[a] + numbers[b] > needed)
                    continue;

                    for (int c = b; c < numbers.Length; c++)
                    {
                        if (numbers[a] + numbers[b] + numbers[c] == needed)
                        return (numbers[a] * numbers[b] * numbers[c]).ToString();
                    }
                }
            }

            return "";
        }
    }
}