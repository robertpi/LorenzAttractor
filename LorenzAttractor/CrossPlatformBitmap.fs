namespace ProgFs.CrossPlatform
open System.Drawing
#if ANDROID
open Android.Graphics
#endif

/// Provides an abstraction between the bitmap in the .NET framework (System.Drawing.Bitmap) 
/// and the bitmap available on the Android platform (Android.Graphics.Bitmap)
type CrossPlatformBitmap(width: int, height: int) =

#if ANDROID
    let bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888)
    let canvas = new Canvas(bitmap)
#else
    let bitmap = new Bitmap(width, height)
    let graphics = Graphics.FromImage(bitmap)
#endif

    /// sets a pixel on the bitmap
    member __.SetPixel(x, y, color) =
        bitmap.SetPixel(x, y, color)

    /// fills the bitmap one color
    member __.Fill (c: Color) =
#if ANDROID
            canvas.DrawColor(c)
#else
            graphics.Clear(c)
#endif

    /// retrive the underlying bitmap so it can be used by drawing libraries
    member __.NativeBitmap = bitmap
