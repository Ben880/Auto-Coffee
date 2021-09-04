using StardewModdingAPI;
using AutoCoffee.API;

namespace AutoCoffee
{
    class ApiManager
    {
        private static IMonitor monitor = ModResources.monitor;
        private static IJsonAssets jsonAssetApi;


        public static void HookIntoJsonAssets(IModHelper helper)
        {
            // Attempt to hook into the IMobileApi interface
            jsonAssetApi = helper.ModRegistry.GetApi<IJsonAssets>("spacechase0.JsonAssets");

            if (jsonAssetApi is null)
            {
                monitor.Log("Failed to hook into spacechase0.JsonAssets.", LogLevel.Error);
                return;
            }

            monitor.Log("Successfully hooked into spacechase0.JsonAssets.", LogLevel.Debug);
        }

        public static IJsonAssets GetJsonAssetInterface()
        {
            return jsonAssetApi;
        }
    }
}
