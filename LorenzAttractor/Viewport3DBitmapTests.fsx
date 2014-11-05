#load "CrossPlatformBitmap.fs"
#load "Viewport3DBitmap.fs"

open System.Drawing
open System.Windows.Forms
open ProgFs.CrossPlatform


let cube =
    [ 0.9, 0.9, 0.9
      0.9, 0.9, -0.9
      0.9, -0.9, -0.9
      0.9, -0.9, 0.9
      -0.9, 0.9, -0.9
      -0.9, -0.9, -0.9
      -0.9, -0.9, 0.9
      -0.9, 0.9, 0.9 ]
 
let cube1 =
    cube |> List.map (fun (x, y, z) -> x * 10., y * 10., z * 10.)
let cube2 =
    cube |> List.map (fun (x, y, z) -> (x * 4.) + 7., (y * 3.) - 1., z * 3.)

let form =
    let form = new Form()
    let xMax = float form.ClientSize.Width
    let yMax = float form.ClientSize.Height
    let bmp = new Viewport3DBitmap(int xMax, int yMax)
    bmp.Points <- [ yield! cube1; yield! cube2 ]
    form.BackgroundImage <- bmp.Bitmap
    form

form.Show()
 
