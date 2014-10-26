namespace LorenzAttractorAndroid

open System
open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget
open ProgFs.CrossPlatform

[<Activity(Label = "LorenzAttractorAndroid", MainLauncher = true)>]
type MainActivity() = 
    inherit Activity()

    // TODO detect size of screen and adjust view model size
    let lorenzViewModel = new LorenzViewModel(300, 300)


    let input (input: EditText) value updateAction =
        let doTextChange text =
            let success, res = System.Double.TryParse(text)
            if success then
                updateAction res
        input.Text <- string value
        input.TextChanged.Add(fun x -> doTextChange input.Text)

    override this.OnCreate(bundle) = 
        base.OnCreate(bundle)
        // Set our view from the "main" layout resource
        this.SetContentView(Resource_Layout.Main)
        let imageView = this.FindViewById<ImageView>(Resource_Id.imageView)
        imageView.SetImageBitmap(lorenzViewModel.Bitmap)

        let sigma = this.FindViewById<EditText>(Resource_Id.sigma)
        input sigma lorenzViewModel.Sigma (fun x -> lorenzViewModel.Sigma <- x)
        let beta = this.FindViewById<EditText>(Resource_Id.beta)
        input beta lorenzViewModel.Beta (fun x -> lorenzViewModel.Beta <- x)
        let sigma = this.FindViewById<EditText>(Resource_Id.rho)
        input sigma lorenzViewModel.Rho (fun x -> lorenzViewModel.Rho <- x)

        lorenzViewModel.Updated.Add(fun _ -> imageView.Invalidate())