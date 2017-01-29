using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

namespace iKalista.Modules.impl.Harass
{
    internal class AutoQHarass : IModule
    {
        public string GetName()
        {
            return "AutoQHarass";
        }

        public bool ShouldExecute()
        {
            return SpellMemeger.Q.IsReady() &&
                   MenuMemeger.HarassMenu.GetMenuItem<CheckBox>("com.ikalista.harass.useQ").CurrentValue &&
                   Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass;
        }

        public void Execute()
        {
            var target = TargetSelector.GetTarget(SpellMemeger.Q.Range, DamageType.Physical);

            if (target == null || !target.IsValidTarget(SpellMemeger.Q.Range) || !SpellMemeger.Q.IsInRange(target))
                return;

            var prediction = SpellMemeger.Q.GetPrediction(target);

            if (prediction.HitChance >= HitChance.Medium)
                SpellMemeger.Q.Cast(prediction.CastPosition);
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}