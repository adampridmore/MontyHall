// Learn more about F# at http://fsharp.org

open System
open MontyHall

[<EntryPoint>]
let main argv =
    seq{0..10000}
    |> Seq.map (fun _ -> playGame() )
    //|> Seq.map (fun (game, win) -> game |> printGame; (game,win) )
    |> Seq.where snd
    |> Seq.length
    |> printfn "Won %A games."

    0 // return an integer exit code
