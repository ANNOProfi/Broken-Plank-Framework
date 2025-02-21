# Broken Plank Framework

This is the github page for the Broken Plank Framework for RimWorld.

Until the wiki is made, refer to this here text on what is in it and how to use it.

## Shield Hediff
Due to the Athena Framework being on developmental pause, I wanted to protect the functionalities that I use for my mods from potential incompatibilities from furture updates, so I reverse-engineered them here. Both function identically to the original version, so you can refer to the [Athena Framework Wiki][https://github.com/SmArtKar/AthenaFramework/wiki/Shield-Hediffs] on how to use it.

## Singular Ability Hediff
The same goes for the [singular ability hediffs][https://github.com/SmArtKar/AthenaFramework/wiki/Singular-Hediff-Abilities].

## Scaling Hediffs
This is designed for implants to scale their part efficiency according to other stats of the pawn, like psychic sensitivity.

To use it, you need to do two things:
1. Change the hediffClass of your implant to BrokenPlankFramework.Hediff_ScalingAddedPart.
2. Add the HediffComp_Scale comp and add your desired StatDef in the properties.

For C# users, you can customize the scaling mechanic by changing the ScalingStat variable in the Hediff_ScalingAddedPart to use whatever multiplier you want to use that is not a regular stat.
Be sure to set useStat to false in the CompProperties.