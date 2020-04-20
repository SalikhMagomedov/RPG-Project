using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            SaveFile(saveFile, CaptureState());
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private void SaveFile(string saveFile, object state)
        {
            var path = GetPathFromSaveFile(saveFile);
            print($"Saving to {path}");
            using (var stream = File.Open(path, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            print($"Load from {path}");
            using (var stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private Dictionary<string, object> CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.UniqueIdentifier] = saveable.CaptureState();
            }
            return state;
        }

        private void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            foreach (var savable in FindObjectsOfType<SaveableEntity>())
            {
                savable.RestoreState(state[savable.UniqueIdentifier]);
            }
        }

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}