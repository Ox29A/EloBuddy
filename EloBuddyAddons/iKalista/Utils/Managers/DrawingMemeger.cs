using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using iKalista.Utils.Misc;

namespace iKalista.Utils.Managers
{
    internal class DrawingMemeger
    {
        public static void Initialize()
        {
            DamageIndicator.Initialize(DamageMemeger.GetRendDamage);
            DamageIndicator.DrawingColor = Color.Goldenrod;

            Drawing.OnDraw += OnDrawing;
        }

        private static void OnDrawing(EventArgs args)
        {
            DamageIndicator.HealthbarEnabled =
                MenuMemeger.DrawMenu.GetMenuItem<CheckBox>("com.ikalista.drawing.drawDamage").CurrentValue;
            DamageIndicator.PercentEnabled =
                MenuMemeger.DrawMenu.GetMenuItem<CheckBox>("com.ikalista.drawing.drawDamagePercent").CurrentValue;

            var spells = new Spell.SpellBase[] {SpellMemeger.Q, SpellMemeger.W, SpellMemeger.E, SpellMemeger.R};

            foreach (
                var spell in
                spells.Where(
                    x =>
                        x.IsReady() &&
                        MenuMemeger.DrawMenu.GetMenuItem<CheckBox>("com.ikalista.drawing.drawRanges").CurrentValue))
                Drawing.DrawCircle(ObjectManager.Player.Position, spell.Range, Color.Aqua);
        }
    }
}