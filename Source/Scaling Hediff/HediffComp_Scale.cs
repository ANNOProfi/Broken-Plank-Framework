using Verse;

namespace BrokenPlankFramework
{
    public class HediffComp_Scale : HediffComp
    {
        public HediffCompProperties_Scale Props
        {
            get
            {
                return (HediffCompProperties_Scale)this.props;
            }
        }

        private float partEfficiencyCached = 1f;

        public override void CompPostMake()
        {
            partEfficiencyCached = Def.addedPartProps.partEfficiency;
        }

        public virtual HediffStage GetStage(HediffStage stage, float scalingStat)
        {
            stage.partEfficiencyOffset = partEfficiencyCached * scalingStat - partEfficiencyCached;

            if(partEfficiencyCached * scalingStat < Props.minimumEfficiency)
            {
                stage.partEfficiencyOffset = Props.minimumEfficiency - partEfficiencyCached;
            }

            return stage;
        }
    }
}