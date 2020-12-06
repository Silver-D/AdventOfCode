using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_5 : Exercise
    {
        public Exercise_2020_5(byte part) : base("2020", "5", part)
        {
            ParseInput();
        }

        private List<Dictionary<string, int>> data = new List<Dictionary<string, int>>();

        private void ParseInput()
        {
            int lower;
            int upper;

            int HalfRange(bool lowerHalf)
            {
                if (lowerHalf)
                return (upper = (lower + upper) / 2);

                return (lower = (lower + upper + 1) / 2);
            }

            string[] lines = input.Split('\n');

            foreach(string line in lines)
            {
                Dictionary<string, int> entry = new Dictionary<string, int>();

                int last = 0;

                lower = 0;
                upper = 127;

                for (int c = 0; c < 7; c++)
                    last = HalfRange(line[c] == 'F');

                entry.Add("row", last);

                last  = 0;
                lower = 0;
                upper = 7;

                for (int c = 7; c < 10; c++)
                    last = HalfRange(line[c] == 'L');

                entry.Add("col", last);

                entry.Add("seat", entry["row"] * 8 + entry["col"]);

                data.Add(entry);
            }
            
        }

        protected override string Part_1()
        {
            return data.Aggregate(1, (l, v) => (v["seat"] > l) ? v["seat"] : l).ToString();
        }

        protected override string Part_2()
        {
            List<int> seats = new List<int>();

            data.ForEach(d => seats.Add(d["seat"]));

            foreach(int seat in seats)
            {
                int find = seat + 1;

                if (seats.Contains(find + 1) && !seats.Contains(find))
                return (find).ToString();
            }

            return "";
        }
    }
}