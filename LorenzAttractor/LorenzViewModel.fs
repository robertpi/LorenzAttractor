namespace ProgFs.CrossPlatform

type LorenzViewModel(width: int, height: int, ?sigma: float, ?beta: float, ?rho: float) =
    let mutable sigma = Option.getWithDefault LorenzSystem.sigma sigma 
    let mutable beta = Option.getWithDefault LorenzSystem.beta beta 
    let mutable rho = Option.getWithDefault LorenzSystem.rho rho

    let mutable points = 1000

    let updatedEvent = new Event<_>()

    let viewport = new Viewport3DBitmap(width, height)

    let doUpdate() =
        let lorentz = LorenzSystem.lorentzDeriv sigma beta rho
        let newPoints = 
            LorenzSystem.integrate lorentz LorenzSystem.dt (0.1, 0.1, 0.1)
            |> Seq.take points
        viewport.Points <- newPoints
        updatedEvent.Trigger()

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

