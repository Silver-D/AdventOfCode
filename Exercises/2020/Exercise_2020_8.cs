using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_8 : Exercise
    {
        public Exercise_2020_8(byte part) : base("2020", "8", part)
        {
            ParseInput();
        }

        static Dictionary<string, int> opcs = new Dictionary<string, int>() { {"nop", 0}, {"acc", 1}, {"jmp", 2} };
        static List<KeyValuePair<int, int>> data = new List<KeyValuePair<int, int>>();

        void ParseInput()
        {
            StringReader sr = new StringReader(input);

            string line;

            while((line = sr.ReadLine()) != null)
            {
                string[] inst = line.Split(' ');

                data.Add(new KeyValuePair<int, int>(opcs[inst[0]], int.Parse(inst[1])));
            }
        }

        private bool RunCode(out int acc)
        {
            acc = 0;

            int idx = 0;
            int num = data.Count();

            List<int> exec = new List<int>();

            Console.WriteLine("NEW LOOP");

            while(idx < num)
            {
                Console.WriteLine(idx + ": " + data[idx].Key + " " + data[idx].Value);
                if (exec.Contains(idx))
                return false;

                exec.Add(idx);

                if (data[idx].Key != opcs["nop"])
                {
                    if (data[idx].Key == opcs["acc"])
                    acc += data[idx].Value;

                    if (data[idx].Key == opcs["jmp"])
                    {
                        idx += data[idx].Value;

                        continue;
                    }
                }

                idx++;
            }

            return true;
        }

        protected override string Part_1()
        {
            RunCode(out int acc);

            return acc.ToString();
        }

        protected override string Part_2()
        {
            int lastFix = -1;
            int acc     = 0;

            static void applyFix(int idx)
            {
                int opc = data[idx].Key;

                Console.WriteLine("APPLYING CHANGE TO " + idx);

                opc = (opc == opcs["nop"]) ? opcs["jmp"] : opcs["nop"];

                data[idx] = new KeyValuePair<int, int>(opc, data[idx].Value);
            }
            
            while(!RunCode(out acc))
            {
                if (lastFix >= 0)
                applyFix(lastFix);

                lastFix = data.FindIndex((lastFix == -1) ? 0 : lastFix + 1, e => (e.Value != 0 && (e.Key == opcs["jmp"] || e.Key == opcs["nop"])));

                if (lastFix >= 0)
                applyFix(lastFix);
            }

            return acc.ToString();
        }
    }
}