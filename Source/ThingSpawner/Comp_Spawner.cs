using Verse;
using RimWorld;
using System.Collections.Generic;
using System;
using RimWorld.Planet;
using System.Linq;

namespace BrokenPlankFramework
{
    public class Comp_PawnSpawner : ThingComp
    {
        public CompProperties_PawnSpawner Props => (CompProperties_PawnSpawner)props; 
        private readonly GameComponent_SortDefs component = Current.Game.GetComponent<GameComponent_SortDefs>();

        public List<Faction> Factions => component.factions;

        public Dictionary<Faction, List<PawnKindDef>> PawnKindsForFactions => component.pawnKindsForFactions;

        public List<PawnKindDef> IndependentPawnKinds => component.independentPawnKinds;

        public Faction selectedFaction = null;

        public Faction previousFaction = null;

        public PawnKindDef selectedPawnKind = null;

        public Pawn spawnedPawn = null;

        public override void PostPostMake()
        {
            Log.Message("BPF Message: Spawner comp generated");

            selectedFaction = (Faction)Factions.FirstOrDefault(f => f.def == Props.faction);

            selectedPawnKind = Props.pawnKind;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }

            if(DebugSettings.godMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Select Faction".Translate(),
                    action = delegate
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();
                        foreach(Faction faction in Factions)
                        {
                            list.Add(new FloatMenuOption(faction.def.defName, delegate
                            {
                                selectedFaction = faction;
                            }));
                        }
                        list.Add(new FloatMenuOption("Independent", delegate
                        {
                            selectedFaction = null;
                        }));
                        Find.WindowStack.Add(new FloatMenu(list));
                    }
                };

                if((selectedFaction != null && !PawnKindsForFactions[selectedFaction].NullOrEmpty()) || !IndependentPawnKinds.NullOrEmpty())
                {
                    if(selectedFaction != previousFaction)
                    {
                        selectedPawnKind = null;
                    }

                    previousFaction = selectedFaction;

                    if(selectedFaction == null && !IndependentPawnKinds.NullOrEmpty())
                    {
                        yield return new Command_Action
                        {
                            defaultLabel = "Select pawnKind".Translate(),
                            action = delegate
                            {
                                List<FloatMenuOption> list = new List<FloatMenuOption>();
                                foreach(PawnKindDef pawnKind in IndependentPawnKinds)
                                {
                                    list.Add(new FloatMenuOption(pawnKind.defName, delegate
                                    {
                                        selectedPawnKind = pawnKind;
                                    }));
                                }
                                Find.WindowStack.Add(new FloatMenu(list));
                            }
                        };
                    }
                    else
                    {
                        yield return new Command_Action
                        {
                            defaultLabel = "Select pawnKind".Translate(),
                            action = delegate
                            {
                                List<FloatMenuOption> list = new List<FloatMenuOption>();
                                foreach(PawnKindDef pawnKind in PawnKindsForFactions[selectedFaction])
                                {
                                    list.Add(new FloatMenuOption(pawnKind.defName, delegate
                                    {
                                        selectedPawnKind = pawnKind;
                                    }));
                                }
                                Find.WindowStack.Add(new FloatMenu(list));
                            }
                        };
                    }
                }

                if(selectedPawnKind != null)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "Spawn dummy".Translate(),
                        action = SpawnDummy
                    };
                }

                if(spawnedPawn != null)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "Despawn dummy".Translate(),
                        action = DespawnDummy
                    };
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            string selection = "";

            if(selectedFaction != null)
            {
                selection = "Faction: "+selectedFaction.def.defName;
            }
            else if(!IndependentPawnKinds.NullOrEmpty())
            {
                selection = "Faction: Independent";
            }
            if(selectedPawnKind != null)
            {
                if(selection != "")
                {
                    selection += "\n";
                }
                selection += "PawnKind: "+selectedPawnKind.defName;
            }
            if(spawnedPawn != null && !spawnedPawn.RaceProps.IsMechanoid && !spawnedPawn.RaceProps.Insect && !spawnedPawn.RaceProps.IsAnomalyEntity)
            {
                if(selection != "")
                {
                    selection += "\n";
                }
                selection += "Spawned Pawn: "+spawnedPawn.Name;
            }

            return selection;
        }

        public void SpawnDummy()
        {
            spawnedPawn = PawnGenerator.GeneratePawn(selectedPawnKind, selectedFaction);

            spawnedPawn.RaceProps.thinkTreeConstant = BPF_DefOf.BPF_DummyConstant;

            spawnedPawn.RaceProps.thinkTreeMain = BPF_DefOf.BPF_Dummy;

            spawnedPawn.RaceProps.foodType = FoodTypeFlags.None;

            spawnedPawn.RaceProps.alwaysAwake = true;

            spawnedPawn.RaceProps.doesntMove = true;

            spawnedPawn.RaceProps.needsRest = false;
            
            /*if(!selectedPawnKind.RaceProps.IsMechanoid && !selectedPawnKind.RaceProps.Insect && !selectedPawnKind.RaceProps.IsAnomalyEntity)
            {
                List<Hediff> spawnedPawnHediffs = spawnedPawn.health.hediffSet.hediffs;

                foreach(Hediff hediff in spawnedPawnHediffs)
                {
                    if(hediff.def.isBad)
                    {
                        spawnedPawn.health.RemoveHediff(hediff);
                    }
                }
            }*/

            GenSpawn.Spawn(spawnedPawn, parent.Position, parent.Map, parent.Rotation.Opposite);
        }

        public void DespawnDummy()
        {
            spawnedPawn.Destroy();
            spawnedPawn = null;
        }
    }
}