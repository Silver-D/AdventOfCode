using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_3 : Exercise
    {
        private bool[,] data;

        private int max_x  = 0;
        private int max_y  = 0;
        private int step_x = 0;
        private int step_y = 0;

        public Exercise_2020_3(byte part) : base("2020", "3", part)
        {
            ParseInput();
        }

        private void ParseInput()
        {
            string[] lines = input.Split('\n');

            max_y = lines.Length;

            for (int y = 0; y < max_y; y++)
            {
                char[] line = lines[y].ToCharArray();

                if (max_x == 0)
                {
                    max_x = line.Length;

                    data = new bool[max_x, max_y];
                }

                for (int x = 0; x < max_x; x++)
                data[x, y] = (line[x] == '#');
            }
        }

        override protected string Part_1()
        {
            int trees = 0;
            int x     = 0;

            if (step_x == 0)
            step_x = 3;

            if (step_y == 0)
            step_y = 1;

            for (int y = step_y; y < max_y; y += step_y)
            {
                x += step_x;

                if (x >= max_x)
                x -= max_x;

                if (y >= max_y)
                continue;

                if (data[x, y])
                trees++;
            }

            return trees.ToString();
        }

        override protected string Part_2()
        {
            List<int[]> steps = new List<int[]>()
            {
                new int[] {1, 1},
                new int[] {3, 1},
                new int[] {5, 1},
                new int[] {7, 1},
                new int[] {1, 2}
            };

            int[] trees = new int[steps.Count()];

            for (int i = 0; i < trees.Length; i++)
            {
                step_x = steps[i][0];
                step_y = steps[i][1];

                trees[i] = int.Parse(Part_1());
            }

            return trees.Aggregate((long)1, (t, v) => t * v).ToString();
        }
    }
}