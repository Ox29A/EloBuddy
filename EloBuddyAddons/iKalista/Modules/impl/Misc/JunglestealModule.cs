using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

namespace iKalista.Modules.impl.Misc
{
    internal class JunglestealModule : IModule
    {
        public string GetName()
        {
            return "JunglestealModule";
        }

        public bool ShouldExecute()
        {
            return SpellMemeger.E.IsReady() &&
                   MenuMemeger.MiscMenu.GetMenuItem<CheckBox>("com.ikalista.misc.jungle").CurrentValue;
        }

        public void Execute()
        {
            if (EntityManager.MinionsAndMonsters.Monsters.Any(x => x.GetRendDamage() >= x.Health) ||
                ObjectManager.Get<Obj_AI_Minion>().Any(
                    m =>
                        (m.CharData.BaseSkinName.Contains("Baron")
                         || m.CharData.BaseSkinName.Contains("Dragon") || m.Name.Contains("Crab") ||
                         m.Name.Contains("Herald")) && m.GetRendDamage() >= m.TotalShieldHealth() &&
                        m.IsValidTarget(SpellMemeger.E.Range)))
                SpellMemeger.E.Cast();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }
    }
}