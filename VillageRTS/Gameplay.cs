using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System;
using System.IO;

namespace VillageRTS
{

    public enum Resource
    {
        //My:
        My_Gold,
        My_HumansLimit,
        My_Humans,
        My_Power,

        //External Trading = x.25

        My_Wood, //8 Gold
        My_Stone, //24 Gold
        My_Iron, //32 Gold

        //Work (1 tick) = 1 Gold /human

        My_Food, //1 Gold
        My_Fuel, //1 Gold
        My_Tools, //36 Gold = Wood + Iron + 2 Work
        My_Clothes, // 50 Gold = Leather + 2 Work
        My_Leather, //48 Gold

        //Village
        Village_Gold,
        Village_HumansLimit,
        Village_Humans,

        Village_Wood,
        Village_Stone,
        Village_Iron,

        Village_Food,
        Village_Fuel,
        Village_Tools,
        Village_Clothes,
        Village_Leather,
    }

    public class Gameplay
    {
        #region Saving
        public const string QuickSave = "quicksave.dat";
        public void Save() => Save(QuickSave);
        public Gameplay TryLoad()
        {
            if (File.Exists(QuickSave)) return Load();
            return this;
        }
        public Gameplay Load() => Load(QuickSave);
        public void Save(string path)
        {
            using (var stream = File.OpenWrite(path))
            using (var bw = new BinaryWriter(stream)) WriteInto(bw);
        }
        public Gameplay Load(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var br = new BinaryReader(stream)) return ReadFrom(br);
        }

        public void WriteInto(BinaryWriter bw)
        {
            bw.Write(Ticks);

            bw.Write(Current.Count);
            foreach (var c in Current)
            {
                bw.Write((int)c.Key);
                bw.Write(c.Value);
            }

            bw.Write(Buildings.Count);
            foreach (var b in Buildings)
            {
                bw.Write(b.Key);
                bw.Write(b.Value);
            }
        }
        public Gameplay ReadFrom(BinaryReader br)
        {
            Ticks = br.ReadInt64();

            var c = br.ReadInt32();
            for (var i = 0; i < c; i++) Current[(Resource)br.ReadInt32()] = br.ReadInt32();

            var b = br.ReadInt32();
            for (var i = 0; i < b; i++) Buildings[br.ReadInt32()] = br.ReadInt32();

            return this;
        }
        #endregion
        public long Ticks { get; private set; } = 0;
        public Dictionary<Resource, int> Current { get; set; } = new Dictionary<Resource, int>();
        public Dictionary<int, int> Buildings { get; set; } = new Dictionary<int, int>();
        public Gameplay Init()
        {
            Buildings = Building.Buildings.Keys.ToDictionary(x => x, x => 0);
            Current = Enum.GetNames(typeof(Resource)).ToDictionary(x => (Resource)Enum.Parse(typeof(Resource), x), x => 0);

            Current[Resource.My_Fuel] = 300;
            Current[Resource.My_Food] = 300;
            Buildings[0] = 1;
            Current[Resource.My_Power] = 0;
            return this;
        }

        public bool GameOver()
        {
            foreach (var x in Current)
                if (x.Value < -50) return true;
            return false;
        }

        public void Tick()
        {
            foreach (var x in Buildings)
            {
                var building = Building.Buildings[x.Key];
                for (var i = 0; i < x.Value; i++)
                    if (building.GetProduction(this).All(y => y.Value.Item2 >= 0 || Current[y.Key] >= -y.Value.Item2))
                        building.OnTick(this);
            }
            Ticks++;
        }
    }
}
