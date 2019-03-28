module FunctionalDesignPatternDemos.Interpreter

open System
open System.IO
open System.Xml
open System.Xml.Linq
open Microsoft.FSharp.Reflection

type Expression =
  Math of Expression list
  | Plus of lhs:Expression * rhs:Expression
  | Value of value:string
  member self.Val =
    let rec eval expr =
      match expr with 
      | Math m -> eval(m.Head)
      | Plus (lhs, rhs) -> eval lhs + eval rhs
      | Value v -> v |> int
    eval self
  
let text = @"<math>
               <plus>
                 <value>2</value>
                 <value>3</value>
               </plus>
             </math>"

let cases = FSharpType.GetUnionCases (typeof<Expression>)
            |> Array.map(fun f -> 
              (f.Name, FSharpValue.PreComputeUnionConstructor(f)))
            |> Map.ofArray
            
let makeCamelCase (text:string) =
  Char.ToUpper(text.Chars(0)).ToString() + text.Substring(1)
            
let rec recursiveBuild (root:XElement) =
  let name = root.Name.LocalName |> makeCamelCase
  
  let makeCase parameters =
    try
      let caseInfo = cases.Item name
      (caseInfo parameters) :?> Expression
    with 
    | exp -> raise <| new Exception(String.Format("Failed to create {0} : {1}", name, exp.Message))
  
  let elems = root.Elements() |> Seq.toArray
  let values = elems |> Array.map(fun f -> recursiveBuild f) 
  if elems.Length = 0 then
    let rootValue = root.Value.Trim()
    makeCase [| box rootValue |]
  else
    try
      values |> Array.map box |> makeCase
    with 
    | _ -> makeCase [| values |> Array.toList |]
    
let rec print expr =
  match expr with 
  | Math m -> print m.Head
  | Plus (lhs, rhs) -> String.Format("({0}+{1})", print lhs, print rhs)
  | Value v -> v
    
let rec eval expr =
  match expr with 
  | Math m -> eval m.Head
  | Plus (lhs, rhs) -> eval lhs + eval rhs
  | Value v -> v |> int

//[<EntryPoint>]
let main argv =
  use stringReader = new StringReader(text)
  use xmlReader = XmlReader.Create(stringReader)
  let doc = XDocument.Load(xmlReader)
  
  let parsed = recursiveBuild doc.Root 
  printf "%s = %d" (print parsed) (eval parsed)

  0 