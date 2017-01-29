using System;
using EloBuddy;
using iKalista.Modules;
using iKalista.Utils.Managers;
using iKalista.Utils.Misc;

namespace iKalista
{
    internal static class Kalista
    {
        public static void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Kalista")
                return;

            MenuMemeger.Initialize();
            ModuleMemeger.Initialize();
            SoulboundSaver.Initialize();
            DrawingMemeger.Initialize();
        }
    }
}