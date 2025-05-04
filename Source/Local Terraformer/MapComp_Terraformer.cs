using Verse;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System.Linq;
using RimWorld;

namespace BrokenPlankFramework
{
    public class MapComp_Terraformer : MapComponent
    {
        private int interval = 2500;
        
        public List<IntVec3> claimedCells = new List<IntVec3>();

        public List<IntVec3> dirtyCells = new List<IntVec3>();

        private Dictionary<IntVec3, TerrainDef> originalTerrain = new Dictionary<IntVec3, TerrainDef>();

        public MapComp_Terraformer(Map map) : base(map)
        {
        }

        public override void MapGenerated()
        {
            base.MapGenerated();
            foreach(IntVec3 cell in map.AllCells)
            {
                originalTerrain.Add(cell, map.terrainGrid.TerrainAt(cell));
            }
        }

        public void RegisterTerraformer(List<IntVec3> cells)
        {
            //Log.Message("Terraformer registering");
            foreach(IntVec3 cell in cells)
            {
                claimedCells.Add(cell);

                if(dirtyCells.Contains(cell))
                {
                    dirtyCells.Remove(cell);
                }
            }

            //Log.Message("Terraformer registered. Claimed cells: "+claimedCells.Count);
        }

        public void DeRegisterTerraformer(List<IntVec3> cells)
        {
            //Log.Message("Terraformer deregistering");
            foreach(IntVec3 cell in cells)
            {
                claimedCells.Remove(cell);

                if(!claimedCells.Contains(cell))
                {
                    dirtyCells.Add(cell);
                }
            }

            //Log.Message("Terraformer registered. Dirty cells: "+dirtyCells.Count);
        }

        public override void MapComponentTick()
        {
            if(Find.TickManager.TicksGame % interval == 0 && !dirtyCells.NullOrEmpty())
            {
                IntVec3 reverseCell = dirtyCells.Where(cell => originalTerrain[cell] != cell.GetTerrain(map)).RandomElement();

                map.terrainGrid.SetTerrain(reverseCell, originalTerrain[reverseCell]);

                dirtyCells.Remove(reverseCell);
            }
        }
    }
}