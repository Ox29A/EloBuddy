using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

namespace iKalista.Modules.impl.Misc
{
    internal class KillstealModule : IModule
    {
        public string GetName()
        {
            return "KillstealModule";
        }

        public bool ShouldExecute()
        {
            return (SpellMemeger.Q.IsReady() || SpellMemeger.E.IsReady()) &&
                   MenuMemeger.KillstealMenu.GetMenuItem<CheckBox>("com.ikalista.ks.enabled").CurrentValue; // menu item
        }

        public void Execute()
        {
            if (MenuMemeger.KillstealMenu.GetMenuItem<CheckBox>("com.ikalista.ks.q").CurrentValue)
                foreach (
                    var target in
                    EntityManager.Heroes.Enemies.Where(
                        x =>
                            SpellMemeger.Q.IsInRange(x) && DamageMemeger.GetQDamage(x) > x.TotalShieldHealth() &&
                            x.IsValidTarget(SpellMemeger.Q.Range)))
                    if (target != null)
                    {
                        var prediction = SpellMemeger.Q.GetPrediction(target);
                        if (prediction.HitChance >= HitChance.High)
                            SpellMemeger.Q.Cast(prediction.CastPosition);
                    }

            if (MenuMemeger.KillstealMenu.GetMenuItem<CheckBox>("com.ikalista.ks.e").CurrentValue)
                foreach (
                    var target in
                    EntityManager.Heroes.Enemies.Where(
                        x =>
                            SpellMemeger.E.IsInRange(x) && x.IsRendKillable() &&
                            x.IsValidTarget(SpellMemeger.E.Range)))
                    if (target != null)
                        SpellMemeger.E.Cast();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}