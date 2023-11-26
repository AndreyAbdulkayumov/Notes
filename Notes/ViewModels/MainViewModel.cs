using Avalonia;
using Avalonia.Controls;
using Notes.Models;
using Notes.Views;
using Notes.Views.MessageWindows;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace Notes.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<BlockData> Blocks
    {
        get => Model.AllBlocks;
        set => this.RaiseAndSetIfChanged(ref Model.AllBlocks, value);
    }  

    private MainModel Model = MainModel.Instance;


    public MainViewModel()
    {
        if (MainWindow.Instance != null)
        {            
            MainWindow.Instance.Opened += Instance_Opened;
            MainWindow.Instance.Closing += Instance_Closing;

            MainWindow.Instance.Button_AddCommand += Instance_Button_AddCommand_Handler;
        }

        if (MainView.Instance != null)
        {
            MainView.Instance.AnyTextBox_LostFocus += Instance_AnyTextBox_LostFocus;
        }        
    }

    private void Instance_AnyTextBox_LostFocus(object? sender, EventArgs e)
    {
        SaveData();
    }

    private void Instance_Opened(object? sender, EventArgs e)
    {
        try
        {
            Model.CheckDirectory();

            // Если файла настроек не обнаружено (если метод возвращает false), то приложение откроектся в центре экрана
            if (Model.RestoreSettings(out MainWindowState State))
            {
                // Предполагается, что все мониторы одинаковой высоты и расположены горизонтально
                double GlobalWidth = 0;

                foreach (var screen in MainWindow.Instance.Screens.All)
                {
                    GlobalWidth += screen.Bounds.Size.Width;
                }

                double GlobalHeight = MainWindow.Instance.LastScreen.Bounds.BottomRight.Y;

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
            Model.SaveSettings(new MainWindowState()
            {
                Left = MainWindow.Instance.Position.X,
                Top = MainWindow.Instance.Position.Y
            });

            Model.SaveContent();
        }

        catch (Exception error)
        {
            MessageBox.Show(MainWindow.Instance, "Ошибка закрытия приложения.\n\n" + error.Message, "Ошибка");
        }
    }
}
