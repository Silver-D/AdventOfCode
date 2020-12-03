using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_2 : Exercise
    {
        private struct Line
        {
            public int    lnum;
            public int    rnum;
            public char   letter;
            public char[] password;
        }

        private List<Line> data = new List<Line>();

        public Exercise_2020_2(byte part) : base("2020", "2", part)
        {
            ParseInput();
        }

        private void ParseInput()
        {
            string[] lines = input.Split('\n');
            string pattern  = @"(\d+)-(\d+) (\w): (\w+)";

            foreach(string l in lines)
            {
                Console.WriteLine(l);

                Match matches = new Regex(pattern).Match(l);

                Line entry = new Line()
                {
                    lnum     = int.Parse(matches.Groups[1].Value),
                    rnum     = int.Parse(matches.Groups[2].Value),
                    letter   = matches.Groups[3].Value.ToCharArray()[0],
                    password = matches.Groups[4].Value.ToCharArray()
                };

                data.Add(entry);
            }
        }

        override protected string Part_1()
        {
            int verified = 0;

            foreach(Line l in data)
            {
                int count = l.password.Where(letter => (letter == l.letter)).Count();

                if (count >= l.lnum && count <= l.rnum)
                verified++;
            }

            return verified.ToString();
        }

        override protected string Part_2()
        {
            int verified = 0;

            foreach(Line l in data)
            {
                int count = 0;

                if (l.password[l.lnum - 1] == l.letter)
                count++;

                if (l.password[l.rnum - 1] == l.letter)
                count++;

                if (count == 1)
                verified++;
            }

            return verified.ToString();
        }
    }
}