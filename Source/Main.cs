using System.Collections.Generic;
using Verse;
using HarmonyLib;

namespace BrokenPlankFramework
{
    [StaticConstructorOnStartup]
    public static class Start
    {
        static Start()
        {
            Log.Message("BPF Message: Broken Plank Framework loaded successfully!");
        }
    }
}
