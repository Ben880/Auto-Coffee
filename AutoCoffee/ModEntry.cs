using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;

namespace AutoCoffee
{
    public class ModEntry : Mod
    {

        //config options
        private string coffeeButton;
        private string autoToggleButton;
        private int stackAmount;
        private bool autoDrink;
        private bool logToConsole;
        private int buffBaseID = 880;
        private int[][] coffeeDefinition;
        //other options
        private ModConfig modCfg;
        private Buff autoDrinkBuff;


        public override void Entry(IModHelper helper)
        {
            //load config
            modCfg = this.Helper.ReadConfig<ModConfig>();
            coffeeButton = modCfg.coffeeButton;
            stackAmount = modCfg.stackAmount;
            autoDrink = modCfg.autoDrink;
            autoToggleButton = modCfg.autoToggleButton;
            logToConsole = modCfg.logToCosole;
            coffeeDefinition = modCfg.coffeeDeffinition;
            //set up envents
            helper.Events.Input.ButtonPressed += ModButtonPressed;
            helper.Events.GameLoop.UpdateTicking += ModUpdate;
            helper.Events.GameLoop.UpdateTicked += ModUpdated;
            helper.Events.GameLoop.SaveLoaded += ModSaveLoaded;
            //auto drink buff
            autoDrinkBuff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "AutoCoffee", "AutoCoffee");
            autoDrinkBuff.description = $"Auto drink enabled, press {autoToggleButton.ToString()} to toggle";
            autoDrinkBuff.millisecondsDuration = 1000;
            autoDrinkBuff.which = buffBaseID + stackAmount;
            autoDrinkBuff.sheetIndex = 9;
        }

        private void ModSaveLoaded(object sender, SaveLoadedEventArgs e)
        {

        }

        private void ModButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            if (e.Button.ToString() == autoToggleButton)
                ToggleAutoDrink();
            if (e.Button.ToString() == coffeeButton)
                DrinkCoffee();
        }


        private void ModUpdated(object sender, EventArgs e)
        {
            // skip if save not loaded yet
            if (!Context.IsWorldReady)
                return;
            if (autoDrink)
            {
                DrinkCoffee();
                Game1.buffsDisplay.removeOtherBuff(buffBaseID + stackAmount);
                Game1.buffsDisplay.addOtherBuff(autoDrinkBuff);
                foreach (Buff buff in Game1.buffsDisplay.otherBuffs)
                {
                    if (buff.which == buffBaseID + stackAmount)
                        buff.millisecondsDuration = 300;
                }

            }
        }


        private void ModUpdate(object sender, EventArgs e)
        {


        }

        private void Log(string s)
        {
            if (logToConsole)
                this.Monitor.Log(s);
        }


        private void ToggleAutoDrink()
        {
            autoDrink = !autoDrink;
            if (autoDrink)
                Game1.buffsDisplay.addOtherBuff(autoDrinkBuff);
            else
                Game1.buffsDisplay.removeOtherBuff(buffBaseID + stackAmount);
        }

        private void DrinkCoffee()
        {
            int drinkIndex = -1;
            for (int i = 0; i < coffeeDefinition.Length; i++)
            {
                if (Game1.player.hasItemInInventory(coffeeDefinition[i][0], 1))
                {
                    drinkIndex = i;
                }
            }
            if (drinkIndex == -1)
                return;

            for (int i = 0; i < stackAmount; i++)
            {
                if (!Game1.buffsDisplay.hasBuff(buffBaseID + i))
                {
                    Game1.player.removeFirstOfThisItemFromInventory(coffeeDefinition[drinkIndex][0]);
                    Buff buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, coffeeDefinition[drinkIndex][2], 0, 0, 0, "Coffee", "Coffee");
                    buff.description = "+" + (coffeeDefinition[drinkIndex][2]) + " Speed";
                    buff.millisecondsDuration = (coffeeDefinition[drinkIndex][1] * 1000);
                    buff.which = buffBaseID + i;
                    buff.sheetIndex = 9;
                    Game1.buffsDisplay.addOtherBuff(buff);
                    return;
                }
            }
        }
    }
}

