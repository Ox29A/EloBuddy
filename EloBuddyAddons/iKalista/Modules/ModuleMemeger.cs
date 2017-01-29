using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using iKalista.Modules.impl.Combo;
using iKalista.Modules.impl.Harass;
using iKalista.Modules.impl.Misc;

namespace iKalista.Modules
{
    internal static class ModuleMemeger
    {
        private static readonly List<IModule> Modules = new List<IModule>
        {
            new AutoEModule(),
            new AutoEHarass(),
            new AutoQModule(),
            new AutoQHarass(),
            new JunglestealModule(),
            new KillstealModule()
        };

        public static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;

            foreach (var module in Modules)
                Console.WriteLine("Module: " + module.GetName() + " Type: " + module.GetModuleType() +
                                  " succesfully loaded.");
        }

        private static void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Modules.Count <= 0)
                return;

            foreach (var module in Modules.Where(x => x.ShouldExecute() && x.GetModuleType() == ModuleType.PreAttack))
                module.Execute();
        }

        private static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Modules.Count <= 0)
                return;

            foreach (var module in Modules.Where(x => x.ShouldExecute() && x.GetModuleType() == ModuleType.PostAttack))
                module.Execute();
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Modules.Count <= 0)
                return;

            foreach (var module in Modules.Where(x => x.ShouldExecute() && x.GetModuleType() == ModuleType.OnUpdate))
                module.Execute();
        }
    }
}