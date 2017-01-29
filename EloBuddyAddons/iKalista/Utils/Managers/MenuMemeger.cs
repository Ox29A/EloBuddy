using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace iKalista.Utils.Managers
{
    internal static class MenuMemeger
    {
        public static Menu MainMenu, ComboMenu, HarassMenu, KillstealMenu, DrawMenu, MiscMenu;

        public static void Initialize()
        {
            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("iKalista", "com.iKalista");

            ComboMenu = MainMenu.AddSubMenu("Combo Options", "com.iKalista.combo");
            {
                ComboMenu.AddBool("com.ikalista.combo.useQ", "Use Q", true);
                ComboMenu.AddBool("com.ikalista.combo.useE", "Use E", true);
                ComboMenu.AddSlider("com.ikalista.combo.eStacks", "E Stacks To Cast", 10, 1, 50);
                ComboMenu.AddBool("com.ikalista.combo.useR", "Save nigga with R", true);
                ComboMenu.AddSlider("com.ikalista.combo.allyPercent", "Percent to save", 5, 1, 100);
            }

            HarassMenu = MainMenu.AddSubMenu("Harass Options", "com.iKalista.harass");
            {
                HarassMenu.AddBool("com.ikalista.harass.useQ", "Use Q", true);
                HarassMenu.AddBool("com.ikalista.harass.useE", "Use E", true);
                HarassMenu.AddSlider("com.ikalista.harass.eStacksH", "Rend at X stacks", 10, 1, 20);
            }

            KillstealMenu = MainMenu.AddSubMenu("Killsteal Options", "com.ikalista.ks");
            {
                KillstealMenu.AddBool("com.ikalista.ks.enabled", "Use Killsteal", true);
                KillstealMenu.AddBool("com.ikalista.ks.q", "Use Q", true);
                KillstealMenu.AddBool("com.ikalista.ks.e", "Use E Killsteal");
            }

            MiscMenu = MainMenu.AddSubMenu("Misc Options", "com.iKalista.misc");
            {
                MiscMenu.AddBool("com.ikalista.misc.jungle", "Use JungleSteal", true);
            }

            DrawMenu = MainMenu.AddSubMenu("Drawing Options", "com.iKalista.drawing");
            {
                DrawMenu.AddBool("com.ikalista.drawing.drawRanges", "Draw Soell Ranges");
                DrawMenu.AddBool("com.ikalista.drawing.drawStacks", "Draw E Stacks");
                DrawMenu.AddBool("com.ikalista.drawing.drawDamage", "Draw Damage Indicator");
                DrawMenu.AddBool("com.ikalista.drawing.drawDamagePercent", "Draw Percent Damage");
            }
        }

        public static CheckBox AddBool(this Menu menu, string name, string displayName, bool value = false)
        {
            return menu.Add(name, new CheckBox(displayName, value));
        }

        public static Slider AddSlider(this Menu menu, string name, string sliderName, int defaultValue, int minValue,
            int maxValue)
        {
            return menu.Add(name, new Slider(sliderName, defaultValue, minValue, maxValue));
        }

        public static T GetMenuItem<T>(this Menu menu, string item) where T : ValueBase
        {
            return menu[item].Cast<T>();
        }
    }
}