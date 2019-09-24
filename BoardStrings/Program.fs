open System
open Constants

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
    let board = readBoard
    let words = readWordsAsList [] false

    let lala = board.Length
    let lala2 = Array.length board


    for w in words do
        for i = 0 to board.Length do
            for j = 0 to board.[i].Length do
                let exists = search board i j (List.ofSeq w)
                if exists then
                    printfn "%s" w
    
    //let apa = "APA"
    //let apaList = List.ofSeq apa
    //let apaArray = Array.ofSeq apa

    //let arr1 = [| 'A'; 'P' |]
    //let arr2 = [| 'B'; 'U' |]
    //let arr3 = [| 'U' |]

    //let matrix = [| arr1; arr2; arr3|]

    //printfn "%b" (search matrix 0 0 ['A'; 'P'; 'A'])

    0




(*
    for each string s
        save s in a map
    
    for x = 0..width
        for y = 0..height
            for each string s in map
                if s begins with char @ (x, y)
                    save (x, y) with s in map

    for each string s in map
        for each coordinate c with s
            look for s at c recursively

    rec func(matrix, x, y, [a:as])
        if !valid coordinates
            return false
        
        if []
            return true

        if matrix(x, y) != a
            return false
        else
            for each 8 directions dx, dy
                return true && func(matrix, dx, dy, as)
*)