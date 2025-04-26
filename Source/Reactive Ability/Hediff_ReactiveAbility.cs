using Verse;
using RimWorld;
using System.Collections.Generic;
using Verse.Sound;
using UnityEngine;
using System;

namespace BrokenPlankFramework
{
    public class Hediff_ReactiveAbility : HediffWithComps
    {
        private List<Ability> abilities = new List<Ability>();

        public List<Ability> Abilities => abilities;

        private Hediff_ReactiveAbility_MinimumHealthExtension minimumHealthExtension;

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            PopulateList();
            minimumHealthExtension = def.GetModExtension<Hediff_ReactiveAbility_MinimumHealthExtension>();
            //Log.Message("BPF Message: Abilities populated: "+abilities.ToString());
        }

        public void PopulateList()
        {
            HediffComp_GiveAbility abilityGiver = this.TryGetComp<HediffComp_GiveAbility>();

            HediffComp_GiveSingularAbility singleAbilityGiver = this.TryGetComp<HediffComp_GiveSingularAbility>();

            if(abilityGiver != null)
            {
                HediffCompProperties_GiveAbility abilityGiverProps = (HediffCompProperties_GiveAbility)abilityGiver.props;
            
                if(abilityGiverProps.abilityDef != null)
                {
                    foreach(Ability ability in pawn.abilities.AllAbilitiesForReading)
                    {
                        //Log.Message("BPF Message: Checking ability "+ability.def.defName);
                        if(abilityGiverProps.abilityDef == ability.def)
                        {
                            abilities.Add(ability);
                            //Log.Message("BPF Message: Added ability "+ability.def.defName);
                        }
                    }
                }

                if(!abilityGiverProps.abilityDefs.NullOrEmpty())
                {
                    //Log.Message("BPF Message: Adding abilities");
                    foreach(Ability ability in pawn.abilities.AllAbilitiesForReading)
                    {
                        //Log.Message("BPF Message: Checking ability "+ability.def.defName);
                        if(abilityGiverProps.abilityDefs.Contains(ability.def))
                        {
                            abilities.Add(ability);
                            //Log.Message("BPF Message: Added ability "+ability.def.defName);
                        }
                    }
                }
            }

            if(singleAbilityGiver != null && !singleAbilityGiver.Props.abilityDefs.NullOrEmpty())
            {
                //Log.Message("BPF Message: Adding abilities");
                foreach(Ability ability in pawn.abilities.AllAbilitiesForReading)
                {
                    //Log.Message("BPF Message: Checking ability "+ability.def.defName);
                    if(singleAbilityGiver.Props.abilityDefs.Contains(ability.def))
                    {
                        abilities.Add(ability);
                        //Log.Message("BPF Message: Added ability "+ability.def.defName);
                    }
                }
            }
        }
        
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if(minimumHealthExtension != null && pawn.health.summaryHealth.SummaryHealthPercent <= minimumHealthExtension.minimumHealth)
            {
                //Log.Message("BPF Message: Damage recieved");
                foreach(Ability ability in abilities)
                {
                    //Log.Message("BPF Message: Checking for cast");
                    if(!pawn.stances.stunner.Stunned && ability.CanCast && dinfo.Instigator.Position.DistanceTo(pawn.Position) <= ability.verb.verbProps.range)
                    {
                        //Log.Message("BPF Message: Casting ability");
                        if(ability.verb.verbProps.soundCast != null)
                        {
                            ability.verb.verbProps.soundCast.PlayOneShot(new TargetInfo(pawn.Position, pawn.MapHeld));
                        }
                        if(ability.verb.verbProps.soundCastTail != null)
                        {
                            ability.verb.verbProps.soundCastTail.PlayOneShotOnCamera(pawn.Map);
                        }

                        int maxExclusive = GenRadial.NumCellsInRadius(ability.verb.verbProps.ForcedMissRadius);
                        int num = Rand.Range(0, maxExclusive);

                        if(num == 0)
                        { 
                            ability.Activate(new LocalTargetInfo(dinfo.Instigator), null);
                        }
                        else
                        {
                            ability.Activate(new LocalTargetInfo(dinfo.Instigator.Position + GenRadial.RadialPattern[num]), null);
                        }
                    }
                }
            }
        }
    }
}