namespace ProgFs.CrossPlatform

/// Extensions to the F# Option module
module Option =
    
    /// gets the value from an option, returns the default value if the option is empty
    let getWithDefault defaultValue opt =
        match opt with Some x -> x | _ -> defaultValue
