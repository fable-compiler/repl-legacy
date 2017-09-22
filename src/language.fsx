type Person =
    | Sarah
    | Pierre
    | Andrea
    | Enrico

type Food =
    | Fruit
    | Vegetables
    | Meat
    | Fish

type Drink =
    | Water
    | Wine
    | Soda
    | Beer

let eat (o: Food) (s: Person) =
    printfn "%A eats %A" s o
    s

let drink (o: Drink) (s: Person) =
    printfn "%A drinks %A" s o
    s


Pierre
|> eat Fruit
|> drink Wine



let make s v o = v o s

make Sarah eat Fish
|> drink Water

