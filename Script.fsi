type BehindDoor = Win | Loose 
type DoorState = Picked | Unpicked

type Door = {
    BehindDoor : BehindDoor;
    State : DoorState
}

type Game = {
    Doors :  Door array
}

let WinDoor = { BehindDoor = Win; State = Unpicked }
let LooseDoor = { BehindDoor = Loose; State = Unpicked }

let newGame = { Doors =[|WinDoor;LooseDoor;LooseDoor|] }

//let random = System.Random(0);
let random = System.Random();

let newRandomGame() = 
    let randomDoorIndex = random.Next(3);
    {
        Doors = Seq.init 3 (fun i ->
            match i with 
            | i when i = randomDoorIndex -> WinDoor 
            | _ -> LooseDoor) 
            |> Seq.toArray
    }

let didContestantWin (game: Game) = 
    game.Doors |> Seq.exists(fun door -> door.State = Picked && door.BehindDoor = Win)

let printGame (game: Game) =
    printfn "%A" game
    
    //let game = newGame

    if (game |> didContestantWin)
    then printfn "Contestant won!"
    else printfn "Contestant lost!"
    ()

let contestantMove doorIndex gameState = 
    let pickDoor (doors : Door array) = 
        doors
        |> Array.mapi (fun i door -> if doorIndex = i
                                     then { door with State = Picked} 
                                     else  door)
    
    {
         gameState with Doors = (gameState.Doors |> pickDoor) 
    }

let gameShowHostMove (gameState: Game) = 
    //let gameState = newGame |> contestantMove 0

    let unpickedDoors = 
        gameState.Doors 
        |> Array.mapi (fun i door -> (i , door))
        |> Array.where (fun (_ , door) -> door.State = Unpicked)


    // TODO -Pick first -> should be random (in the future)
    let unpickedDoorToKeepIndex = 
        unpickedDoors
        |> Seq.head
        |> fst
    
    let newDoors = 
        gameState.Doors
        |> Seq.mapi (fun i d -> (i, d)) 
        |> Seq.where (fun (i, d) -> i = unpickedDoorToRemoveIndex || d.State = Picked)
        |> Seq.map snd
        |> Seq.toArray

    {
        gameState with Doors = newDoors
    }


let contestantSwap (gameState: Game) = 
    let invertDoorState door = 
        let invertState state = 
            match state with 
            | Picked -> Unpicked
            | Unpicked -> Picked
        {
            door with State = door.State |> invertState
        }

    {
        gameState with Doors = gameState.Doors |> Array.map invertDoorState 
    }

//newGame 
let playGame() = 
    let endGame = 
        newRandomGame()
        |> contestantMove 0 
        |> gameShowHostMove
        |> contestantSwap
    //    |> printGame
    
    (endGame, endGame |> didContestantWin)

seq{0..10000}
|> Seq.map (fun _ -> playGame() )
//|> Seq.map (fun (game, win) -> game |> printGame; (game,win) )
|> Seq.where snd
|> Seq.length

// Need to remove all but 1 random door