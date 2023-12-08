using Notes.Views;
using Notes.Views.MessageWindows;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class BlockData
    {
        public readonly int ID;

        public string? Title { get; set; }
        public string? Content { get; set; }

        public ReactiveCommand<Unit, Task> Command_RemoveBlock { get; }


        public BlockData(int ID, ObservableCollection<BlockData> AllBlocks)
        {
            this.ID = ID;

            Command_RemoveBlock = ReactiveCommand.Create(async () =>
            {
                MessageBoxResult Result = await MessageBox.ShowYesNo(MainWindow.Instance, "Удалить данный столбец?", "Сообщение");

                if (Result != MessageBoxResult.Yes)
                {
                    return;
                }

                AllBlocks.Remove(this);
            });
        }
    }

    public class BlockData_ForSave
    {
        public int ID { get; set; }

        public string? Title { get; set; } 
        public string? Content { get; set; }

        public static BlockData_ForSave GetDefault()
        {
            return new BlockData_ForSave()
            {
                ID = 1,
                Title = null,
                Content = null
            };
        }
    }

    internal class MainModel
    {
        public ObservableCollection<BlockData> AllBlocks = new ObservableCollection<BlockData>();

        private static MainModel? _instance;

        public static MainModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainModel();

                return _instance;
            }
        }

        // Имя корневой папки (имя разработчика ПО)
        private const string CommonFolderName = "XSoft";

        // Имя папки приложения
        private const string ProgramFolderName = "Notes";

        // Имена служебных файлов
        private const string FileName_Settings = "Settings.json";
        private const string FileName_SavedContent = "SavedContent.json";

        private readonly string DirectoryNameInDocuments;

        private readonly string FilePath_Settings;

        private readonly string FilePath_SavedContent;

        private Random RandomGenerator = new Random();


        public MainModel()
        {
            DirectoryNameInDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), CommonFolderName, ProgramFolderName);

            FilePath_Settings = Path.Combine(DirectoryNameInDocuments, FileName_Settings);
            FilePath_SavedContent = Path.Combine(DirectoryNameInDocuments, FileName_SavedContent);
        }

        public void AddBlock()
        {
            int NewID;

            do
            {
                NewID = RandomGenerator.Next();
            }
            while (AllBlocks.Where((element) => element.ID == NewID).Count() != 0);           

            AllBlocks.Add(new BlockData(NewID, AllBlocks)
            {
                Title = null,
                Content = null
            });
        }

        public void CheckDirectory()
        {
            if (Directory.Exists(DirectoryNameInDocuments) == false)
            {
                Directory.CreateDirectory(DirectoryNameInDocuments);
            }
        }

        public void SaveSettings(MainWindowState State)
        {
            SettingsManager.Save(FilePath_Settings, State);
        }

        public bool RestoreSettings(out MainWindowState State)
        {
            bool SettingsFile_Exists = File.Exists(FilePath_Settings);

            State = SettingsManager.Read(FilePath_Settings);

            return SettingsFile_Exists;
        }

        public void SaveContent()
        {
            List<BlockData_ForSave> SaveData = new List<BlockData_ForSave>();

            foreach (var element in AllBlocks)
            {
                SaveData.Add(
                    new BlockData_ForSave() 
                    { 
                        ID = element.ID,
                        Title = element.Title, 
                        Content = element.Content 
                    });
            }

            SettingsManager.Save(FilePath_SavedContent, SaveData);
        }

        public void RestoreContent()
        {
            if (File.Exists(FilePath_SavedContent) == false)
            {
                SettingsManager.Save(FilePath_SavedContent, 
                    new List<BlockData_ForSave>() { BlockData_ForSave.GetDefault() });
            }

            SettingsManager.Read(FilePath_SavedContent, out List<BlockData_ForSave>? SavedData);

            if (SavedData != null)
            {
                AllBlocks.Clear();

                foreach(var element in SavedData)
                {
                    AllBlocks.Add(
                        new BlockData(element.ID, AllBlocks)
                        {
                            Title = element.Title,
                            Content = element.Content
                        });
                }
            }
        }
    }
}
