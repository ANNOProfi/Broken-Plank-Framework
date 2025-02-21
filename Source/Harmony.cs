using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace BrokenPlankFramework
{
    public class BrokenPlankFrameworkPatches : Mod
    {
        public Harmony harmonyInstance;

        public BrokenPlankFrameworkPatches(ModContentPack content) : base(content)
        {
            harmonyInstance = new Harmony(id: "rimworld.annoprofi.brokenplankframework.main");
            harmonyInstance.PatchAll();
        }
    }

    public static class BrokenPlankCache
    {
        public static Dictionary<int, List<IRenderable>> renderCache;

        public static Dictionary<int, List<IDamageResponse>> responderCache;

        public static void AddCache<T>(T elem, ref Dictionary<int, List<T>> cacheList, int id)
        {
            if (cacheList == null)
            {
                Reset();
            }

            if (cacheList.TryGetValue(id, out List<T> mods))
            {
                if (!mods.Contains(elem))
                {
                    mods.Add(elem);
                }
            }
            else
            {
                cacheList[id] = new List<T>() { elem };
            }
            //Log.Message("BPF Message: New entry added to "+cacheList);
        }

        public static void RemoveCache<T>(T elem, Dictionary<int, List<T>> cacheList, int id)
        {
            if(!cacheList.TryGetValue(id, out List<T> mods))
            {
                return;
            }

            mods.Remove(elem);

            if (mods.Count == 0)
            {
                cacheList.Remove(id);
            }
        }

        public static void Reset()
        {
            BrokenPlankCache.renderCache = new Dictionary<int, List<IRenderable>>();
            BrokenPlankCache.responderCache = new Dictionary<int, List<IDamageResponse>>();
        }
    }

}