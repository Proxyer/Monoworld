module Infusion.Harmonize.CompBiocodable

open HarmonyLib
open Poet.Lyric
open RimWorld
open Verse

open Infusion
open VerseTools


[<HarmonyPatch(typeof<CompBiocodable>, "CodeFor")>]
// separate flow for infusing biocodeds
module CodeFor =
  let Prefix (__instance: CompBiocodable, p: Pawn) =
    if (not (__instance :? CompBladelinkWeapon)) then
      do
        Comp.ofThing<CompInfusion> __instance.parent
        |> Option.filter (fun _ -> Settings.BiocodeBonus.handle.Value)
        |> Option.iter (fun comp ->
          let qualityInt = byte (comp.Quality) + 2uy

          // clamp!
          let quality =
            if qualityInt > byte (QualityCategory.Legendary) then
              QualityCategory.Legendary
            else
              LanguagePrimitives.EnumOfValue qualityInt

          do comp.Biocoder <- Some __instance
          do comp.SlotCount <- comp.CalculateSlotCountFor quality
          do comp.SetInfusions(CompInfusion.pickInfusions quality comp, false))

  let Postfix (__instance: CompQuality) =
    // See Harmonize.CompQuality.SetQuality:Postfix
    do Comp.getParent __instance |> resetHP
