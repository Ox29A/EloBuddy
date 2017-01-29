using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

namespace iKalista.Modules.impl.Combo
{
    internal class AutoEModule : IModule
    {
        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public string GetName()
        {
            return "AutoEModule";
        }

        public bool ShouldExecute()
        {
            return SpellMemeger.E.IsReady() &&
                   MenuMemeger.ComboMenu.GetMenuItem<CheckBox>("com.ikalista.combo.useE").CurrentValue &&
                   Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
        }

        public void Execute()
        {
            foreach (
                var target in
                EntityManager.Heroes.Enemies.Where(
                    x => x.IsValid && x.HasRendBuff() && SpellMemeger.E.IsInRange(x)))
                if (target.GetRendBuff().Count >=
                    MenuMemeger.ComboMenu.GetMenuItem<Slider>("com.ikalista.combo.eStacks").CurrentValue)
                {
                    Chat.Print("Damages mate, kill this kunt");
                    SpellMemeger.E.Cast();
                }
        }
    }
}