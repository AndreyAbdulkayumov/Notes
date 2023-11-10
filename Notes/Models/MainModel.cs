using Notes.Views.MessageWindows;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class BlockData
    {
        public readonly int ID;

        public string? Title { get; set; }
        public string? Content { get; set; }

        public ReactiveCommand<Unit, Unit> Command_RemoveBlock { get; }


        public BlockData(int ID, ObservableCollection<BlockData> AllBlocks, Func<MessageBoxResult>? Message)
        {
            this.ID = ID;

            Command_RemoveBlock = ReactiveCommand.Create(() =>
            {
                if (Message?.Invoke() == MessageBoxResult.No)
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

        public Func<MessageBoxResult>? ViewWarningMessage { get; set; }

        private const string FilePath_SavedContent = "SavedContent.json";

        private Random RandomGenerator = new Random();

        public void AddBlock()
        {
            int NewID;

            do
            {
                NewID = RandomGenerator.Next();
            }
            while (AllBlocks.Where((element) => element.ID == NewID).Count() != 0);

            AllBlocks.Add(new BlockData(NewID, AllBlocks, ViewWarningMessage)
            {
                Title = "Title ",
                Content = "Content "
            });
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
            SettingsManager.Read(FilePath_SavedContent, out List<BlockData_ForSave>? SavedData);

            if (SavedData != null)
            {
                AllBlocks.Clear();

                foreach(var element in SavedData)
                {
                    AllBlocks.Add(
                        new BlockData(element.ID, AllBlocks, ViewWarningMessage)
                        {
                            Title = element.Title,
                            Content = element.Content
                        });
                }
            }
        }
    }
}
