using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
                var playerTransform = GetPlayerTransform();
                var formatter = new BinaryFormatter();
                var position = new SerializableVector3(playerTransform.position);
                formatter.Serialize(stream, position);
            }
        }

        private Transform GetPlayerTransform() => GameObject.FindWithTag("Player").transform;

        public void Load(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            print($"Load from {path}");
            using (var stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                var position = (SerializableVector3) formatter.Deserialize(stream);
                var playerTransform = GetPlayerTransform();
                playerTransform.position = position.ToVector3();
            }
        }

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}