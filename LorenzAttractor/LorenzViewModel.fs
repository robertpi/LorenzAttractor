namespace ProgFs.CrossPlatform
open System
open System.Collections.Generic
#if ANDROID
open Android.Graphics
#else
open System.Drawing
#endif
open System.Threading

type LorenzViewModel(width: int, height: int, ?sigma: float, ?beta: float, ?rho: float) =
    let mutable sigma = Option.getWithDefault LorenzSystem.sigma sigma 
    let mutable beta = Option.getWithDefault LorenzSystem.beta beta 
    let mutable rho = Option.getWithDefault LorenzSystem.rho rho

    let mutable points = 1000

    let updatedEvent = new Event<_>()

    let viewport = new Viewport3DBitmap(width, height)

    let lorentz = LorenzSystem.lorentzDeriv sigma beta rho

    let colors = [ Color.HotPink; Color.CornflowerBlue; Color.LightGoldenrodYellow]

    let rnd = new Random()
    
    let getWindowedEnumerator() =
        let windowedSeq =
            LorenzSystem.integrate lorentz LorenzSystem.dt (rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble())
            |> Seq.windowed 1000
        windowedSeq.GetEnumerator()

    let getListOfWindowedPoints() =
        colors 
        |> Seq.map (fun x -> x, getWindowedEnumerator()) 
        |> List.ofSeq

    let invertWindowedMatrix (windowed: seq<_*IEnumerator<_>>) =
        seq { while true do
               let nextPointSet =
                    windowed 
                    |> Seq.map (fun (color, x) -> 
                        x.MoveNext() |> ignore
                        color, x.Current :> seq<float*float*float>)
               yield nextPointSet }

    let startUpdateSequece() =
        let syncContext = SynchronizationContext.Current
        let sequenceOfWindowedPointsWithColors = invertWindowedMatrix (getListOfWindowedPoints())
        async { for points in sequenceOfWindowedPointsWithColors do
                    do! Async.SwitchToContext syncContext
                    do viewport.Points <- points
                    do updatedEvent.Trigger()
                    do! Async.Sleep 30 }
        |> Async.Start

    let doUpdate() =
        Async.CancelDefaultToken()
        startUpdateSequece()
    
    do doUpdate()

    member __.Updated = updatedEvent.Publish

    member __.Bitmap = viewport.Bitmap

    member __.Sigma
        with get() = sigma
        and set x = 
            sigma <- x
            doUpdate()

    member __.Beta
        with get() = beta
        and set x = 
            beta <- x
            doUpdate()

    member __.Rho
        with get() = rho
        and set x = 
            rho <- x
            doUpdate()

