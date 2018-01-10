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

//let contestantMode (doors : Door array) = 

