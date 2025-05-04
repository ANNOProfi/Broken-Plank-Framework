using Verse;
using UnityEngine;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace BrokenPlankFramework
{
    public class PlaceWorker_ShowRadius : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            CompProperties_Terraformer compProperties = def.GetCompProperties<CompProperties_Terraformer>();

            GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white);
        }
    }
}