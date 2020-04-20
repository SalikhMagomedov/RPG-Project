using System.IO;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            print($"Saving to {GetPathFromSaveFile(saveFile)}");
        }

        public void Load(string saveFile)
        {
            print($"Load from {GetPathFromSaveFile(saveFile)}");
        }

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}