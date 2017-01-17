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