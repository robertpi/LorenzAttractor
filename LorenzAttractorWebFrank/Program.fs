open System
open System.IO
open System.Collections.Specialized
open System.Drawing.Imaging
open System.Globalization
open System.Net
open System.Net.Http
open System.Net.Http.Formatting
open System.Net.Http.Headers
open System.Web.Http
open System.Web.Http.HttpResource
open System.Web.Http.SelfHost
open Frank
open FSharp.Control
open FSharpx.Option
open ProgFs.CrossPlatform

module HelloResource =

  let makePage sigma beta rho =
    sprintf @"<!doctype html>
<html>
    <header>
        <meta charset=utf-8>
        <title>Lorenz Attractor</title>
    </header>
    <body>
        <p><img src=""/lorenz?sigma=%f&beta=%f&rho=%f"" /></p>
        <form action=""/"" method=""post"">
            <p>Sigma: <input name=""sigma"" value=""%f""></p>
            <p>Beta: <input name=""beta"" value=""%f""></p>
            <p>Rho: <input name=""rho"" value=""%f""></p>
            <p><input type=""submit""></p>
        </form>
    </body>
</html>" 
        sigma beta rho sigma beta rho
  
  let lorenzResponse sigma beta rho =
    respond HttpStatusCode.OK 
    <| ``Content-Type`` "text/html"
    <| Some(Formatted (makePage sigma beta rho, System.Text.Encoding.UTF8, "text/html"))

  let getLorenzPage (request: HttpRequestMessage) =
    lorenzResponse LorenzSystem.sigma LorenzSystem.beta LorenzSystem.rho
    <| request
    |> async.Return
  
  let getFormItemWithDefault (formData: NameValueCollection) (name: string) defaultValue =
    try
        float formData.[name]
    with _ -> defaultValue

  let postLorenzPage (request: HttpRequestMessage) =
    async { let! formData = request.Content.ReadAsFormDataAsync()
            let sigma = getFormItemWithDefault formData "sigma" LorenzSystem.sigma
            let beta = getFormItemWithDefault formData "beta" LorenzSystem.sigma
            let rho = getFormItemWithDefault formData "rho" LorenzSystem.sigma
            return lorenzResponse sigma beta rho
            <| request }
    
  let helloResource = route "/" (get getLorenzPage <|> post postLorenzPage)

  let getQueryParam (request: HttpRequestMessage) key =
    let values = request.GetQueryNameValuePairs()
    values 
    |> Seq.tryFind (fun x -> x.Key = key)
    |> Option.map (fun x -> x.Value)

  let lorenzImage (request: HttpRequestMessage) =
    let paramWithDefault name defaultValue =
        getQueryParam request name
        |> Option.map float 
        |> Option.getWithDefault defaultValue
    let sigma = paramWithDefault "sigma" LorenzSystem.sigma
    let beta = paramWithDefault "beta" LorenzSystem.beta
    let rho = paramWithDefault "rho" LorenzSystem.rho

    let model = new LorenzViewModel(300, 300, sigma, beta, rho)
    let memStream = new MemoryStream()
    model.Bitmap.Save(memStream, ImageFormat.Png)
    memStream.Flush()
    let lengthAsInt = int memStream.Length
    let output = Array.zeroCreate lengthAsInt : byte[]
    memStream.Position <- 0L
    memStream.Read(output, 0, lengthAsInt) |> ignore
    respond HttpStatusCode.OK 
    <| ``Content-Type`` "image/png"
    <| Some(Bytes output)
    <| request
    |> async.Return

  let lorenzImageResource = route "/lorenz" (get lorenzImage)


(* Configure and run the application *)

let baseUri = "http://127.0.0.1:1000"
let config = new HttpSelfHostConfiguration(baseUri)
config |> register [ HelloResource.helloResource; HelloResource.lorenzImageResource; ]

try
    let server = new HttpSelfHostServer(config)

    server.OpenAsync().Wait()
    Console.WriteLine("Running on " + baseUri)
    Console.WriteLine("Press any key to stop.")
    Console.ReadKey() |> ignore

    server.CloseAsync().Wait()
with ex -> 
    printfn "%O" ex
    Console.ReadLine() |> ignore


