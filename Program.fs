// Learn more about F# at http://fsharp.org

open System

type Door = Win | Loose

type Game = {
    Doors :  Door array
}

let newGame = {Doors =[|Win;Loose;Loose|] }

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code
