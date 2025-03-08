using System.Collections.Generic;
using RimWorld;
using Verse;
using System;
using System.Linq;
using UnityEngine;
using RimWorld.Planet;

namespace BrokenPlankFramework
{
    public class GameComponent_SortDefs : GameComponent
    {
        public List<PawnKindDef> pawnKinds;

        public List<PawnKindDef> independentPawnKinds;

        public List<Faction> factions;

        public Dictionary<Faction, List<PawnKindDef>> pawnKindsForFactions = new Dictionary<Faction, List<PawnKindDef>>();

        private FactionManager FactionManager => Current.Game.World.factionManager;

        public GameComponent_SortDefs(Game game)
        {
        }

        public override void LoadedGame()
        {
            GetDefs();
        }

        public override void StartedNewGame()
        {
            GetDefs();
        }

        public void GetDefs()
        {
            pawnKinds = DefDatabase<PawnKindDef>.AllDefsListForReading;

            independentPawnKinds = pawnKinds.ToList();

            factions = FactionManager.AllFactionsListForReading;

            //factions.Add(new Faction());

            foreach(Faction faction in factions)
            {
                pawnKindsForFactions.Add(faction, SortToFaction(faction));
            }
        }

        public List<PawnKindDef> SortToFaction(Faction faction)
        {
            List<PawnKindDef> list = new List<PawnKindDef>();

            foreach(PawnKindDef def in pawnKinds)
            {
                if(def.defaultFactionType == faction.def)
                {
                    list.Add(def);
                    independentPawnKinds.Remove(def);
                }

                if(def.RaceProps.Insect && faction.def.techLevel == TechLevel.Animal && !faction.def.permanentEnemyToEveryoneExcept.Contains(FactionDefOf.Insect))
                {
                    list.Add(def);
                    independentPawnKinds.Remove(def);
                }                
            }

            return list;
        }
    }
}