module day7

open System.Text.RegularExpressions

type Bag = { color: string; mutable contains: Map<Bag, int>  }

let rec containsGold (bag: Bag) =
    if bag.color = "shiny gold" then true
    else
        bag.contains |> Map.exists (fun b _ -> containsGold b)
        
let getBags (lines: string array): Map<string, Bag> =
    let linesAsString = String.concat " " lines
    let bags =
       Regex.Matches(linesAsString, "\\b\w+\s\w+\s(bag)")
       |> Seq.map (fun m -> m.ToString().Remove(m.ToString().Length - 4))
       |> Seq.distinct
       |> Seq.map (fun s -> s, { color = s; contains = Map.empty } )
         |> Map.ofSeq
       
    for rule in lines do
        let ruleBag = Regex.Matches(rule, "\\b\w+\s\w+\s(bags)\s")
                              |> Seq.map (fun m -> m.ToString().Remove(m.ToString().Length - 6))
                              |> List.ofSeq |> List.head
        let bag = bags[ruleBag]
        let subBags = Regex.Matches(rule, "\\b\d\s\w+\s\w+\s(bag)")
                      |> Seq.map (fun m -> m.ToString().Remove(m.ToString().Length - 4))
                      |> Seq.map (fun m -> bags[m.Substring(2)], m.Split(' ')[0] |> int)
                      |> Map.ofSeq
        bag.contains <- subBags
        
    bags

let day7part1 (lines: string array) =
    let bags = getBags lines
        
    let mutable answer = 0
    for KeyValue(id, bag) in bags do
        if id = "shiny gold" then ()
        elif containsGold bag then
            answer <- answer + 1
       
    answer.ToString()
    
let rec countBags (bag: Bag) =
    let mutable answer = 0
    for KeyValue(b, count) in bag.contains do
        answer <- answer + count + count * countBags b
    answer
    
let day7part2 (lines: string array): string =
    let bags = getBags lines
    countBags bags["shiny gold"] |> string
    
            