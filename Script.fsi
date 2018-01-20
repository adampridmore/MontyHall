#load "MontyHall.fs"

open MontyHall

seq{0..10000}
|> Seq.map (fun _ -> playGame() )
//|> Seq.map (fun (game, win) -> game |> printGame; (game,win) )
|> Seq.where snd
|> Seq.length

// Need to remove all but 1 random door
