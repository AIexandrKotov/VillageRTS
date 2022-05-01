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
        public virtual void AdditionalAction(Gameplay gameplay) { }
        public virtual void AdditionalReverseAction(Gameplay gameplay) { }
        public virtual bool HasReserve => false;

        public bool AllowDo(Gameplay gameplay, int mult)
        {
            return ResourceAction.All(x => x.Value >= 0 || gameplay.Current[x.Key] >= -x.Value * mult);
        }

        public bool AllowReverseDo(Gameplay gameplay, int mult)
        {
            return HasReserve && ResourceAction.All(x => -x.Value >= 0 || gameplay.Current[x.Key] >= x.Value * mult);
        }

        public void Do(Gameplay gameplay, int mult)
        {
            if (!AllowDo(gameplay, mult)) return;
            foreach (var x in ResourceAction)
                gameplay.Current[x.Key] += x.Value * mult;
            for (var i = 0; i < mult; i++) AdditionalAction(gameplay);
        }

        public void ReverseDo(Gameplay gameplay, int mult)
        {
            if (!AllowReverseDo(gameplay, mult)) return;
            foreach (var x in ResourceAction)
                gameplay.Current[x.Key] -= x.Value * mult;
            for (var i = 0; i < mult; i++) AdditionalReverseAction(gameplay);
        }
    }

    public class Cut_Tree : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Power, -10 },
            { Resource.My_Wood, +16 },
        };
    }

    public class Sell_Woods : Action
    {
        public override Dictionary<Resource, int> ResourceAction { get; protected set; } = new Dictionary<Resource, int>() {
            { Resource.My_Gold, +4 },
            { Resource.My_Wood, -1 },
            { Resource.Humans_Gold, -4 },
            { Resource.Humans_Wood, +1 },
        };
    }
}
