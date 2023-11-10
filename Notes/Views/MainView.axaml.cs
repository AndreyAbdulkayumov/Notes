using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Notes.Views
{
    public partial class MainView : UserControl
    {
        public event EventHandler<EventArgs> AnyTextBox_LostFocus;

        public static MainView Instance { get; private set; }

        public MainView()
        {
            InitializeComponent();

            Instance = this;
        }

        public void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AnyTextBox_LostFocus?.Invoke(this, new EventArgs());
        }
    }
}
