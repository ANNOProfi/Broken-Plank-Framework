using RimWorld;
using Verse;

namespace BrokenPlankFramework
{
    public class Hediff_ScalingAddedPart : Hediff_AddedPart
    {
        public HediffStage stage = new HediffStage();

        protected virtual float ScalingStat => 0;

        private float scalingStatCached = 1f;

        private int ticksUntilNextCheck = 0;

        public override void Tick()
        {
            if(ticksUntilNextCheck > 0)
            {
                ticksUntilNextCheck--;
                
                return;
            }

            scalingStatCached = ScalingStat;

            ticksUntilNextCheck = 60;
        }

        public override HediffStage CurStage
        {
            get
            {
                HediffStage curStage;

                if (def.stages.NullOrEmpty<HediffStage>())
                {
                    curStage = stage;
                }
                else
                {
                    curStage = def.stages[CurStageIndex];
                }

                for (int i = comps.Count - 1; i >= 0; i--)
                {
                    if (comps[i] is HediffComp_Scale scaleComp)
                    {
                        curStage = scaleComp.GetStage(curStage, scalingStatCached);
                        pawn.health.capacities.Notify_CapacityLevelsDirty();
                    }
                }

                return curStage;
            }
        }
    }
}