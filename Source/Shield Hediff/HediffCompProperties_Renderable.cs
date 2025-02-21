using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using RimWorld;
using System.Reflection;

namespace BrokenPlankFramework
{
    public class HediffCompProperties_Renderable : HediffCompProperties
    {
        public HediffCompProperties_Renderable()
        {
            this.compClass = typeof(HediffComp_Renderable);
        }

        // Displayed graphic. This graphic is drawn above the pawn at MoteOverhead altitude layer by default
        public GraphicData graphicData;
        
        // Altitude for graphic rendering
        public AltitudeLayer altitude = AltitudeLayer.MoteOverhead;

        // If this graphic should be rendered only when the pawn is drafted. Overridden by graphic package settings
        public bool onlyRenderWhenDrafted = false;
    }
}