using System;
using System.Collections.Generic;
using System.Linq;

namespace VillageRTS
{
    public abstract class Building
    {
        static Building()
        {
            foreach (var type in typeof(Building).Assembly.GetTypes().Where(x => x.BaseType == typeof(Building))
                .Select((x, i) => ((x.GetConstructor(new Type[0]).Invoke(new object[0]) as Building).SetName(x.Name), i)).ToArray()) Buildings.Add(type.Item2, type.Item1);
        }
        public static Building GetBuildingByName(string name)
        {
            foreach (var x in Buildings.Values)
                if (x.Name == name) return x;
            throw new ArgumentException("Wrong param", "name");
        }
        public static int GetBuildingIndexByName(string name)
        {
            var i = 0;
            foreach (var x in Buildings.Values)
            {
                if (x.Name == name) return i;
                i++;
            }
            throw new ArgumentException("Wrong param", "name");
        }
        public static Dictionary<int, Building> Buildings { get; set; } = new Dictionary<int, Building>();
        public virtual int MaxCount => int.MaxValue;
        public virtual bool Indestructible => false;
        internal Building SetName(string name)
        {
            Name = name;
            return this;
        }
        public string Name { get; set; }
        public bool HasAll(Gameplay gameplay, int count) => GetProduction(gameplay).All(x => x.Value.Item2 > 0 || gameplay.Current[x.Key] >= x.Value.Item2 * count);
        public void OnBuilding(Gameplay gameplay)
        {
            foreach (var x in Cost)
                gameplay.Current[x.Key] += x.Value;
        }
        public void OnTick(Gameplay gameplay)
        {
            foreach (var x in GetProduction(gameplay))
                if (gameplay.Ticks % x.Value.Item1 == 0)
                    gameplay.Current[x.Key] += x.Value.Item2;
        }
        public void OnDestruction(Gameplay gameplay)
        {
            foreach (var x in Cost)
                gameplay.Current[x.Key] -= x.Value;
        }
        public abstract Dictionary<Resource, int> Cost { get; protected set; }
        public virtual IEnumerable<KeyValuePair<Resource, double>> GetMultipliers(Gameplay gameplay)
        {
            yield break;
        }
        public virtual IEnumerable<KeyValuePair<Resource, int>> GetAdditions(Gameplay gameplay)
        {
            yield break;
        }
        public double GetMultiplier(Gameplay gameplay, Resource resource)
        {
            var mults = GetMultipliers(gameplay);
            if (mults == null) return 1.0D;
            foreach (var x in mults)
                if (resource == x.Key) return x.Value;
            return 1.0D;
        }
        public int GetAddition(Gameplay gameplay, Resource resource)
        {
            var adds = GetAdditions(gameplay);
            if (adds == null) return 0;
            foreach (var x in adds)
                if (resource == x.Key) return x.Value;
            return 0;
        }
        public IEnumerable<KeyValuePair<Resource, (int, int)>> GetProduction(Gameplay gameplay)
        {
            foreach (var x in InternalProduction)
                yield return new KeyValuePair<Resource, (int, int)>
                    (x.Key, (x.Value.Item1, (int)Math.Round(x.Value.Item2 + GetAddition(gameplay, x.Key) * GetMultiplier(gameplay, x.Key))));
        }
        public abstract Dictionary<Resource, (int, int)> InternalProduction { get; protected set; }
    }

    #region My Buildings

    public class I_am_King: Building
    {
        public override int MaxCount => 1;
        public override bool Indestructible => true;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {

        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, +2) },
            { Resource.My_Food, (1, -1) },
            { Resource.My_Fuel, (1, -1) }
        };
    }

    public class Main_House : Building
    {
        public override int MaxCount => 1;
        public override bool Indestructible => true;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Wood, 32 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, +1) },
            { Resource.My_Food, (1, +1) },
            { Resource.My_Fuel, (1, +1) }
        };
    }

    public class My_House : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Wood, -64 },
            { Resource.My_Power, -32 },
            { Resource.My_HumansLimit, 6 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Humans, (30, +1) },
            { Resource.My_Food, (1, -1) },
            { Resource.My_Fuel, (1, -1) }
        };
    }

    #endregion

    #region Humans Buildings

    public class TownHall : Building
    {
        public override int MaxCount => 1;
        public override bool Indestructible => true;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Wood, -96 },
            { Resource.My_Power, -48 },
            { Resource.Village_HumansLimit, 12 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.Village_Humans, (30, +1) },
            { Resource.Village_HumansLimit, (30, -1) },
            { Resource.Village_Gold, (1, +1) },
            { Resource.Village_Food, (1, +1) },
            { Resource.Village_Fuel, (1, +1) },
        };
    }

    public class House : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Gold, +544 },
            { Resource.My_Wood, -64 },
            { Resource.My_Power, -32 },
            { Resource.Village_HumansLimit, 6 },
            { Resource.Village_Gold, -544 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.Village_Humans, (30, +1) },
            { Resource.Village_HumansLimit, (30, -1) },
            { Resource.Village_Food, (1, -1) },
            { Resource.Village_Fuel, (1, -1) }
        };
    }


    #endregion
}
