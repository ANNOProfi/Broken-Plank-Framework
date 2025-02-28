using UnityEngine;
using Verse;

namespace BrokenPlankFramework
{
    [StaticConstructorOnStartup]
    public class Gizmo_HediffShieldStatus : Gizmo
    {
        public HediffComp_Shield shieldHediff;

        public static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));
        public static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        private HediffCompProperties_Shield Props => shieldHediff.props as HediffCompProperties_Shield;

        static Gizmo_HediffShieldStatus() { }

        public Gizmo_HediffShieldStatus(HediffComp_Shield shield)
        {
            Order = -101f;
            shieldHediff = shield;
        }

        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect backgroundRect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect drawRect = backgroundRect.ContractedBy(6f);
            Widgets.DrawWindowBackground(backgroundRect);
            Rect textRect = drawRect;
            textRect.height = backgroundRect.height / 2f - 12f;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(textRect, Props.gizmoTitle);
            Rect barRect = drawRect;
            barRect.yMin = drawRect.y + drawRect.height / 2f;
            float num = shieldHediff.energy / Mathf.Max(1f, shieldHediff.MaxEnergy);
            Widgets.FillableBar(barRect, num, Gizmo_HediffShieldStatus.FullShieldBarTex, Gizmo_HediffShieldStatus.EmptyShieldBarTex, false);
            Text.Font = GameFont.Small;
            Widgets.Label(barRect, (shieldHediff.energy).ToString("F0") + " / " + (shieldHediff.MaxEnergy).ToString("F0"));
            Text.Anchor = TextAnchor.UpperLeft;
            TooltipHandler.TipRegion(drawRect, Props.gizmoTip);
            return new GizmoResult(GizmoState.Clear);
        }
    }
}