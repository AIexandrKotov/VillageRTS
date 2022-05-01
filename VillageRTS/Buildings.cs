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
        public static Dictionary<int, Building> Buildings { get; set; } = new Dictionary<int, Building>();
        public virtual int MaxCount => int.MaxValue;
        public virtual bool Indestructible => false;
        internal Building SetName(string name)
        {
            Name = name;
            return this;
        }
        public string Name { get; set; }
        public bool HasAll(Gameplay gameplay, int count) => Production.All(x => x.Value.Item2 > 0 || gameplay.Current[x.Key] >= x.Value.Item2 * count);
        public void OnBuilding(Gameplay gameplay)
        {
            foreach (var x in Cost)
                gameplay.Current[x.Key] += x.Value;
        }
        public void OnTick(Gameplay gameplay)
        {
            foreach (var x in Production)
                if (gameplay.Ticks % x.Value.Item1 == 0)
                    gameplay.Current[x.Key] += x.Value.Item2;
        }
        public void OnDestruction(Gameplay gameplay)
        {
            foreach (var x in Cost)
                    gameplay.Current[x.Key] -= x.Value;
        }
        public abstract Dictionary<Resource, int> Cost { get; protected set; }
        public abstract Dictionary<Resource, (int, int)> Production { get; protected set; }
    }

    #region My Buildings

    public class I_am_King: Building
    {
        public override int MaxCount => 1;
        public override bool Indestructible => true;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {

        };
        public override Dictionary<Resource, (int, int)> Production { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, +1) },
            { Resource.My_Food, (1, -1) },
            { Resource.My_Fuel, (1, -1) }
        };
    }

    #endregion

    #region Humans Buildings

    public class TH : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>() {

        };
        public override Dictionary<Resource, (int, int)> Production { get; protected set; } = new Dictionary<Resource, (int, int)>() {

        };
    }

    #endregion
}
