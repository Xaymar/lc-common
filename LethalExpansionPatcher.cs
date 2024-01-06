using System;
using System.Linq;
using UnityEngine;

#if !DEBUG
using HarmonyLib;

using LethalExpansion;

// Eventually will need to figure out Transpiler injection. This is as good as it gets for now.

namespace com.xaymar.lethalcompany.patches
{
    [HarmonyPatch(typeof(LethalExpansion.LethalExpansion))]
    class LethalExpansionPatcher
    {
        [HarmonyPatch("CheckAndRemoveIllegalComponents")]
        [HarmonyPrefix]
        static bool CheckAndRemoveIllegalComponents_Prefix(LethalExpansion.LethalExpansion __instance, Transform root)
        {
            try
            {
                var comps = root.GetComponents<Component>();
                foreach (var comp in comps)
                {
                    if (com.xaymar.lethalcompany.PlugIn.lethalExpansionWhitelist.Any(type => (comp.GetType() == type)))
                    {
                        return false; // Skip Lethal Expansion's method.
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}

#endif