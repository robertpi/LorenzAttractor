#load "LorenzSystem.fs"

open ProgFs.CrossPlatform

let x, y, z = LorenzSystem.lorentzDeriv LorenzSystem.sigma LorenzSystem.beta LorenzSystem.rho 0.1 0.1 0.1

// results should be:
// val z : float = -0.2566666667
// val y : float = 2.69
// val x : float = 0.0

let values =
    LorenzSystem.integrate 
        (LorenzSystem.lorentzDeriv LorenzSystem.sigma LorenzSystem.beta LorenzSystem.rho) 
        LorenzSystem.dt 
        (0.1, 0.1, 0.1)
    |> Seq.take 10
    |> Seq.toList

// results should be:
//val values : (float * float * float) list =
//  [(0.1, 0.1, 0.1); (0.1, 0.1269, 0.09743333333);
//   (0.10269, 0.1535335667, 0.09496201111);
//   (0.1077743567, 0.1806539145, 0.09258735443);
//   (0.1150623125, 0.2089244098, 0.09031305691);
//   (0.1244485222, 0.2389486969, 0.08814510198);
//   (0.1358985397, 0.2712951009, 0.08609193405);
//   (0.1494381958, 0.3065167433, 0.08416483522);
//   (0.1651460505, 0.3451684963, 0.08237849271);
//   (0.1831482951, 0.3878216606, 0.08075176504)]