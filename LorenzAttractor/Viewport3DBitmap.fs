namespace ProgFs.CrossPlatform
open System.Drawing
#if ANDROID
open Android.Graphics
#endif

// This webpage provides a good description of how to map 3D points on to a 2D plane: 
// http://anthony.liekens.net/index.php/Computers/RenderingTutorial3DTo2D

// If you'd like to go further, the same site provides details of how to rotate objects:
// http://anthony.liekens.net/index.php/Computers/RenderingTutorialRotations

type Viewport3DBitmap(width: int, height: int) =
    let bitmap = 
#if ANDROID
        Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888)
#else
        new Bitmap(width, height)
    let g = Graphics.FromImage(bitmap)
#endif
    let xMax = float width
    let yMax = float height

    let mutable points = Seq.empty : seq<float*float*float>

    let distance = 35.
    let xOffSet = 0.5
    let yOffSet = 0.5

    let map3dTo2d (x3d, y3d, z3d) =
        let x2d = xOffSet + (x3d / (z3d + distance))
        let y2d = xOffSet + (y3d / (z3d + distance))
        x2d, y2d


    let setPoint x y color =
        let x' = int (x * xMax)
        let y' = int (y * yMax)
        if 0 <= x' && x' < (int xMax) &&
           0 <= y' && y' < (int yMax) then
            bitmap.SetPixel(x', y', color)

    member __.Points 
        with set newPoints =
            points <- newPoints
#if ANDROID
            // TODO clearing the bitmap this way, is very slow, use Android drawing api?
            for x in 0 .. width - 1 do
                for y in 0 .. height - 1 do
                    bitmap.SetPixel(x, y, Color.Black)
#else
            g.Clear(Color.Black)
#endif
            for (x, y, z) in points do
                let x', y' = map3dTo2d (x, y, z)
                setPoint x' y' Color.White
        and get() = points

    member __.Bitmap = bitmap
