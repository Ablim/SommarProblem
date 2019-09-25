open System
open Constants
open System.Diagnostics

let isValidCell (grid : char[][]) row col =
    row >= 0 &&
    row < grid.Length &&
    col >= 0 &&
    col < grid.[row].Length

///The star of the show. Recursive search of the given
///word at a specific starting cell.
let rec searchCell grid row col word =
    match word with
    | [] -> true
    | head :: tail -> 
        if not (isValidCell grid row col) then
            false
        else if not (grid.[row].[col].Equals head) then
            false
        else
            searchCell grid (row - 1) (col - 1) tail ||
            searchCell grid (row - 1) col tail ||
            searchCell grid (row - 1) (col + 1) tail ||
            searchCell grid row (col - 1) tail ||
            searchCell grid row (col + 1) tail ||
            searchCell grid (row + 1) (col - 1) tail ||
            searchCell grid (row + 1) col tail ||
            searchCell grid (row + 1) (col + 1) tail

///Search up to all cells of the board for the given word.
let rec wordExistsOnBoardRec board row col word =
    let exists = searchCell board row col word
    if exists then
        true
    else if row = (board.Length - 1) && col = (board.[row].Length - 1) then
        false
    else if col = (board.[row].Length - 1) then
        wordExistsOnBoardRec board (row + 1) 0 word
    else
        wordExistsOnBoardRec board row (col + 1) word

///Read the board from stdin.
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

///Read words from stdin.
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

//----------Top level functions below----------

let getBoard =
    let board = readBoardAsList [] false
    let reversed = List.rev board
    let arrays = reversed |> List.map (fun x -> Array.ofSeq x)
    Array.ofSeq arrays

let getWords =
    readWordsAsList [] false

let wordExistsOnBoard board word = 
    wordExistsOnBoardRec board 0 0 (List.ofSeq word)

[<EntryPoint>]
let main argv =
    let timer = new Stopwatch()
    timer.Start()
    
    let board = getBoard
    let words = getWords

    timer.Stop()
    printfn "Input time: %i ms" timer.ElapsedMilliseconds
    timer.Restart()

    for w in words do
        let exists = wordExistsOnBoard board w
        if exists then
            printfn "%s" w
    
    timer.Stop()
    printfn "Execution time: %i ms" timer.ElapsedMilliseconds
    0