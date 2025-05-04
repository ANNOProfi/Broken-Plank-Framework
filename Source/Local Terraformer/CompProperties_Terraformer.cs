using Verse;

namespace BrokenPlankFramework
{
    public class CompProperties_Terraformer : CompProperties
    {
        public CompProperties_Terraformer()
        {
            compClass = typeof(CompTerraformer);
        }

        public float radius;

        public int interval = 60;

        public bool shouldDecay = true;

        public TerrainDef terrain;
    }
}