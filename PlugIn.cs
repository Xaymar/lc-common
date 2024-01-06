using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !DEBUG
using BepInEx;
using HarmonyLib;

namespace com.xaymar.lethalcompany
{
    [BepInPlugin(GUID: PlugInInfo.GUID, Name: PlugInInfo.Name, Version: PlugInInfo.Version)]
    class PlugIn : BaseUnityPlugin
    {
        private static readonly Harmony harmony = new Harmony(PlugInInfo.GUID);

        public static List<Type> lethalExpansionWhitelist = new List<Type> {
            typeof(LadderGenerator)
        };

        private void Awake()
        {
            Logger.LogInfo($"PlugIn {PlugInInfo.Name} (Guid: {PlugInInfo.GUID}) v{PlugInInfo.Version} is loading...");

            try
            {
                Logger.LogInfo("Applying patch to Lethal Expansion...");
                harmony.PatchAll(typeof(com.xaymar.lethalcompany.patches.LethalExpansionPatcher));
            }
            catch (Exception e)
            {
                Logger.LogFatal($"Failed to apply patch: {e}");
                UnityEngine.Application.Quit(1);
            }

            Logger.LogInfo("Loaded successfully.");
        }
    }
}
#endif