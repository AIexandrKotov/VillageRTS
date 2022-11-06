using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRTS
{
    #region Изменение окружения

    //      Вырубить лес      //
    public class CutForest : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1000 },
            { Resource.Env_Forests, -1 },
            { Resource.My_Wood, +1000 },
            { Resource.Env_Free_Space, +8 },
        };
    }

    //      Создать поле      //
    //      Освободить поле      //
    //      Исследование окружения      //
    #endregion

    #region Внешняя торговля
    public class ET_BuyWood : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Wood, +1 },
            { Resource.My_Gold, -8 },
        };
    }
    public class ET_SellWood : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Wood, -1 },
            { Resource.My_Gold, +4 },
        };
    }
    public class ET_BuyStone : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Stone, +1 },
            { Resource.My_Gold, -24 },
        };
    }
    public class ET_SellStone : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Stone, -1 },
            { Resource.My_Gold, +12 },
        };
    }
    public class ET_BuyIron : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Iron, +1 },
            { Resource.My_Gold, -32 },
        };
    }
    public class ET_SellIron : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Iron, -1 },
            { Resource.My_Gold, +16 },
        };
    }
    public class ET_BuyInstruments : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Tools, +1 },
            { Resource.My_Gold, -42 },
        };
    }
    public class ET_SellInstruments : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Tools, -1 },
            { Resource.My_Gold, +21 },
        };
    }
    #endregion

    #region Торговля с деревней
    #endregion

    #region Торговля строениями
    /*public class Sell_House : Action
    {
        public override Dictionary<int, int> BuildingAction { get; protected set; } = new Dictionary<int, int>()
        {
            { Building.GetBuildingIndexByName("House"), -1 },
            { Building.GetBuildingIndexByName("Village_House"), +1 },
        };
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Gold, +544 },
            { Resource.Village_Gold, -544 },
            { Resource.My_HumansLimit, -6 },
            { Resource.Village_HumansLimit, +6 },
        };
    }*/

    #endregion

    #region Строения игрока

    public class IamArrive : Building
    {
        public override int MaxCount => 1;
        public override bool Indestructible => true;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {

        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, +1) }
        };
    }

    public class IamHere : Building
    {
        public override int MaxCount => 1;
        public override bool Indestructible => true;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Gold, -500 },
            { Resource.My_Humans, +8 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, +8) }
        };
    }

    public class Administation : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Wood, -48 },
            { Resource.My_Stone, -24 },
            { Resource.My_Iron, -8 },
            { Resource.My_Power, -660 },
            { Resource.My_Humans, -6 },
            { Resource.Env_Free_Space, -1 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, +6) }
        };
    }

    public class MyCollectEnvironment : Building
    {
        public override int MaxCount => 1;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {

        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, -1) }
        };

        internal static RandChooser<KeyValuePair<Resource, (int, int)>> rnd = new RandChooser<KeyValuePair<Resource, (int, int)>>(
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Wood, (7, 1)), 0.4),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Stone, (7, 1)), 0.2),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Iron, (7, 1)), 0.15),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Food, (7, 1)), 0.3),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Gold, (7, 1)), 0.1)
        );

        public override IEnumerable<KeyValuePair<Resource, (int, int)>> GetProduction(Gameplay gameplay)
        {
            foreach (var x in base.GetProduction(gameplay)) yield return x;
            yield return rnd.GetItem();
        }
    }

    public class CollectEnvironment : Building
    {
        public override int MaxCount => 1;
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Humans, -8 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, -8) }
        };

        internal static RandChooser<KeyValuePair<Resource, (int, int)>> rnd = new RandChooser<KeyValuePair<Resource, (int, int)>>(
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Wood, (1, 1)), 0.4),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Stone, (1, 1)), 0.2),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Iron, (1, 1)), 0.15),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Food, (1, 1)), 0.3),
            (new KeyValuePair<Resource, (int, int)>(Resource.My_Gold, (1, 1)), 0.1)
        );

        public override IEnumerable<KeyValuePair<Resource, (int, int)>> GetProduction(Gameplay gameplay)
        {
            foreach (var x in base.GetProduction(gameplay)) yield return x;
            yield return rnd.GetItem();
        }
    }

    public class House : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Wood, -32 },
            { Resource.My_HumansLimit, +6 },
            { Resource.Env_Free_Space, -1 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Humans, (30, +1) },
            { Resource.My_HumansLimit, (30, -1) },
            { Resource.My_Power, (1, -1) },
            { Resource.My_Food, (1, -1) },
            { Resource.My_Fuel, (1, -1) },
        };
    }

    public class Field : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Humans, -6 },
            { Resource.My_Tools, -24 },
            { Resource.Env_Fields, -1 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, -1) },
            { Resource.My_Food, (1, +16) },
        };
    }

    public class Foresters : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Humans, -2 },
            { Resource.My_Tools, -8 },
            { Resource.Env_Forests, -1 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, -1) },
            { Resource.My_Wood, (1, +3) },
        };
    }

    public class Woodcutting : Building
    {
        public override Dictionary<Resource, int> Cost { get; protected set; } = new Dictionary<Resource, int>()
        {
            { Resource.My_Wood, -24 },
            { Resource.My_Tools, -4 },
            { Resource.Env_Free_Space, -1 },
        };
        public override Dictionary<Resource, (int, int)> InternalProduction { get; protected set; } = new Dictionary<Resource, (int, int)>()
        {
            { Resource.My_Power, (1, -1) },
            { Resource.My_Wood, (1, -1) },
            { Resource.My_Fuel, (1, +5) },
        };
    }

    #endregion

    #region Строения деревни

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

    public class Village_House : Building
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
