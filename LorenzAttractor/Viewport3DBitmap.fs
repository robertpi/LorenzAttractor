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

    let mutable points = Seq.empty : seq<float*float*float>

    let distance = 35.
    let xOffSet = 0.5
    let yOffSet = 0.5

    let map3dTo2d (x3d, y3d, z3d) = failwith "To be implemeneted for Task Two"

    let setPoint x y color = failwith "To be implemeneted for Task Two"

    member __.Points 
        with set newPoints =
            points <- newPoints
            bitmap.Fill(Color.Black)
            for (x, y, z) in points do
                let x', y' = map3dTo2d (x, y, z)
                setPoint x' y' Color.White
        and get() = points

    member __.Bitmap = bitmap.NativeBitmap
