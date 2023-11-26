using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using Avalonia.Platform;
using System.Linq;

namespace Notes.Views;

public partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; }

    public readonly Screen LastScreen;

    public event EventHandler<EventArgs>? Button_AddCommand;
    

    public MainWindow()
    {
        InitializeComponent();

        Instance = this;

        LastScreen = Screens.All.Last();
    }

    public void Border_Tools_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }

    public void Button_AddBlock_Click(object sender, RoutedEventArgs args)
    {
        Button_AddCommand?.Invoke(this, new EventArgs());
    }

    public void Button_Close_Click(object sender, RoutedEventArgs args)
    {
        this.Close();
    }
}
