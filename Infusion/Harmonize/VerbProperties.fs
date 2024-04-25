module Infusion.Harmonize.VerbProperties

open System
open System.Reflection.Emit

open HarmonyLib
open Verse


[<HarmonyPatch(typeof<VerbProperties>, "GetHitChanceFactor")>]
module GetHitChanceFactor =
  /// Changes max accuracy to 200%.
  let Transpiler (instructions: seq<CodeInstruction>) =
    let insts = Array.ofSeq instructions

    // Mathf.Clamp(value, 0.01f, 1f)
    //                           ~~
    let targetOpCodePos =
      insts
      |> Array.tryFindIndex (fun code ->
        code.opcode = OpCodes.Ldc_R4
        && Convert.ToSingle code.operand = 1.0f)

    match targetOpCodePos with
    | Some s -> insts.[s].operand <- 2.0f
    | None ->
      Log.Warning(
        "[Infusion 2] Couldn't find matching opCode for VerbProperties.GetHitChanceFactor(). Can't apply accuracy overcapping."
      )

    seq insts
