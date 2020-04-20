using System;
using System.IO;
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
                var buffer = SerializeVector(playerTransform.position);
                stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        private Transform GetPlayerTransform() => GameObject.FindWithTag("Player").transform;

        public void Load(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            print($"Load from {path}");
            using (var stream = File.Open(path, FileMode.Open))
            {
                var buffer = new byte[stream.Length];
                stream.ReadAsync(buffer, 0, buffer.Length);

                var playerTransform = GetPlayerTransform();
                playerTransform.position = DeserializeVector(buffer);
            }
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            var vectorBytes = new byte[12];

            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);

            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] buffer) =>
            new Vector3
            {
                x = BitConverter.ToSingle(buffer, 0),
                y = BitConverter.ToSingle(buffer, 4),
                z = BitConverter.ToSingle(buffer, 8)
            };

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}