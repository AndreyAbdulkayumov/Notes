using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Notes.Views.MessageWindows
{
    public partial class MessageBoxView : Window
    {
        public MessageBoxView(string Message, string? Title)
        {
            InitializeComponent();

            TextBlock_Message.Text = Message;
            this.Title = Title;
        }

        private void Button_OK_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Close_Click(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

        public void Title_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            this.BeginMoveDrag(e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Close();
            }
        } 
    }
}
