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

        // TODO: Task Six. Complete the connection of the various controls.

        lorenzViewModel.Updated.Add(fun _ -> imageView.Invalidate())