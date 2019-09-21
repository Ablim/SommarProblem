open System

let rec search grid x y word =
    match word with
    | [] -> true
    | head :: tail -> 
        search grid x y tail

[<EntryPoint>]
let main argv =
    printf("Hello")
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