using Verse;
using RimWorld;

namespace BrokenPlankFramework
{
    public class CompProperties_PawnSpawner : CompProperties
    {
        public CompProperties_PawnSpawner()
        {
            compClass = typeof(Comp_PawnSpawner);
        }

        public PawnKindDef pawnKind = null;

        public FactionDef faction = null;
    }
}