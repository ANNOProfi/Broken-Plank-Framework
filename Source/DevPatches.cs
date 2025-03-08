using HarmonyLib;
using RimWorld;
using Verse;

namespace BrokenPlankFramework
{
    [HarmonyPatch(typeof(DesignationCategoryDef))]
    public static class DesignationCategoryDef_Visible_Patch
    {
        [HarmonyPatch(nameof(DesignationCategoryDef.Visible), MethodType.Getter)]
        [HarmonyPostfix]
        private static void VisibilityGetter(DesignationCategoryDef __instance, ref bool __result)
        {
            if(__instance.defName == "BPF_Dev")
            {
                if(DebugSettings.godMode)
                {
                    __result = true;
                }
                else
                {
                    __result = false;
                }
            }
        }
    }
}