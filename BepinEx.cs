using HarmonyLib;

namespace BepInEx.StationeerModLoader.WorkshopPatches
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class BepinEx : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "modder.ihatetn931.StationeersMods.ModLoader.WorkShopPatches";
        public const string PLUGIN_NAME = "Stationeers.ModLoader.WorkShopPatches";
        public const string PLUGIN_VERSION = "1.0.1";

        void Awake()
        {
            Logger.LogInfo($"Plugin {PLUGIN_NAME} is loading!");
            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {PLUGIN_NAME} Verison {PLUGIN_VERSION} is loaded!");
        }

    }
}

