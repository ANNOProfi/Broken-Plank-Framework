using RimWorld;
using Verse;

namespace BrokenPlankFramework
{
    public class Hediff_ScalingAddedPart : Hediff_AddedPart
    {
        public HediffStage stage = new HediffStage();

        protected virtual float ScalingStat => pawn.GetStatValue(scaleStat);
        /*{
            get
            {
                return pawn.GetStatValue(scaleStat);
            }
        }*/

        private StatDef scaleStat = null;

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
                        if(scaleStat == null)
                        {
                            scaleStat = scaleComp.Props.scaleStat;
                            if(scaleStat == null && scaleComp.Props.useStat)
                            {
                                Log.Error("BPF Error: No valid scaling stat for "+ def.defName +" on pawn "+pawn.Name);
                            }
                            scalingStatCached = ScalingStat;
                        }

                        curStage = scaleComp.GetStage(curStage, scalingStatCached);
                        pawn.health.capacities.Notify_CapacityLevelsDirty();
                    }
                }

                return curStage;
            }
        }
    }
}