using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System;

namespace VillageRTS
{

    public enum Resource
    {
        //My:
        My_Gold,
        My_HumansLimit,
        My_Humans,
        My_Power,

        My_Wood,
        My_Stone,
        My_Iron,

        My_Food,
        My_Fuel,
        My_Tools,
        My_Clothes,

        //Village
        Humans_Gold,
        Humans_HumansLimit,
        Humans_Humans,

        Humans_Wood,
        Humans_Stone,
        Humans_Iron,

        Humans_Food,
        Humans_Fuel,
        Humans_Tools,
        Humans_Clothes,
    }

    public class Gameplay
    {
        public Gameplay()
        {

        }

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

        public Dictionary<Resource, int> Current { get; set; } = new Dictionary<Resource, int>();
        public Dictionary<int, int> Buildings { get; set; } = new Dictionary<int, int>();

        public bool GameOver()
        {
            foreach (var x in Current)
                if (x.Value < -50) return true;
            return false;
        }

        public long Ticks { get; private set; } = 0;
        public void Tick()
        {
            foreach (var x in Buildings)
            {
                var building = Building.Buildings[x.Key];
                for (var i = 0; i < x.Value; i++)
                    if (building.Production.All(y => y.Value.Item2 >= 0 || Current[y.Key] >= -y.Value.Item2))
                        building.OnTick(this);
            }
            Ticks++;
        }
    }
}
