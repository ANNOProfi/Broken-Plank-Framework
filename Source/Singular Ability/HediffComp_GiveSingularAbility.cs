using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BrokenPlankFramework
{
    public class HediffComp_GiveSingularAbility : HediffComp_GiveAbility
    {
        public HediffCompProperties_GiveSingularAbility Props => props as HediffCompProperties_GiveSingularAbility;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            for (int i = Props.abilityDefs.Count - 1; i >= 0; i--)
            {
                Ability ability = parent.pawn.abilities.GetAbility(Props.abilityDefs[i]);

                if (ability == null)
                {
                    parent.pawn.abilities.GainAbility(Props.abilityDefs[i]);
                    continue;
                }

                for (int j = ability.comps.Count - 1; j >= 0; j--)
                {
                    CompAbility_SingularTracker tracker = ability.comps[j] as CompAbility_SingularTracker;

                    if (tracker != null)
                    {
                        tracker.AddAbility();
                    }
                }
            }
        }

        public override void CompPostPostRemoved()
        {
            for (int i = 0; i < Props.abilityDefs.Count; i++)
            {
                Ability ability = parent.pawn.abilities.GetAbility(Props.abilityDefs[i]);

                if (ability == null)
                {
                    continue;
                }

                for (int j = ability.comps.Count - 1; j >= 0; j--)
                {
                    CompAbility_SingularTracker tracker = ability.comps[j] as CompAbility_SingularTracker;

                    if (tracker != null)
                    {
                        tracker.RemoveAbility();
                    }
                }
            }
        }
    }

    public class HediffCompProperties_GiveSingularAbility : HediffCompProperties
    {
        public List<AbilityDef> abilityDefs;

        public HediffCompProperties_GiveSingularAbility()
        {
            compClass = typeof(HediffComp_GiveSingularAbility);
        }
    }
}