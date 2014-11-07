open System.Windows.Forms
open System.Drawing
open ProgFs.CrossPlatform
open ProgFs.CrossPlatform.LorenzSystem

let input name left top value updateAction =
    let control = new Control(Width=550, Height=30, Left = left, Top = top)
    let label = new Label(Text = name, Width = 190)
    let input = new TextBox(Text = string value, Width = 60, Left = 200)
    control.Controls.Add(label)
    control.Controls.Add(input)
    let doTextChange text =
        let success, res = System.Double.TryParse(text)
        if success then
            updateAction res
    input.TextChanged.Add(fun x -> doTextChange input.Text)
    control

type DoubleBuffered() =
    inherit Control()
    do base.DoubleBuffered <- true

let form =
    let form = new Form(Text = "Lorenz Attractor Viewer",
                                  Width = 550,
                                  Height = 660)

    let modelWidth = 500
    let modelHeight = 500

    let viewModel = new LorenzViewModel(modelWidth, modelHeight)


    let lorenzBitmap = new DoubleBuffered(Width = modelWidth, Height = modelHeight,
                                          Top = 10, Left = 20)
    lorenzBitmap.BackgroundImage <- viewModel.Bitmap


    let sigma = input "Sigma" 20 520 viewModel.Sigma (fun x -> viewModel.Sigma <- x)
    let beta = input "Beta" 20 560 viewModel.Beta (fun x -> viewModel.Beta <- x)
    let rho = input "Rho" 20 600 viewModel.Rho (fun x -> viewModel.Rho <- x)

    form.Controls.AddRange([| lorenzBitmap; sigma; beta; rho |])

    viewModel.Updated.Add(fun () -> lorenzBitmap.Invalidate())

    form

[<EntryPoint>]
let main argv = 
    Application.Run form
    0 // return an integer exit code
