namespace ProgFs.CrossPlatform

module Option =
    let getWithDefault x opt =
        match opt with Some x -> x | _ -> x
