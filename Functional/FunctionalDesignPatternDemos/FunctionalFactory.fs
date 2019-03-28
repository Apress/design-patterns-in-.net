module FunctionalDesignPatternDemos.FunctionalFactory

type ICountryInfo =
  abstract member Capital : string

type Country =
  | USA
  | UK
with
  static member Create = function
    | "USA" | "America" -> USA
    | "UK" | "England" -> UK
    | _ -> failwith "No such country"
  
let make country =
  match country with
  | USA -> { new ICountryInfo with
             member x.Capital = "Washington" }
  | UK -> { new ICountryInfo with
            member x.Capital = "London" }

//[<EntryPoint>]
let main argv =
  
  let uk = make Country.UK
  printfn "%s" uk.Capital // London
  
  let usa = Country.Create "America"
  
  0