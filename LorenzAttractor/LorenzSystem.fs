module ProgFs.CrossPlatform.LorenzSystem

let dt = 0.01

let sigma = 10. 
let beta = 8. / 3.
let rho = 28.0

let lorentzDeriv (sigma: float) beta rho x y z =
     sigma * (y - x), 
     x * (rho - z) - y, 
     x * y - beta * z


let rec integrate (func: float -> float -> float -> (float * float * float)) dt (x, y, z) =
    seq { yield (x, y, z)
          let x', y', z' = func x y z
          let accumluate x x' = x + (dt * x') 
          let x'' = accumluate x x'
          let y'' = accumluate y y'
          let z'' = accumluate z z'
          yield! integrate func dt (x'', y'', z'') }




