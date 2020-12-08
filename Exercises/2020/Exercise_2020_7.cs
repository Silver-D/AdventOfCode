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
            readonly Dictionary<Bag, bool> holdsCache;

            class BagEqualityComparer : IEqualityComparer<Bag>
            {
                public int GetHashCode(Bag bag) { return bag.id.GetHashCode(); }

                public bool Equals(Bag bag1, Bag bag2) { return bag1.id == bag2.id; }
            }

            readonly static BagEqualityComparer dictKeyComparer = new BagEqualityComparer();

            public Bag(string id)
            {
                this.id = id;

                holds      = new Dictionary<Bag, int>(dictKeyComparer);
                holdsCache = new Dictionary<Bag, bool>(dictKeyComparer);

                BagCollection.AddBag(this);
            }

            public void AddContents(Bag bag, int count)
            {
                if (holds.ContainsKey(bag))
                return;

                holds.Add(bag, count);
                holdsCache.Add(bag, true);
            }

            public bool CanHold(Bag bag)
            {
                if (holdsCache.ContainsKey(bag))
                return holdsCache[bag];

                foreach(Bag enclosed in holds.Keys)
                {
                    if (enclosed.CanHold(bag))
                    {
                        holdsCache.Add(bag, true);

                        return true;
                    }

                }

                holdsCache.Add(bag, false);

                return false;
            }

            public int GetTotalBagCount(int mult = 1)
            {
                int count = 0;

                foreach(KeyValuePair<Bag, int> kvp in holds)
                {
                    int num = kvp.Value * mult;

                    count += num;
                    count += kvp.Key.GetTotalBagCount(num);
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

                            bag.AddContents(BagCollection.ById(bag_id), count);

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
            Bag myBag = BagCollection.ById("shinygold");
            int count = 0;

            foreach(Bag bag in BagCollection.Bags.Where(e => (e.id != myBag.id)))
            {
                if (bag.CanHold(myBag))
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