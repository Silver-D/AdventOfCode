using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_4 : Exercise
    {
        public Exercise_2020_4(byte part) : base("2020", "4", part)
        {
            ParseInput();
        }

        private List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        private void ParseInput()
        {
            Dictionary<string, string> entry = new Dictionary<string, string>();

            StringReader sr = new StringReader(input);

            string line;

            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);

                if (line.Length == 0)
                {
                    data.Add(entry);

                    entry = new Dictionary<string, string>();

                    continue;
                }

                string[] pairs = line.Split(' ');
                
                foreach(string p in pairs)
                {
                    string[] pair = p.Split(':');

                    entry.Add(pair[0], pair[1]);
                }
            }

            data.Add(entry);
        }

        protected override string Part_1()
        {
            List<string> req_fields = new List<string>()
            {
                "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"
            };

            int total_verified = 0;

            foreach(Dictionary<string, string> entry in data)
            {
                bool verified = true;

                foreach(string f in req_fields)
                {
                    if (!entry.ContainsKey(f))
                    {
                        verified = false;
                        break;
                    }
                }

                if (verified)
                total_verified++;
            }

            return total_verified.ToString();
        }

        protected override string Part_2()
        {
            static bool InRange (string strValue, int start, int end)
            {
                int value = int.Parse(strValue);

                return (value >= start && value <= end);
            }

            List<string> req_fields = new List<string>() { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            List<string> eye_colors = new List<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            int total_verified = 0;

            foreach(Dictionary<string, string> entry in data)
            {
                bool verified = true;

                foreach(string f in req_fields)
                {
                    if (!entry.TryGetValue(f, out string value))
                    {
                        verified = false;
                        break;
                    }

                    value = value.Trim();

                    if (value.Length == 0)
                    {
                        verified = false;
                        break;
                    }

                    if (f == "byr" && !InRange(value, 1920, 2002))
                    verified = false;

                    else if (f == "iyr" && !InRange(value, 2010, 2020))
                    verified = false;

                    else if (f == "eyr" && !InRange(value, 2020, 2030))
                    verified = false;

                    else if (f == "hgt")
                    {
                        if (value.Length < 4)
                        {
                            verified = false;
                            break;
                        }

                        string ut = value.Substring(value.Length - 2).ToLower();
                        string uv = value.Substring(0, value.Length - 2);

                        if (!((ut == "cm" && InRange(uv, 150, 193)) || (ut == "in" && InRange(uv, 59, 76))))
                        {
                            verified = false;
                            break;
                        }
                    }

                    else if (f == "hcl")
                    {
                        if (value[0] != '#' || value.Length != 7)
                        {
                            verified = false;
                            break;
                        }

                        if (!int.TryParse(value.Substring(1, 6), System.Globalization.NumberStyles.HexNumber, null, out int blank))
                        {
                            verified = false;
                            break;
                        }
                    }

                    else if (f == "ecl" && !eye_colors.Contains(value))
                    verified = false;

                    else if (f == "pid" && (value.Length != 9 || !int.TryParse(value, out int blank)))
                    verified = false;
                }

                if (verified)
                total_verified++;
            }

            return total_verified.ToString();
        }
    }
}