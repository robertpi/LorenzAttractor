module ProgFs.CrossPlatform.LorenzSystem

let dt = 0.01

let sigma = 10. 
let beta = 8. / 3.
let rho = 28.0

let lorentzDeriv (sigma: float) beta rho x y z = failwith "To be implemented for Task Two"

let rec integrate (func: float -> float -> float -> (float * float * float)) dt (x, y, z) =
    seq { yield (x, y, z)
          let x', y', z' = func x y z
          let accumulate x x' = x + (dt * x') 
          let x'' = accumulate x x'
          let y'' = accumulate y y'
          let z'' = accumulate z z'
          yield! integrate func dt (x'', y'', z'') }




