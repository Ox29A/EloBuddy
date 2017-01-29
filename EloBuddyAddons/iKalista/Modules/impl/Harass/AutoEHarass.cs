using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

namespace iKalista.Modules.impl.Harass
{
    internal class AutoEHarass : IModule
    {
        public string GetName()
        {
            return "AutoEHarass";
        }

        public bool ShouldExecute()
        {
            return SpellMemeger.E.IsReady() &&
                   MenuMemeger.HarassMenu.GetMenuItem<CheckBox>("com.ikalista.harass.useE").CurrentValue &&
                   Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass;
        }

        public void Execute()
        {
            foreach (
                var target in
                EntityManager.Heroes.Enemies.Where(
                    x => x.IsValid && x.HasRendBuff() && SpellMemeger.E.IsInRange(x)))
                if (target.GetRendBuff().Count >=
                    MenuMemeger.ComboMenu.GetMenuItem<Slider>("com.ikalista.harass.eStacksH").CurrentValue)
                    SpellMemeger.E.Cast();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}