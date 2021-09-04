using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AutoCoffee
{
    class Drink
    {

        public int id { get; set; }
        public string name { get; set; }

        private JsonWrapper jsonWrapper;

        private Dictionary<string, int> stats = new Dictionary<string, int>();

        public Drink(string json)
        {
            jsonWrapper = new JsonWrapper(json);
            string[] buffKeys = new string[] { "farming", "fishing", "mining", "diging", "luck", "foraging", "crafting", "maxStamina", "magneticRadius", "speed", "defense", "attack", "minutes" };
            foreach (string key in buffKeys)
            {
                stats.Add(key, jsonWrapper.tryGetInt(key));
            }
            name = jsonWrapper.tryGetString("name");
            id = jsonWrapper.tryGetInt("id");
        }

        public void setDescription(string description)
        {
            jsonWrapper.trySet("description", description);
        }

        public void setSource(string source)
        {
            jsonWrapper.trySet("source", source);
        }

        public void setDisplaySource(string displaySource)
        {
            jsonWrapper.trySet("displaySource", displaySource);
        }

        public void setMillisecondsDuration(int mills)
        {
            jsonWrapper.trySet("millisecondsDuration", mills);
        }

        public void setSheetIndex(int i)
        {
            jsonWrapper.trySet("setSheetIndex", i);
        }

        public Buff getBuff(int which)
        {
            string source = jsonWrapper.tryGetString("source", jsonWrapper.tryGetString("name"));
            string displaySource = jsonWrapper.tryGetString("displaySource", jsonWrapper.tryGetString("name"));
            Buff buff = new Buff(stats["farming"], stats["fishing"], stats["mining"], stats["diging"], stats["luck"], stats["foraging"], stats["crafting"], stats["maxStamina"],
                            stats["magneticRadius"], stats["speed"], stats["defense"], stats["attack"], stats["minutes"], source, displaySource);
            buff.description = jsonWrapper.tryGetString("description", jsonWrapper.tryGetString("name"));
            buff.millisecondsDuration = jsonWrapper.tryGetInt("millisecondsDuration", jsonWrapper.tryGetInt("minutes") * 60000);
            buff.sheetIndex = jsonWrapper.tryGetInt("sheetIndex", 9);
            buff.which = which;
            return buff;
        }

    }


    class DrinkManager
    {

        private Buff autoDrinkBuff;
        public bool autoDrink { get; set; } = false;

        private List<Drink> drinks = new List<Drink>();

        public DrinkManager()
        {
            Drink autoDrinkDrink = new Drink(JsonWrapper.emptyJson);
            autoDrinkDrink.setDescription($"Auto drink enabled, press {ModResources.autoToggleButton.ToString()} to toggle");
            autoDrinkDrink.setSource("AutoCoffee");
            autoDrinkDrink.setDisplaySource("AutoCoffee");
            autoDrinkDrink.setMillisecondsDuration(1000);
            autoDrinkDrink.setSheetIndex(9);
            autoDrinkBuff = autoDrinkDrink.getBuff(ModResources.buffBaseID + ModResources.stackAmount);
        }

        public void addDrink(Drink drink)
        {
            drinks.Add(drink);
        }

        public void Update(object sender, EventArgs e)
        {
            if (autoDrink)
            {
                DrinkCoffee();
                Game1.buffsDisplay.removeOtherBuff(autoDrinkBuff.which);
                Game1.buffsDisplay.addOtherBuff(autoDrinkBuff);
                foreach (Buff buff in Game1.buffsDisplay.otherBuffs)
                {
                    if (buff.which == autoDrinkBuff.which)
                        buff.millisecondsDuration = 300;
                }
            }
        }

        public void ModButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.ToString() == ModResources.autoToggleButton)
                ToggleAutoDrink();
            if (e.Button.ToString() == ModResources.coffeeButton)
                DrinkCoffee();
        }

        private void ToggleAutoDrink()
        {
            autoDrink = !autoDrink;
            if (autoDrink)
                Game1.buffsDisplay.addOtherBuff(autoDrinkBuff);
            else
                Game1.buffsDisplay.removeOtherBuff(ModResources.buffBaseID + ModResources.stackAmount);
        }

        private void DrinkCoffee()
        {
            // find a drink inside of inventory
            Drink drink = getFirstDrink();
            if (drink is null)
                return;
            // for allowed buff stacking amount add coffee buff
            for (int i = 0; i < ModResources.stackAmount; i++)
            {
                if (!Game1.buffsDisplay.hasBuff(ModResources.buffBaseID + i))
                {
                    ModResources.monitor.Log($"Trying to add buff", LogLevel.Debug);
                    Game1.player.removeFirstOfThisItemFromInventory(drink.id);
                    Game1.buffsDisplay.addOtherBuff(drink.getBuff(ModResources.buffBaseID + i));
                }
            }
        }

        private Drink getFirstDrink()
        {
            foreach (Drink drink in drinks)
            {
                if (Game1.player.hasItemInInventory(drink.id, 1))
                {
                    return drink;
                }
            }
            return null;
        }


    }
}
