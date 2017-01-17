using System;
using System.Linq;
using EloBuddy;
using SharpDX;
using Color = System.Drawing.Color;

namespace iTwitch
{
    internal static class Helper
    {
        public static void DrawTextOnScreen(this Vector3 location, string message, Color colour)
        {
            var worldToScreen = Drawing.WorldToScreen(location);
            Drawing.DrawText(worldToScreen[0] - message.Length * 5, worldToScreen[1] - 200, colour, message);
        }

        public static bool HasUndyingBuff(this Obj_AI_Base target1)
        {
            var target = target1 as AIHeroClient;

            if (target == null) return false;

            // Tryndamere R
            if (target.ChampionName == "Tryndamere"
                && target.Buffs.Any(
                    b => b.Caster.NetworkId == target.NetworkId && b.IsValid && b.DisplayName == "Undying Rage"))
                return true;

            // Zilean R
            if (target.Buffs.Any(b => b.IsValid && b.DisplayName == "Chrono Shift"))
                return true;

            // Kayle R
            if (target.Buffs.Any(b => b.IsValid && b.DisplayName == "JudicatorIntervention"))
                return true;

            if (target.Buffs.Any(b => b.IsValid && b.Name == "kindredrnodeathbuff"))
                return true;

            // TODO poppy
            return false;
        }

        public static float GetRemainingBuffTime(this Obj_AI_Base target, string buffName)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => string.Equals(buff.Name, buffName, StringComparison.CurrentCultureIgnoreCase))
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.Time;
        }
    }
}