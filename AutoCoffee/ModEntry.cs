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
    public class ModEntry : Mod
    {

        private DrinkManager drinkManager;
        private ModConfig modCfg;


        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // Check if spacechase0's JsonAssets is in the current mod list
            if (Helper.ModRegistry.IsLoaded("spacechase0.JsonAssets"))
            {
                Monitor.Log("Attempting to hook into spacechase0.JsonAssets.", LogLevel.Debug);
                ApiManager.HookIntoJsonAssets(Helper);

                // Hook into Json Asset's IdsAssigned event
                ApiManager.GetJsonAssetInterface().IdsAssigned += OnIdsAssigned;
            }
        }

        private void OnIdsAssigned(object sender, EventArgs e) { }

        public override void Entry(IModHelper helper)
        {
            //load config
            modCfg = this.Helper.ReadConfig<ModConfig>();
            ModResources.coffeeButton = modCfg.coffeeButton;
            ModResources.stackAmount = modCfg.stackAmount;
            ModResources.autoToggleButton = modCfg.autoToggleButton;
            ModResources.monitor = this.Monitor;
            //set up envents
            helper.Events.Input.ButtonPressed += ModButtonPressed;
            helper.Events.GameLoop.UpdateTicking += ModUpdate;
            helper.Events.GameLoop.UpdateTicked += ModUpdated;
            helper.Events.GameLoop.SaveLoaded += ModSaveLoaded;
            drinkManager = new DrinkManager();
            drinkManager.autoDrink = modCfg.autoDrink;
            string[] coffeeTypes = modCfg.coffeeTypes;
            foreach (string json in coffeeTypes)
            {
                drinkManager.addDrink(new Drink(json));
            }
        }

        private void ModSaveLoaded(object sender, SaveLoadedEventArgs e) { }

        private void ModButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // skip if save not loaded yet
            if (!Context.IsWorldReady)
                return;
            drinkManager.ModButtonPressed(sender, e);
        }

        private void ModUpdated(object sender, EventArgs e)
        {
            // skip if save not loaded yet
            if (!Context.IsWorldReady)
                return;
            drinkManager.Update(sender, e);
        }

        private void ModUpdate(object sender, EventArgs e) { }

    }

}