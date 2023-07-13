using Avalonia.Controls;
using ReactiveUI;
using System.Reactive;

namespace Notes.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> Command_Close { get; }

    private readonly Window MainWindow;

    public MainViewModel()
    {
        //this.MainWindow = MainWindow;

        Command_Close = ReactiveCommand.Create(() => { });
    }
}
