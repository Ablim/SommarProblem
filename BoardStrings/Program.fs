open System
open Constants
open System.Diagnostics

let isValid (grid : char[][]) row col =
    row >= 0 &&
    row < grid.Length &&
    col >= 0 &&
    col < grid.[row].Length

let rec search grid row col word =
    match word with
    | [] -> true
    | head :: tail -> 
        if not (isValid grid row col) then
            false
        else if not (grid.[row].[col].Equals head) then
            false
        else
            search grid (row - 1) (col - 1) tail ||
            search grid (row - 1) col tail ||
            search grid (row - 1) (col + 1) tail ||
            search grid row (col - 1) tail ||
            search grid row (col + 1) tail ||
            search grid (row + 1) (col - 1) tail ||
            search grid (row + 1) col tail ||
            search grid (row + 1) (col + 1) tail

let rec readBoardAsList (data : string list) read =
    let row = Console.ReadLine()
    match row with
    | BoardStart -> readBoardAsList data true
    | BoardEnd -> data
    | _ ->
        if read then
            readBoardAsList (row :: data) read
        else
            readBoardAsList data read

let readBoard =
    let board = readBoardAsList [] false
    let reversed = List.rev board
    let arrays = reversed |> List.map (fun x -> Array.ofSeq x)
    Array.ofSeq arrays

let rec readWordsAsList (data : string list) read =
    let row = Console.ReadLine()
    match row with
    | ListStart -> readWordsAsList data true
    | ListEnd -> data
    | _ ->
        if read then
            readWordsAsList (row :: data) read
        else
            readWordsAsList data read

[<EntryPoint>]
let main argv =
    let timer = new Stopwatch()
    timer.Start()
    
    let board = readBoard
    let words = readWordsAsList [] false

    timer.Stop()
    printfn "Input time: %i ms" timer.ElapsedMilliseconds
    timer.Restart()

    for i = 0 to (board.Length - 1) do
        for j = 0 to (board.[i].Length - 1) do
            for w in words do
                let exists = search board i j (List.ofSeq w)
                if exists then
                    printfn "%s" w
    
    timer.Stop()
    printfn "Execution time: %i ms" timer.ElapsedMilliseconds
    0