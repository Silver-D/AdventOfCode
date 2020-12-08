using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_7 : Exercise
    {
        public Exercise_2020_7(byte part) : base("2020", "7", part)
        {
            ParseInput();
        }

        static class BagCollection
        {
            readonly static Dictionary<string, Bag> entries;

            public static List<Bag> Bags => entries.Values.ToList();

            static BagCollection()
            {
                entries = new Dictionary<string, Bag>();
            }

            public static Bag ById(string id)
            {
                if (!entries.TryGetValue(id, out Bag bag))
                bag = new Bag(id);

                return bag;
            }

            public static void AddBag(Bag bag)
            {
                try
                {
                    entries.Add(bag.id, bag);
                }
                catch(ArgumentException)
                {
                    Console.WriteLine("A Bag with id " + bag.id + " already exists in BagCollection or not of valid id");

                    throw new ArgumentException();
                }
            }
        }

        class Bag
        {
            readonly public string id;

            readonly Dictionary<Bag, int>  holds;
            readonly Dictionary<Bag, bool> holdsEver;

            class BagEqualityComparer : IEqualityComparer<Bag>
            {
                public int GetHashCode(Bag bag) { return bag.id.GetHashCode(); }

                public bool Equals(Bag bag1, Bag bag2) { return bag1.id == bag2.id; }
            }

            public Bag(string id)
            {
                this.id = id;

                holds     = new Dictionary<Bag, int>(new BagEqualityComparer());
                holdsEver = new Dictionary<Bag, bool>(new BagEqualityComparer());

                BagCollection.AddBag(this);
            }

            public void AddContents(string id, int count)
            {
                Bag bag = BagCollection.ById(id);

                if (holds.ContainsKey(bag))
                holds[bag] += count;

                else
                {
                    holds.Add(bag, count);
                    holdsEver.Add(bag, true);
                }
            }

            public bool CanHold(Bag bag)
            {
                if (holdsEver.ContainsKey(bag))
                return holdsEver[bag];

                foreach(Bag enclosed in holds.Keys)
                {
                    if (enclosed.CanHold(bag))
                    {
                        if (!enclosed.holdsEver.ContainsKey(bag))
                        enclosed.holdsEver.Add(bag, true);

                        return true;
                    }

                }

                holdsEver.Add(bag, false);

                return false;
            }

            public int GetTotalBagCount(int mult = 1)
            {
                int count = 0;

                foreach(KeyValuePair<Bag, int> kvp in holds)
                {
                    count += kvp.Value * mult;

                    count += kvp.Key.GetTotalBagCount(kvp.Value * mult);
                }

                return count;
            }
        }

        void ParseInput()
        {
            StringReader sr = new StringReader(input);

            string line;

            while((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(' ');

                string bag_id = "";
                int    count  = 0;
                Bag    bag    = null;

                foreach(string w in words)
                {
                    if (w == "contain")
                    {
                        Console.WriteLine("Adding bag " + bag_id);

                        bag = BagCollection.ById(bag_id);

                        bag_id = "";

                        continue;
                    }

                    if (int.TryParse(w, out int number))
                    {
                        count = number;

                        continue;
                    }

                    if (w.Contains("bag"))
                    {
                        if (count > 0)
                        {
                            Console.WriteLine("Adding " + count + " " + bag_id);

                            bag.AddContents(bag_id, count);

                            bag_id = "";
                        }

                        continue;
                    }

                    bag_id += w;
                }
            }
        }

        protected override string Part_1()
        {
            Bag findBag = BagCollection.ById("shinygold");
            int count   = 0;

            foreach(Bag bag in BagCollection.Bags)
            {
                if (bag.CanHold(findBag))
                count++;
            }

            return count.ToString();
        }

        protected override string Part_2()
        {
            Bag myBag = BagCollection.ById("shinygold");

            return myBag.GetTotalBagCount().ToString();
        }
    }
}