module FunctionalDesignPatternDemos.FunctionalBuilder
open System.Text

let p args =
  let allArgs = args |> String.concat "\n"
  ["<p>"; allArgs; "</p>"] |> String.concat "\n"

let img url = "<img src=\"" + url + "\"/>"

[<EntryPoint>]
let main argv =
  let html =
    p [
      "Check out this picture";
      img "pokemon.com/pikachu.png"
    ]
  printfn "%s" html
  
  0