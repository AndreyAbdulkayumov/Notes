using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Notes.Settings;
using System;
using System.IO;
using Avalonia.Platform;
using System.Linq;

namespace Notes.Views;

public partial class MainWindow : Window
{
    private const string FilePath_SavedText = "SavedText.txt";

    private const string FilePath_Settings = "Settings.json";


    public MainWindow()
    {
        InitializeComponent();
    }

    public void MainWindow_Opened(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(FilePath_SavedText) == false)
            {
                File.Create(FilePath_SavedText).Close();
            }

            TextBox_Text.Text = File.ReadAllText(FilePath_SavedText);

            bool SettingsFile_Exists = true;

            if (File.Exists(FilePath_Settings) == false)
            {
                SettingsFile_Exists = false;
            }

            MainWindowState State = SettingsManager.Read(FilePath_Settings);

            // Если файла настроек не обнаружено, то приложение откроектся в центре экрана
            if (SettingsFile_Exists)
            {
                Screen LastScreen = Screens.All.Last();

                double GlobalWidth = LastScreen.Bounds.BottomRight.X;
                double GlobalHeight = LastScreen.Bounds.BottomRight.Y;

                // Если сохраненная позиция окна находится вне рамок доступной рабочей области,
                // то приложение откроется в центре главного экрана.
                if (State.Left < GlobalWidth && State.Top < GlobalHeight)
                {
                    this.Position = new PixelPoint(State.Left, State.Top);
                }                
            }

            this.Width = State.Width;
            this.Height = State.Height;

            this.ClientSize = new Size(this.Width, this.Height);
        }

        catch (Exception error)
        {
            MessageBox window = new MessageBox("Ошибка инциализации приложения.\n\n" + error.Message, "Ошибка");

            window.ShowDialog(this);
        }
    }

    private void MainWindow_Closing(object sender, WindowClosingEventArgs e)
    {
        SaveData();
    }

    private void SaveData()
    {
        try
        {
            if (File.Exists(FilePath_SavedText) == false)
            {
                File.Create(FilePath_SavedText).Close();
            }

            File.WriteAllText(FilePath_SavedText, String.Empty);

            File.WriteAllText(FilePath_SavedText, TextBox_Text.Text);

            MainWindowState State = new MainWindowState()
            {
                Left = this.Position.X,
                Top = this.Position.Y,
                Height = this.Height,
                Width = this.Width
            };

            SettingsManager.Save(FilePath_Settings, State);
        }

        catch (Exception error)
        {
            MessageBox window = new MessageBox("Ошибка закрытия приложения.\n\n" + error.Message, "Ошибка");

            window.ShowDialog(this);
        }
    }

    public void TextBox_Text_LostFocus(object sender, RoutedEventArgs e)
    {
        SaveData();
    }

    public void Border_Tools_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }

    public void Button_Close_Click(object sender, RoutedEventArgs args)
    {
        this.Close();
    }
}
