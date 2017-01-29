using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace iKalista.Utils.Managers
{
    internal static class DamageMemeger
    {
        private const string BuffName = "kalistaexpungemarker";
        public static Spell.Active RendSpell = SpellMemeger.E;

        private static float BaseRendDamage => new[] {20f, 30f, 40f, 50f, 60f}[RendSpell.Level - 1];

        private static float AdditionalRendDamage => new[] {0.6f, 0.6f, 0.6f, 0.6f, 0.6f}[RendSpell.Level - 1];

        private static float SpearDamagePerStack => new[] {5, 9, 14, 20, 27}[RendSpell.Level - 1];

        private static float AdditionalSpearDamage => new[] {0.15f, 0.18f, 0.21f, 0.24f, 0.27f}[RendSpell.Level - 1];

        public static bool IsRendKillable(this Obj_AI_Base @base)
        {
            if (@base == null || !@base.IsValidTarget() || !@base.HasRendBuff())
                return false;

            var target = @base as AIHeroClient;
            double baseDamage = GetRendDamage(target);

            if (target == null) return baseDamage >= @base.TotalShieldHealth();

            if (target.HasUndyingBuff() || target.HasBuffOfType(BuffType.SpellShield) ||
                target.HasBuffOfType(BuffType.SpellImmunity))
                return false;

            return baseDamage >= @base.TotalShieldHealth();
        }

        public static float GetQDamage(Obj_AI_Base target)
        {
            return new[] {10f, 70f, 130f, 190f, 250f}[SpellMemeger.Q.Level - 1] + ObjectManager.Player.BaseAttackDamage +
                   ObjectManager.Player.FlatPhysicalDamageMod;
        }

        public static float GetRawRendDamage(Obj_AI_Base target)
        {
            var rendBuff = target?.GetRendBuff();

            if (rendBuff == null) return 0;

            var stacks = rendBuff.Count;
            return BaseRendDamage + stacks * SpearDamagePerStack +
                   ObjectManager.Player.TotalAttackDamage * (AdditionalRendDamage + stacks * AdditionalSpearDamage);
        }

        public static float GetRendDamage(this Obj_AI_Base target)
        {
            return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Physical, GetRawRendDamage(target));
        }

        public static BuffInstance GetRendBuff(this Obj_AI_Base target)
        {
            return target.Buffs.FirstOrDefault(x => x.Name == BuffName);
        }

        public static bool HasRendBuff(this Obj_AI_Base target)
        {
            return target.GetRendBuff() != null;
        }
    }
}