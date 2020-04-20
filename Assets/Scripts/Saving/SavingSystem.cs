using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            print($"Saving to {path}");
            using (var stream = File.Open(path, FileMode.Create))
            {
                var bytes = Encoding.UTF8.GetBytes("Hello World!");
                stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public void Load(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            print($"Load from {path}");
            using (var stream = File.Open(path, FileMode.Open))
            {
                var buffer = new byte[stream.Length];
                stream.ReadAsync(buffer, 0, buffer.Length);

                var loadedString = Encoding.UTF8.GetString(buffer);
                print(loadedString);
            }
        }

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}