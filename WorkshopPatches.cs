using Cysharp.Threading.Tasks;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace BepInEx.StationeerModLoader.WorkshopPatches
{
    internal class ModLoaderWorkShopPatches : WorkshopMenu
    {
        //The patch for loadmodconfig this is where it update the modconfig.xml as well
        [HarmonyPatch(typeof(WorkshopMenu), nameof(WorkshopMenu.LoadModConfig))]
        public static class WorkshopMenu_LoadModConfig_Patch
        {
            [HarmonyPostfix]
            static async void Postfix(WorkshopMenu __instance)
            {
                if (File.Exists(ConfigFile.ModConfigPath))
                {
                    ConfigFile.AttemptToLoadModConfig();
                    ModLoaderWorkShopPatches path = new ModLoaderWorkShopPatches();
                    await path.UpdateList();
                }
            }
        }

        //Created my own Init() so I can call the Unitask refreshlist
        public async UniTask UpdateList()
        {
            await GetModFolders();
            DataCleanup();
            RefreshList();
        }
        //Just here to update my config file for my modloader
        [HarmonyPatch(typeof(WorkshopMenu), nameof(WorkshopMenu.SaveModConfig))]
        public static class WorkshopMenu_SaveModConfig_Patch
        {
            [HarmonyPostfix]
            static void PostFix(WorkshopMenu __instance)
            {
                if (File.Exists(ConfigFile.ModConfigPath))
                {
                    ConfigFile.AttemptToLoadModConfig();
                }
            }
        }

        //How I update the list on game start, this is just a modfied version of RefreshList
        [HarmonyPatch(typeof(WorkshopMenu), nameof(WorkshopMenu.RefreshList))]
        public static class WorkshopMenu_RefreshList_Patch
        {
            [HarmonyPrefix]
            static bool Prefix(WorkshopMenu __instance)
            {
                foreach (WorkshopModListItem listItem in __instance._listItems)
                {
                    Object.Destroy(listItem.gameObject);
                }
                __instance._listItems.Clear();
                foreach (global::ModData mod in ModsConfig.Mods)
                {
                    var workshop = GameObject.FindObjectsOfType(typeof(WorkshopMenu));
                    foreach (WorkshopMenu menu in workshop)
                    {
                        if (menu.gameObject.activeSelf)
                        {
                            WorkshopModListItem workshopModListItem = null;
                            if (workshopModListItem == null)
                            {
                                workshopModListItem = Object.Instantiate(menu.WorkshopItemPrefab, menu.ModListContainer);
                                workshopModListItem.transform.SetParent(menu.ModListContainer);
                                workshopModListItem.SetData(mod);
                                if (!__instance._listItems.Contains(workshopModListItem))
                                {
                                    __instance._listItems.Add(workshopModListItem);
                                }
                            }
                        }
                    }
                }
                if (File.Exists(ConfigFile.ModConfigPath))
                {
                    ConfigFile.AttemptToLoadModConfig();
                }
                SaveModConfig();
                return false;
            }
        }

        //Just here to update my config file for my modloader
        [HarmonyPatch(typeof(WorkshopMenu), nameof(WorkshopMenu.UnsubscribeButton))]
        public static class WorkshopMenu_UnsubscribeButton_Patch
        {
            [HarmonyPostfix]
            static void PostFix(WorkshopMenu __instance)
            {
                if (File.Exists(ConfigFile.ModConfigPath))
                {
                    ConfigFile.AttemptToLoadModConfig();
                }
            }
        }
    }
}
