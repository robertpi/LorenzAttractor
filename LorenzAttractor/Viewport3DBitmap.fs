namespace ProgFs.CrossPlatform
open ProgFs.CrossPlatform
#if ANDROID
open Android.Graphics
#else
open System.Drawing
#endif

// This webpage provides a good description of how to map 3D points on to a 2D plane: 
// http://anthony.liekens.net/index.php/Computers/RenderingTutorial3DTo2D

// If you'd like to go further, the same site provides details of how to rotate objects:
// http://anthony.liekens.net/index.php/Computers/RenderingTutorialRotations

type Viewport3DBitmap(width: int, height: int) =
    let bitmap = new CrossPlatformBitmap(width, height)
    let xMax = float width
    let yMax = float height

    let mutable points = Seq.empty : seq<Color*seq<float*float*float>>

    let distance = 35.
    let xOffSet = 0.5
    let yOffSet = 0.5

    let map3dTo2d (x3d, y3d, z3d) =
        let x2d = xOffSet + (x3d / (z3d + distance))
        let y2d = yOffSet + (y3d / (z3d + distance))
        x2d, y2d


    let setPoint x y color =
        let x' = int (x * xMax)
        let y' = int yMax - int (y * yMax)
        if 0 <= x' && x' < (int xMax) &&
           0 <= y' && y' < (int yMax) then
            bitmap.SetPixel(x', y', color)

    member __.Points 
        with set newPoints =
            points <- newPoints
            bitmap.Fill(Color.Black)
            for (color, points) in points do
                for x, y, z in points do
                let x', y' = map3dTo2d (x, y, z)
                setPoint x' y' color
        and get() = points

    member __.Bitmap = bitmap.NativeBitmap
