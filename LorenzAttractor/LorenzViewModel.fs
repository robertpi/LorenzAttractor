namespace ProgFs.CrossPlatform

type LorenzViewModel(width: int, height: int, ?sigma: float, ?beta: float, ?rho: float) =
    let mutable sigma = Option.getWithDefault LorenzSystem.sigma sigma 
    let mutable beta = Option.getWithDefault LorenzSystem.beta beta 
    let mutable rho = Option.getWithDefault LorenzSystem.rho rho

    let mutable points = 1000

    let updatedEvent = new Event<_>()

    let viewport = new Viewport3DBitmap(width, height)

    let doUpdate() = failwith "To be implemented for Task Three"

    do doUpdate()

    member __.Updated = failwith "To be implemented for Task Three"

    member __.Bitmap = viewport.Bitmap

    member __.Sigma
        with get() = failwith "To be implemented for Task Three"
        and set x = failwith "To be implemented for Task Three"

    member __.Beta
        with get() = failwith "To be implemented for Task Three"
        and set x = failwith "To be implemented for Task Three"

    member __.Rho
        with get() = failwith "To be implemented for Task Three"
        and set x = failwith "To be implemented for Task Three"

