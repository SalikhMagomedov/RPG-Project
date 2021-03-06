﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string SceneIndex = "lastSceneBuildIndex";

        public IEnumerator LoadLastScene(string saveFile)
        {
            var state = LoadFile(saveFile);
            var buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey(SceneIndex)) buildIndex = (int) state[SceneIndex];
            yield return SceneManager.LoadSceneAsync(buildIndex);

            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            var state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private void SaveFile(string saveFile, object state)
        {
            var path = GetPathFromSaveFile(saveFile);
            using (var stream = File.Open(path, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path)) return new Dictionary<string, object>();
            using (var stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private void CaptureState(IDictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
                state[saveable.UniqueIdentifier] = saveable.CaptureState();

            state[SceneIndex] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            foreach (var savable in FindObjectsOfType<SaveableEntity>())
            {
                var id = savable.UniqueIdentifier;
                if (state.ContainsKey(id)) savable.RestoreState(state[id]);
            }
        }

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");

        public void Delete(string defaultSaveFile)
        {
            File.Delete(GetPathFromSaveFile(defaultSaveFile));
        }
    }
}