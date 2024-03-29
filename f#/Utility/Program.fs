﻿// For more information see https://aka.ms/fsharp-console-apps

open System.IO
open Common

let year = 2023

let inputDirectoy = @$"{Input.projectRootDirectory}\Input\{year}\"

let mutable existingDirs = Directory.GetDirectories(inputDirectoy)

for day in 1..25 do
    if not (existingDirs |> Array.exists (_.EndsWith("{day}"))) then
        Directory.CreateDirectory(@$"{inputDirectoy}day{day.ToString()}") |> ignore
        
existingDirs <- Directory.GetDirectories(inputDirectoy)
    

let testFile (dir: string) =
    @$"{dir}\test.txt"
    
let inputFile (dir: string) =
    @$"{dir}\input.txt"

for dir in existingDirs do
    if dir.Contains("day") then
        if not (File.Exists(testFile(dir))) then
            File.Create(testFile(dir)) |> ignore
        if not (File.Exists(inputFile(dir))) then
            File.Create(inputFile(dir)) |> ignore
        