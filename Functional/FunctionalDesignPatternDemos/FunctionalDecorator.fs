module FunctionalDesignPatternDemos.FunctionalDecorator
open System.Diagnostics

let doWork() =
  printfn "Doing some work"
  
let logger work name =
  let sw = Stopwatch.StartNew()
  printfn "%s %s" "Entering method" name
  work()
  sw.Stop()
  printfn "Exiting method %s; %fs elapsed" name sw.Elapsed.TotalSeconds

//[<EntryPoint>]
let main argv =
  let loggedWork() = logger doWork "doWork"
  loggedWork()
  0