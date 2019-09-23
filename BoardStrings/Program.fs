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

//let readBoard =
//    while true do
//        let row = Console.ReadLine()

//    [||]

[<EntryPoint>]
let main argv =
    printfn "Hello"
    
    let apa = "APA"
    let apaList = List.ofSeq apa
    let apaArray = Array.ofSeq apa

    let arr1 = [| 'A'; 'P' |]
    let arr2 = [| 'B'; 'U' |]
    let arr3 = [| 'U' |]

    let matrix = [| arr1; arr2; arr3|]

    printfn "%b" (search matrix 0 0 ['A'; 'P'; 'A'])

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