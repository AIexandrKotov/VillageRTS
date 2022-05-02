using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRTS
{
    public abstract class Action
    {
        private static Action[] GetAllActions() =>
            typeof(Action).Assembly.GetTypes().Where(x => x.BaseType == typeof(Action))
            .Select(x => x.GetConstructor(new Type[0]).Invoke(new object[0]) as Action).ToArray();
        public static Action[] Actions = GetAllActions();
        public abstract Dictionary<Resource, int> ResourceAction { get; protected set; }
        public virtual Dictionary<int, int> BuildingAction { get; protected set; } = new Dictionary<int, int>();
        public virtual void AdditionalAction(Gameplay gameplay) { }
        public virtual void AdditionalReverseAction(Gameplay gameplay) { }
        public virtual bool HasReserve => false;
        public virtual bool AllowMult => true;

        public bool AllowDo(Gameplay gameplay, int mult)
        {
            if (mult > 1 && !AllowMult) return false;
            return ResourceAction.All(x => x.Value >= 0 || gameplay.Current[x.Key] >= -x.Value * mult)
                && BuildingAction.All(x => x.Value >= 0 || gameplay.Buildings[x.Key] >= -x.Value * mult);
        }

        public bool AllowReverseDo(Gameplay gameplay, int mult)
        {
            if (mult > 1 && !AllowMult) return false;
            return HasReserve &&
                ResourceAction.All(x => -x.Value >= 0 || gameplay.Current[x.Key] >= x.Value * mult)
                && BuildingAction.All(x => -x.Value >= 0 || gameplay.Buildings[x.Key] >= x.Value * mult);
        }

        public void Do(Gameplay gameplay, int mult)
        {
            if (!AllowDo(gameplay, mult)) return;
            foreach (var x in ResourceAction)
                gameplay.Current[x.Key] += x.Value * mult;
            foreach (var x in BuildingAction)
                gameplay.Buildings[x.Key] += x.Value;
            for (var i = 0; i < mult; i++) AdditionalAction(gameplay);
        }

        public void ReverseDo(Gameplay gameplay, int mult)
        {
            if (!AllowReverseDo(gameplay, mult)) return;
            foreach (var x in ResourceAction)
                gameplay.Current[x.Key] -= x.Value * mult;
            foreach (var x in BuildingAction)
                gameplay.Buildings[x.Key] -= x.Value;
            for (var i = 0; i < mult; i++) AdditionalReverseAction(gameplay);
        }
    }

    public class Cut_Tree : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -32 },
            { Resource.My_Wood, +4 },
        };
    }

    public class Cut_Woods : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -1 },
            { Resource.My_Wood, -1 },
            { Resource.My_Fuel, +7 },
        };
    }

    public class E_Sell_Woods : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Gold, +4 },
            { Resource.My_Wood, -2 },
        };
    }

    public class Sell_Woods : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Gold, +16 },
            { Resource.My_Wood, -2 },
            { Resource.Village_Gold, -16 },
            { Resource.Village_Wood, +2 },
        };
    }

    public class Sell_House : Action
    {
        public override Dictionary<int, int> BuildingAction { get; protected set; } = new Dictionary<int, int>()
        {
            { Building.GetBuildingIndexByName("My_House"), -1 },
            { Building.GetBuildingIndexByName("House"), +1 },
        };
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Gold, +544 },
            { Resource.Village_Gold, -544 },
            { Resource.My_HumansLimit, -6 },
            { Resource.Village_HumansLimit, +6 },
        };
    }
}
