open System.Windows.Forms
open System.Drawing
open ProgFs.CrossPlatform
open ProgFs.CrossPlatform.LorenzSystem

let input name left top value updateAction =
    let control = new Control(Width=550, Height=30, Left = left, Top = top)
    // TODO: Task Four. Set up the input control here

    control

let form =
    let form = new Form(Text = "Lorenz Attractor Viewer",
                        Width = 550,
                        Height = 660)

    // TODO: Task Four. Complete the population of the Form.

    form

[<EntryPoint>]
let main argv = 
    Application.Run form
    0 // return an integer exit code
