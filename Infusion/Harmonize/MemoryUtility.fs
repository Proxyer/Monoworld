﻿module Infusion.Harmonize.MemoryUtility

open HarmonyLib
open Verse.Profile

open Infusion


[<HarmonyPatch(typeof<MemoryUtility>, "ClearAllMapsAndWorld")>]
module ClearAllMapsAndWorld =
  let Postfix () = CompInfusion.ClearCaches()
