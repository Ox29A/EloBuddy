using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

namespace iKalista.Modules.impl.Combo
{
    internal class AutoQModule : IModule
    {
        public string GetName()
        {
            return "AutoQModule";
        }

        public bool ShouldExecute()
        {
            return SpellMemeger.Q.IsReady() &&
                   MenuMemeger.ComboMenu.GetMenuItem<CheckBox>("com.ikalista.combo.useQ").CurrentValue &&
                   Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
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