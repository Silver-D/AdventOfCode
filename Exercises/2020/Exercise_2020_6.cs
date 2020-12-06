using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_6 : Exercise
    {
        public Exercise_2020_6(byte part) : base("2020", "6", part)
        {
            ParseInput();
        }

        private List<List<char>> data = new List<List<char>>();

        private void ParseInput()
        {
            List<char> answers = new List<char>();

            int people = 0;

            StringReader sr = new StringReader(input);

            string line;

            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);

                if (line.Length == 0)
                {
                    Console.WriteLine(answers.Count());

                    data.Add(answers);

                    answers = new List<char>();
                    people  = 0;

                    continue;
                }

                if (part == 2)
                people++;

                for (int c = 0; c < line.Length; c++)
                {
                    if (!answers.Contains(line[c]))
                    {
                        if (part == 2 && people > 1)
                        continue;

                        answers.Add(line[c]);
                    }
                }

                if (part == 2 && people > 1)
                answers = answers.Intersect(line.ToCharArray()).ToList();
            }

            Console.WriteLine("\n" + answers.Count());

            data.Add(answers);
        }

        protected override string Part_1()
        {
            return data.Aggregate(0, (t, v) => (t += v.Count())).ToString();
        }

        protected override string Part_2()
        {
            return Part_1();
        }
    }
}