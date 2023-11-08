using Avalonia;
using Avalonia.Controls;
using Notes.Models;
using Notes.Views;
using Notes.Views.MessageWindows;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Notes.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<BlockData> Blocks
    {
        get => Model.AllBlocks;
        set => this.RaiseAndSetIfChanged(ref Model.AllBlocks, value);
    }

    private const string FilePath_SavedText = "SavedText.txt";
    private const string FilePath_Settings = "Settings.json";

    private MainModel Model = MainModel.Instance;


    public MainViewModel()
    {
        if (MainWindow.Instance != null)
        {            
            MainWindow.Instance.Opened += Instance_Opened;
            MainWindow.Instance.Closing += Instance_Closing;

            MainWindow.Instance.Button_AddCommand += Instance_Button_AddCommand_Handler;
        }


        //Blocks.Add(new BlockData(1, Blocks)
        //{
        //    Title = "Title 1",
        //    Content = "Content 1",
        //});

        //Blocks.Add(new BlockData(2, Blocks)
        //{
        //    Title = "Title 2",
        //    Content = "Content 2"
        //});
    }


    private void Instance_Opened(object? sender, EventArgs e)
    {
        try
        {
            if (File.Exists(FilePath_SavedText) == false)
            {
                File.Create(FilePath_SavedText).Close();
            }

            //TextBox_Text.Text = File.ReadAllText(FilePath_SavedText);

            bool SettingsFile_Exists = true;

            if (File.Exists(FilePath_Settings) == false)
            {
                SettingsFile_Exists = false;
            }

            MainWindowState State = SettingsManager.Read(FilePath_Settings);

            // Если файла настроек не обнаружено, то приложение откроектся в центре экрана
            if (SettingsFile_Exists)
            {
                double GlobalWidth = MainWindow.Instance.LastScreen.Bounds.BottomRight.X;
                double GlobalHeight = MainWindow.Instance.Bounds.BottomRight.Y;

                // Если сохраненная позиция окна находится вне рамок доступной рабочей области,
                // то приложение откроется в центре главного экрана.
                if (State.Left < GlobalWidth && State.Top < GlobalHeight)
                {
                    MainWindow.Instance.Position = new PixelPoint(State.Left, State.Top);
                }
            }

            Model.RestoreContent();
        }

        catch (Exception error)
        {
            MessageBox.Show(MainWindow.Instance, "Ошибка инциализации приложения.\n\n" + error.Message, "Ошибка");
        }
    }

    private void Instance_Closing(object? sender, WindowClosingEventArgs e)
    {
        SaveData();
    }

    private void Instance_Button_AddCommand_Handler(object? sender, EventArgs e)
    {
        Model.AddBlock();
    }

    private void SaveData()
    {
        try
        {
            MainWindowState State = new MainWindowState()
            {
                Left = MainWindow.Instance.Position.X,
                Top = MainWindow.Instance.Position.Y,
                Height = MainWindow.Instance.Height,
                Width = MainWindow.Instance.Width
            };

            SettingsManager.Save(FilePath_Settings, State);

            Model.SaveContent();
        }

        catch (Exception error)
        {
            MessageBox.Show(MainWindow.Instance, "Ошибка закрытия приложения.\n\n" + error.Message, "Ошибка");
        }
    }
}
