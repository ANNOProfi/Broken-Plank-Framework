using Verse;
using RimWorld;

namespace BrokenPlankFramework
{
    [DefOf]
    public static class BPF_DefOf
    {
        static BPF_DefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BPF_DefOf));
		}

        public static ThingDef BPF_Human_Dummy;

        //public static PawnKindDef BPF_Dummy;

        public static ThinkTreeDef BPF_Dummy;

        public static ThinkTreeDef BPF_DummyConstant;

        public static DesignationCategoryDef BPF_Dev;
    }
}