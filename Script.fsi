type BehindDoor = Win | Loose 
type DoorState = Picked | Unpicked

type Door = {
    name : string
    BehindDoor : BehindDoor;
    State : DoorState
}

type Game = {
    Doors :  Door array
}

let WinDoor name = {
    name = name; 
    BehindDoor = Win;
    State = Unpicked 
}

let LooseDoor name = { 
    name = name;
    BehindDoor = Loose; 
    State = Unpicked 
}

let newGame = { 
    Doors = [|WinDoor "door 1";LooseDoor "door 2" ;LooseDoor "door 3"|] 
}

//let random = System.Random(0);
let random = System.Random();

let newRandomGame() = 
    let randomDoorIndex = random.Next(3);
    {
        Doors = Seq.init 3 (fun i ->
            let doorName = sprintf "door %d" i
            match i with 
            | i when i = randomDoorIndex -> WinDoor (doorName)
            | _ -> LooseDoor (doorName)  ) 
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

let pickRandom (items: array<'T>) = 
    items.[random.Next(items |> Array.length)]

let gameShowHostMove (gameState: Game) = 
    //let gameState = newGame |> contestantMove 0
    //let gameState = game |> contestantMove 0 
    
    let unpickedDoors = gameState.Doors |> Array.where (fun d -> d.State = DoorState.Unpicked)

    let unpickedWinningDoor = 
        unpickedDoors
        |> Array.where (fun d -> d.BehindDoor = BehindDoor.Win)
        |> Array.tryHead

    let doorToKeep = 
        match unpickedWinningDoor with
        | Some (winningDoor) -> winningDoor
        | None ->   unpickedDoors
                    //|> Seq.head
                    |> pickRandom
        
    let newDoors = 
      gameState.Doors 
      |> Array.where (fun door -> door.State = Picked || door = doorToKeep)

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


let game = newRandomGame()
game
|> contestantMove 0 
|> gameShowHostMove
|> contestantSwap
|> didContestantWin
