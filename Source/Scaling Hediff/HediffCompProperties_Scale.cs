using RimWorld;
using Verse;

namespace BrokenPlankFramework
{
    public class HediffCompProperties_Scale : HediffCompProperties
    {
        public HediffCompProperties_Scale()
        {
            this.compClass = typeof(HediffComp_Scale);
        }

        public float minimumEfficiency = 1f;

        public StatDef scaleStat;

        public bool useStat = true;
    }
}