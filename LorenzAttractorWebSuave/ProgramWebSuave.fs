open System
open System.Drawing.Imaging
open System.IO
open System.Net

open Suave.Types
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Log
open ProgFs.CrossPlatform


let makePage sigma beta rho =
    sprintf @"<!doctype html>
<html>
    <header>
        <meta charset=utf-8>
        <title>Lorenz Attractor</title>
    </header>
    <body>
        <p><img src=""/%f/%f/%f/lorenz.png"" /></p>
        <form action=""/"" method=""post"">
            <p>Sigma: <input name=""sigma"" value=""%f""></p>
            <p>Beta: <input name=""beta"" value=""%f""></p>
            <p>Rho: <input name=""rho"" value=""%f""></p>
            <p><input type=""submit""></p>
        </form>
    </body>
</html>" 
        sigma beta rho sigma beta rho

let getFormParam formData key defaultValue =
    formData 
    |> Seq.tryFind (fun (key', _) -> key' = key)
    |> Option.map (fun (_, value) -> Option.getWithDefault (string defaultValue) value |> float)
    |> Option.getWithDefault defaultValue

let pageOfFormData formData =
    let simga = getFormParam formData "sigma" LorenzSystem.sigma
    let beta = getFormParam formData "beta" LorenzSystem.beta
    let rho = getFormParam formData "rho" LorenzSystem.rho
    makePage simga beta rho

let lorenzImage sigma beta rho =
    let model = new LorenzViewModel(300, 300, sigma, beta, rho)
    use memStream = new MemoryStream()
    model.Bitmap.Save(memStream, ImageFormat.Png)
    memStream.Flush()
    let lengthAsInt = int memStream.Length
    let output = Array.zeroCreate lengthAsInt : byte[]
    memStream.Position <- 0L
    memStream.Read(output, 0, lengthAsInt) |> ignore
    output

let logger = Loggers.sane_defaults_for Debug

let app : WebPart =
    choose [
       log logger log_format >>= never;
       GET >>= url "/" >>= Successful.OK (makePage LorenzSystem.sigma LorenzSystem.beta LorenzSystem.rho)
       POST >>= url "/" >>= request (fun x -> Successful.OK (pageOfFormData (form x)))
       url_scan "/%f/%f/%f/lorenz.png" 
            (fun (sigma,beta,rho) -> 
                Writers.set_header "Content-Type" "image/png" 
                >>= Successful.ok(lorenzImage sigma beta rho))
    ]

let config = 
    { default_config with
       bindings = [ { scheme = HTTP ; ip = IPAddress.Parse "127.0.0.1" ; port   = 8082us } ]
    }

web_server config app
