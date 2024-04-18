namespace Infusion.OnHitWorkers

open Poet.Lib
open Verse

open Infusion
open VerseTools


type IfBioTargetHit =
  inherit OnHitWorker

  val mutable value: OnHitWorker

  new() = { value = null }

  override this.BulletHit record =
    do
      record.target
      |> Option.bind tryCast<Pawn>
      |> Option.filter Pawn.isAliveAndWell
      |> Option.iter (fun _ -> do this.value.BulletHit record)

  override this.ConfigErrors() =
    if isNull this.value then
      seq { "no value" }
    else
      Seq.empty

  override this.MeleeHit record =
    do
      tryCast<Pawn> record.target
      |> Option.filter Pawn.isAliveAndWell
      |> Option.iter (fun _ -> do this.value.MeleeHit record)
