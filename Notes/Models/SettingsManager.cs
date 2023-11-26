using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class MainWindowState
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }

        public static MainWindowState CreateDefault()
        {
            return new MainWindowState
            {
                Left = 0,
                Top = 0,
                Width = 300,
                Height = 200
            };
        }
    }

    internal static class SettingsManager
    {
        public static void Save<T>(string FilePath, T Data)
        {
            if (File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, string.Empty);
            }            

            JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            using (FileStream Stream = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(Stream, Data, JsonOptions);
            }
        }

        public static MainWindowState Read(string FilePath)
        {
            try
            {
                MainWindowState? Data;

                if (File.Exists(FilePath) == false)
                {
                    File.Create(FilePath).Close();
                }

                using (FileStream Stream = new FileStream(FilePath, FileMode.Open))
                {
                    Data = JsonSerializer.Deserialize<MainWindowState>(Stream);
                }

                if (Data == null)
                {
                    return MainWindowState.CreateDefault();
                }

                return Data;
            }

            catch (JsonException)
            {
                return MainWindowState.CreateDefault();
            }
        }

        public static void Read(string FilePath, out List<BlockData_ForSave>? Data)
        {
            using (FileStream Stream = new FileStream(FilePath, FileMode.Open))
            {
                Data = JsonSerializer.Deserialize<List<BlockData_ForSave>>(Stream);
            }
        }
    }
}
