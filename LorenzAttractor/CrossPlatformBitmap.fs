namespace ProgFs.CrossPlatform
open System.Drawing
#if ANDROID
open Android.Graphics
#endif

type CrossPlatformBitmap(width: int, height: int) =
    let bitmap = 
#if ANDROID
        Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888)
    let canvas = new Canvas(bitmap)
#else
        new Bitmap(width, height)
    let graphics = Graphics.FromImage(bitmap)
#endif

    member __.SetPixel(x, y, color) =
        bitmap.SetPixel(x, y, color)

    member __.Fill (c: Color) =
#if ANDROID
            canvas.DrawColor(c)
#else
            graphics.Clear(c)
#endif

    member __.NativeBitmap = bitmap
