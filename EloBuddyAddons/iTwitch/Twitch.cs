using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

namespace iTwitch
{
    public class Twitch
    {
        #region Fields

        private Menu _mainMenu, _comboMenu, _harassMenu, _drawMenu, _miscMenu;

        #endregion

        #region Static Fields

        public static Spell.Skillshot W;
        public static Spell.Active Q, E, R;

        #endregion

        #region Public Methods and Operators

        public void LoadMenu()
        {
            _mainMenu = MainMenu.AddMenu("iTwitch 2.0", "com.itwitch");

            _comboMenu = _mainMenu.AddSubMenu("Combo Options", "com.itwitch.combo");
            {
                _comboMenu.Add("com.itwitch.combo.useW", new CheckBox("Use W"));
                _comboMenu.Add("com.itwitch.combo.useE", new CheckBox("Use E"));
            }

            _harassMenu = _mainMenu.AddSubMenu("Harass Options", "com.itwitch.harass");
            {
                _harassMenu.Add("com.itwitch.harass.useW", new CheckBox("Use W"));
                _harassMenu.Add("com.itwitch.harass.useEKillable", new CheckBox("Use E"));
            }

            _miscMenu = _mainMenu.AddSubMenu("Misc Options", "com.itwitch.misc");
            {
                _miscMenu.Add("com.itwitch.misc.recall",
                    new KeyBind("Stealth Recall Key", false, KeyBind.BindTypes.HoldActive, 'T'));
                _miscMenu.Add("com.itwitch.misc.autoYo", new CheckBox("Youmuus with R"));
                _miscMenu.Add("com.itwitch.misc.noWTurret", new CheckBox("Don't W Under Tower"));
                _miscMenu.Add("com.itwitch.misc.noWAA", new Slider("No W if x aa can kill", 2, 0, 10));
                _miscMenu.Add("com.itwitch.misc.ebeforedeath", new CheckBox("E Before Death"));
                _miscMenu.Add("com.itwitch.misc.saveManaE", new CheckBox("Save Mana for E"));
            }

            _drawMenu = _mainMenu.AddSubMenu("Drawing Options", "com.itwitch.drawing");
            {
                _drawMenu.Add("com.itwitch.drawing.drawERange", new CheckBox("Draw E Range"));
                _drawMenu.Add("com.itwitch.drawing.drawRRange", new CheckBox("Draw R Range"));
                _drawMenu.Add("com.itwitch.drawing.drawQTime", new CheckBox("Draw Q Time"));
                _drawMenu.Add("com.itwitch.drawing.drawEStacks", new CheckBox("Draw E Stacks"));
                _drawMenu.Add("com.itwitch.drawing.drawEStackT", new CheckBox("Draw E Stack Time"));
                _drawMenu.Add("com.itwitch.drawing.drawRTime", new CheckBox("Draw R Time"));
            }
        }

        public void LoadSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, 1400, 280)
            {
                MinimumHitChance = HitChance.Medium
            };
            E = new Spell.Active(SpellSlot.E, 1200);
            R = new Spell.Active(SpellSlot.R);
        }

        public void OnCombo()
        {
        }

        public void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Twitch") return;

            LoadSpells();
            LoadMenu();

            Spellbook.OnCastSpell += (sender, eventArgs) =>
            {
                if (eventArgs.Slot == SpellSlot.Recall && Q.IsReady()
                    && _miscMenu["com.itwitch.misc.recall"].Cast<KeyBind>().CurrentValue)
                {
                    Q.Cast();
                    Core.DelayAction(() => ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall), Q.CastDelay + 300);
                    eventArgs.Process = false;
                    return;
                }

                if (eventArgs.Slot == SpellSlot.R && _miscMenu["com.itwitch.misc.autoYo"].Cast<CheckBox>().CurrentValue)
                    if (!EntityManager.Enemies.Any(x => ObjectManager.Player.Distance(x) <= R.Range))
                        return;

                if (_miscMenu["com.itwitch.misc.saveManaE"].Cast<CheckBox>().CurrentValue &&
                    eventArgs.Slot == SpellSlot.W)
                    if (ObjectManager.Player.Mana <= E.ManaCost + 10)
                        eventArgs.Process = false;
            };

            PassiveManager.Init();
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += OnPostAttack;
        }

        private void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (target == null || !target.IsEnemy || target.GetType() != typeof(AIHeroClient))
                return;

            if (_comboMenu["com.itwitch.combo.useW"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                if (_miscMenu["com.itwitch.misc.saveManaE"].Cast<CheckBox>().CurrentValue &&
                    ObjectManager.Player.Mana <= E.ManaCost + W.ManaCost)
                    return;

                if (_miscMenu["com.itwitch.misc.noWTurret"].Cast<CheckBox>().CurrentValue &&
                    ObjectManager.Player.IsUnderTurret())
                    return;

                var wTarget = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                if (wTarget != null
                    && wTarget.Health
                    < ObjectManager.Player.GetAutoAttackDamage(wTarget, true)
                    * _miscMenu["com.itwitch.misc.noWAA"].Cast<Slider>().CurrentValue)
                    return;

                if (wTarget.IsValidTarget(W.Range)
                    && !ObjectManager.Player.HasBuff("TwitchHideInShadows"))
                {
                    var prediction = W.GetPrediction(wTarget);
                    if (prediction.HitChance >= HitChance.High)
                        W.Cast(prediction.CastPosition);
                }
            }
        }

        public void OnHarass()
        {
            if (_harassMenu["com.itwitch.harass.useW"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var wTarget = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                if (wTarget.IsValidTarget(W.Range))
                {
                    var prediction = W.GetPrediction(wTarget);
                    if (prediction.HitChance >= HitChance.High)
                        W.Cast(prediction.CastPosition);
                }
            }
        }

        #endregion

        #region Methods

        private void OnDraw(EventArgs args)
        {
            if (_drawMenu["com.itwitch.drawing.drawRRange"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(ObjectManager.Player.Position, R.Range, Color.BlueViolet);
                Drawing.DrawCircle(ObjectManager.Player.Position, 850, Color.BlueViolet);
            }

            if (_drawMenu["com.itwitch.drawing.drawERange"].Cast<CheckBox>().CurrentValue)
                Drawing.DrawCircle(ObjectManager.Player.Position, E.Range, Color.BlueViolet);

            if (_drawMenu["com.itwitch.drawing.drawQTime"].Cast<CheckBox>().CurrentValue
                && ObjectManager.Player.HasBuff("TwitchHideInShadows"))
            {
                var position = new Vector3(
                    ObjectManager.Player.Position.X,
                    ObjectManager.Player.Position.Y - 30,
                    ObjectManager.Player.Position.Z);
                position.DrawTextOnScreen(
                    "Stealth:  " + $"{ObjectManager.Player.GetRemainingBuffTime("TwitchHideInShadows"):0.0}",
                    Color.AntiqueWhite);
            }

            if (_drawMenu["com.itwitch.drawing.drawRTime"].Cast<CheckBox>().CurrentValue
                && ObjectManager.Player.HasBuff("TwitchFullAutomatic"))
                ObjectManager.Player.Position.DrawTextOnScreen(
                    "Ultimate:  " + $"{ObjectManager.Player.GetRemainingBuffTime("TwitchFullAutomatic"):0.0}",
                    Color.AntiqueWhite);

            if (_drawMenu["com.itwitch.drawing.drawEStacks"].Cast<CheckBox>().CurrentValue)
                foreach (var source in
                    EntityManager.Heroes.Enemies.Where(x => x.HasBuff("TwitchDeadlyVenom") && !x.IsDead && x.IsVisible))
                {
                    var position = new Vector3(source.Position.X, source.Position.Y + 10, source.Position.Z);
                    position.DrawTextOnScreen($"{"Stacks: " + source.PassiveCount()}", Color.AntiqueWhite);
                }

            if (_drawMenu["com.itwitch.drawing.drawEStackT"].Cast<CheckBox>().CurrentValue)
                foreach (var source in
                    EntityManager.Heroes.Enemies.Where(x => x.HasBuff("TwitchDeadlyVenom") && !x.IsDead && x.IsVisible))
                {
                    var position = new Vector3(source.Position.X, source.Position.Y - 30, source.Position.Z);
                    position.DrawTextOnScreen(
                        "Stack Timer:  " + $"{source.GetRemainingBuffTime("TwitchDeadlyVenom"):0.0}",
                        Color.AntiqueWhite);
                }
        }

        private void OnUpdate(EventArgs args)
        {
            if (_miscMenu["com.itwitch.misc.recall"].Cast<KeyBind>().CurrentValue)
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall);

            if (_comboMenu["com.itwitch.combo.useE"].Cast<CheckBox>().CurrentValue && E.IsReady())
                if (PassiveManager.PassiveEnemies.Any(x => x.Killable && x.StackCount > 0 && x.Target is AIHeroClient))
                    E.Cast();

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    OnCombo();
                    break;

                case Orbwalker.ActiveModes.Harass:
                    OnHarass();
                    break;
            }
        }

        #endregion
    }
}