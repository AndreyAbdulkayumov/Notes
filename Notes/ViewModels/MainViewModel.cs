using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Interactivity;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Notes.ViewModels;

public class BlockData
{
    public string? Title { get; set; }
    public string? Content { get; set; }

    public ReactiveCommand<Unit, Unit> Command_RemoveBlock { get; }


    public BlockData(ObservableCollection<BlockData> AllBlocks)
    {
        Command_RemoveBlock = ReactiveCommand.Create(() =>
        {
            AllBlocks.Remove(this);
        });
    }
}

public class MainViewModel : ViewModelBase
{
    private ObservableCollection<BlockData> _blocks = new ObservableCollection<BlockData>();

    public ObservableCollection<BlockData> Blocks
    {
        get => _blocks;
        set => this.RaiseAndSetIfChanged(ref _blocks, value);
    }

    public ReactiveCommand<Unit, Unit> Command_AddBlock { get; }

    private int Counter = 2;


    public MainViewModel()
    {
        Command_AddBlock = ReactiveCommand.Create(() =>
        {
            Counter++;

            Blocks.Add(new BlockData(Blocks) 
            {
                Title = "Title " + Counter.ToString(),
                Content = "Content " + Counter.ToString()
            });
        });

        Blocks.Add(new BlockData(Blocks)
        {
            Title = "Title 1",
            Content = "Content 1",            
        });

        Blocks.Add(new BlockData(Blocks) 
        {
            Title = "Title 2",
            Content = "Content 2"
        });
    }    
}
