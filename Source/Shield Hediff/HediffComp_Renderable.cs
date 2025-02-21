using Verse;
using UnityEngine;
using RimWorld;

namespace BrokenPlankFramework
{
    [StaticConstructorOnStartup]

    public class HediffComp_Renderable : HediffComp, IRenderable
    {
        private HediffCompProperties_Renderable Props => props as HediffCompProperties_Renderable;

        public virtual void DrawAt(Vector3 drawPos, BodyTypeDef bodyType)
        {
            if (Props.onlyRenderWhenDrafted && (Pawn.drafter == null || !Pawn.drafter.Drafted))
            {
                return;
            }

            if (Props.graphicData != null)
            {
                Props.graphicData.Graphic.Draw(new Vector3(drawPos.x, Props.altitude.AltitudeFor(), drawPos.z), Pawn.Rotation, Pawn);
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            BrokenPlankCache.AddCache(this, ref BrokenPlankCache.renderCache, Pawn.thingIDNumber);
        }

        public override void CompExposeData()
        {
            base.CompExposeData();

            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                BrokenPlankCache.AddCache(this, ref BrokenPlankCache.renderCache, Pawn.thingIDNumber);
            }
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            BrokenPlankCache.RemoveCache(this, BrokenPlankCache.renderCache, Pawn.thingIDNumber);
        }
    }
}