using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BrokenPlankFramework
{
    public class CompTerraformer : ThingComp
    {
        public CompProperties_Terraformer Props => (CompProperties_Terraformer)props;

        private MapComp_Terraformer component;

        private List<IntVec3> cells = new List<IntVec3>();

        private List<IntVec3> originalCells;

        protected virtual bool Active => parent.Spawned;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            /*if (!respawningAfterLoad)
            {
                parent.Map.terrainGrid.SetTerrain(parent.Position, Props.terrain);
            }*/

            cells = GetCells();

            if(Props.shouldDecay)
            {
                originalCells = cells.ToList();

                component = parent.Map.GetComponent<MapComp_Terraformer>();

                component.RegisterTerraformer(originalCells);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            if(Props.shouldDecay)
            {
                component.DeRegisterTerraformer(originalCells);
            }
        }

        protected bool CellValidator(IntVec3 cell)
        {
            TerrainDef terrain = cell.GetTerrain(parent.Map);
            if (terrain != null && terrain != Props.terrain && TerrainValidator(terrain))
            {
                //Log.Message("Terrain valid");
                Building building = cell.GetEdifice(parent.Map);

                return building == null || building == parent;
            }
            //Log.Message("Cell not valid");
            return false;
        }

        public static bool TerrainValidator(TerrainDef terrain)
        {
            if (!terrain.IsWater && !terrain.layerable && (terrain.natural || (terrain.affordances != null && terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))))
            {
                return !terrain.HasTag("Road");
            }
            //Log.Message("Terrain not valid");
            return false;
        }

        protected List<IntVec3> GetCells()
        {
            List<IntVec3> cells = (from cell in GenRadial.RadialCellsAround(parent.Position, Props.radius, useCenter: true)
            where cell.InBounds(parent.Map) && CellValidator(cell)
            select cell).ToList();

            //Log.Message("Cell count: "+cells.Count);

            return cells;
        }

        public override void CompTick()
        {
            //Log.Message("interval elapsed: "+Find.TickManager.TicksGame % Props.interval == 0+", Active: "+Active+", cells empty: "+cells.NullOrEmpty());
            if(Find.TickManager.TicksGame % Props.interval == 0 && Active && !cells.NullOrEmpty())
            {
                //Log.Message("Cells not empty, size: "+cells.Count);

                IntVec3 cell = cells.OrderBy((IntVec3 x) => x.DistanceTo(parent.Position)).FirstOrDefault();

                if(cell != default)
                {
                    //Log.Message("Cell not default");
                    parent.Map.terrainGrid.SetTerrain(cell, Props.terrain);
                }

                cells = GetCells();
            }
            else if(Find.TickManager.TicksGame % (Props.interval*2) == 0 && cells.NullOrEmpty())
            {
                cells = GetCells();
            }
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            GenDraw.DrawRadiusRing(parent.Position, Props.radius, Color.white);
        }
    }
}