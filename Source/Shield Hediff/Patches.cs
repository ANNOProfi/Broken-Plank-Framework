using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace BrokenPlankFramework
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.PreApplyDamage))]
    public static class Pawn_PrePreApplyDamage
    {
        static void Postfix(Pawn __instance, ref DamageInfo dinfo, ref bool absorbed)
        {
            //Log.Message("BPF Message: Beginning damage patch for thing "+__instance.Name);
            if (absorbed)
            {
                //Log.Message("BPF Message: Abort due to absorbed damage");
                return;
            }

            //Log.Message("BPF Message: Trying to get cache");

            if (BrokenPlankCache.responderCache.NullOrEmpty() || !BrokenPlankCache.responderCache.TryGetValue(__instance.thingIDNumber, out List<IDamageResponse> mods))
            {
                //Log.Message("BPF Message: Abort due to empty cache");
                return;
            }

            foreach(IDamageResponse responder in mods)
            {
                //Log.Message("BPF Message: Looping");

                responder.PreApplyDamage(ref dinfo, ref absorbed);

                if (absorbed)
                {
                    return;
                }
            }

            //Log.Message("BPF Message: Exit");
            //return;
        }
    }

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.DynamicDrawPhaseAt))]
    public static class Pawn_PostDrawAt
    {
        public static void Postfix(Pawn __instance, ref Vector3 drawLoc)
        {
            //Log.Message("BPF Message: Beginning rendering patch for pawn "+__instance.Name);
            //Log.Message("BPF Message: bodyType = "+__instance.story.bodyType);
            BodyTypeDef bodyType = null;

            if(__instance.story != null && __instance.story.bodyType != null)
            {
                bodyType = __instance.story.bodyType;
            }

            if(bodyType == null || BrokenPlankCache.renderCache.NullOrEmpty())
            {
                //Log.Message("BPF Message: Abort due to null bodyType");
                return;
            }

            if(BrokenPlankCache.renderCache.TryGetValue(__instance.thingIDNumber, out List<IRenderable> mods2))
            {
                //Log.Message("BPF Message: Entered TryGetValue");

                foreach(IRenderable renderable in mods2)
                {
                    renderable.DrawAt(drawLoc, __instance.story.bodyType);
                }
            }

            //Log.Message("BPF Message: Exit");
            return;
        }
    }

    /*[HarmonyPatch(typeof(Thing), nameof(Thing.PreApplyDamage))]
    public static class Thing_PrePreApplyDamage
    {
        static void Postfix(Thing __instance, ref DamageInfo dinfo, ref bool absorbed)
        {
            Log.Message("BPF Message: Beginning damage patch for thing "+__instance.ThingID);
            if (absorbed)
            {
                return;
            }

            if (!BrokenPlankCache.responderCache.TryGetValue(__instance.thingIDNumber, out List<IDamageResponse> mods))
            {
                return;
            }

            for (int i = mods.Count - 1; i >= 0; i--)
            {
                IDamageResponse responder = mods[i];

                responder.PreApplyDamage(ref dinfo, ref absorbed);

                if (absorbed)
                {
                    return;
                }
            }
        }
    }*/
}