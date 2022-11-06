using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRTS
{
    public abstract class Action
    {
        private string typename;
        public string Typename
        {
            get
            {
                if (typename == null) typename = GetType().Name;
                return typename;
            }
        }
        public string Title => Program.Text[this, DBT.Name];
        public string Description => Program.Text[this, DBT.Description];
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
}
