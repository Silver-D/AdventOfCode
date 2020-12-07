using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Exercise_2020_7 : Exercise
    {
        public Exercise_2020_7(byte part) : base("2020", "7", part)
        {
            ParseInput();
        }

        class Bag
        {
            readonly string id;

            readonly Dictionary<Bag, int>  holds;
            readonly Dictionary<Bag, bool> holdsEventually;

            public Bag(string id)
            {
                this.id = id;

                this.holds = new Dictionary<Bag, int>(new BagEqualityComparer());

                this.holdsEventually = new Dictionary<Bag, bool>(new BagEqualityComparer());
            }

            class BagEqualityComparer : IEqualityComparer<Bag>
            {
                public int GetHashCode(Bag bag) { return bag.id.GetHashCode(); }

                public bool Equals(Bag bag1, Bag bag2) { return bag1.id == bag2.id; }
            }

            public void AddBag(string id, int count)
            {
                Bag bag = BagCollection.ById(id);

                if (holds.ContainsKey(bag))
                holds[bag] += count;

                else
                holds.Add(bag, count);
            }

            bool CanHoldDirectly(Bag bag)
            {
                return holds.ContainsKey(bag);
            }

            public bool CanHold(Bag bag)
            {
                if (CanHoldDirectly(bag))
                return true;

                if (holdsEventually.ContainsKey(bag))
                return holdsEventually[bag];

                foreach(Bag enclosed in holds.Keys)
                {
                    if (enclosed.CanHold(bag))
                    {
                        if (!enclosed.holdsEventually.ContainsKey(bag))
                        enclosed.holdsEventually.Add(bag, true);

                        return true;
                    }

                }

                holdsEventually.Add(bag, false);

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

        static class BagCollection
        {
            readonly public static Dictionary<string, Bag> collection;

            static BagCollection()
            {
                collection = new Dictionary<string, Bag>();
            }

            public static Bag ById(string id)
            {
                if (!collection.TryGetValue(id, out Bag bag))
                {
                    bag = new Bag(id);

                    collection.Add(id, bag);
                }

                return bag;
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
                int    number = 0;
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

                    if (int.TryParse(w, out int count))
                    {
                        number = count;

                        continue;
                    }

                    if (w.Contains("bag"))
                    {
                        if (number > 0)
                        {
                            Console.WriteLine("Adding " + number + " " + bag_id);

                            bag.AddBag(bag_id, number);

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
            Bag find_bag = BagCollection.ById("shinygold");
            int count    = 0;

            foreach(Bag bag in BagCollection.collection.Values)
            {
                if (bag.CanHold(find_bag))
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