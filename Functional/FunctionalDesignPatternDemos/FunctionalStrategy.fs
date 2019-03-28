module FunctionalDesignPatternDemos.FunctionalStrategy

let processList items startToken itemAction endToken =
  let mid = items |> (Seq.map itemAction) |> (String.concat "\n")
  [startToken; mid; endToken] |> String.concat "\n"
  
let processListHtml items =
  processList items "<ul>" (fun i -> "  <li>" + i + "</li>") "</ul>"
  
let processListMarkdown items =
  processList items "" (fun i -> " * " + i) ""

//[<EntryPoint>]
let main argv =
  let items = ["hello"; "world"]
  printfn "%s" (processListHtml items)
  printfn "%s" (processListMarkdown items)
  0