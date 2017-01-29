using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Managers;

/**
 * 
 * 
 * Hellsingah Credits more then anything else rofl.
 * 
 */

namespace iKalista.Utils.Misc
{
    internal class SoulboundSaver
    {
        private static readonly Dictionary<float, float> IncDamage = new Dictionary<float, float>();
        private static readonly Dictionary<float, float> InstDamage = new Dictionary<float, float>();
        public static AIHeroClient SoulBoundAlly;

        public static float IncomingDamage
        {
            get { return IncDamage.Sum(e => e.Value) + InstDamage.Sum(e => e.Value); }
        }

        public static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (SoulBoundAlly == null)
                SoulBoundAlly =
                    EntityManager.Heroes.Allies.Find(
                        h => !h.IsMe && h.Buffs.Any(b => b.Caster.IsMe && b.Name == "kalistacoopstrikeally"));
            else if (SpellMemeger.R.IsReady() &&
                     MenuMemeger.ComboMenu.GetMenuItem<CheckBox>("com.ikalista.combo.useR").CurrentValue)
                if (SoulBoundAlly.HealthPercent <
                    MenuMemeger.ComboMenu.GetMenuItem<Slider>("com.ikalista.combo.allyPercent").CurrentValue &&
                    SoulBoundAlly.CountEnemyChampionsInRange(500) > 0 ||
                    IncomingDamage > SoulBoundAlly.Health)
                    SpellMemeger.R.Cast();

            foreach (var entry in IncDamage.Where(entry => entry.Key < Game.Time).ToArray())
                IncDamage.Remove(entry.Key);

            foreach (var entry in InstDamage.Where(entry => entry.Key < Game.Time).ToArray())
                InstDamage.Remove(entry.Key);
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || !SpellMemeger.R.IsReady())
                return;

            if (SoulBoundAlly == null ||
                !MenuMemeger.ComboMenu.GetMenuItem<CheckBox>("com.ikalista.combo.useR").CurrentValue)
                return;

            if ((!(sender is AIHeroClient) || args.SData.IsAutoAttack()) && args.Target != null &&
                args.Target.NetworkId == SoulBoundAlly.NetworkId)
            {
                IncDamage[
                    SoulBoundAlly.ServerPosition.Distance(sender.ServerPosition) / args.SData.MissileSpeed +
                    Game.Time] = sender.GetAutoAttackDamage(SoulBoundAlly);
            }
            else
            {
                var attacker = sender as AIHeroClient;
                if (attacker == null)
                    return;

                var slot = attacker.GetSpellSlotFromName(args.SData.Name);

                if (slot == SpellSlot.Unknown)
                    return;

                if (slot == attacker.GetSpellSlotFromName("SummonerDot") && args.Target != null &&
                    args.Target.NetworkId == SoulBoundAlly.NetworkId)
                    InstDamage[Game.Time + 2] = attacker.GetSummonerSpellDamage(SoulBoundAlly,
                        DamageLibrary.SummonerSpells.Ignite);
                else if (slot == SpellSlot.Q || slot == SpellSlot.W || slot == SpellSlot.E || slot == SpellSlot.R)
                    if (args.Target != null && args.Target.NetworkId == SoulBoundAlly.NetworkId ||
                        args.End.Distance(SoulBoundAlly.ServerPosition) <
                        Math.Pow(args.SData.LineWidth, 2))
                        InstDamage[Game.Time + 2] = attacker.GetSpellDamage(SoulBoundAlly, slot);
            }
        }
    }
}