using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";

        private SavingSystem _savingSystem;
        
        [SerializeField] private float fadeInTime = .2f;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();

            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return _savingSystem.LoadLastScene(DefaultSaveFile);
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) Save();
            if (Input.GetKeyDown(KeyCode.L)) Load();
            if (Input.GetKeyDown(KeyCode.Delete)) Delete();
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFile);
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }

        public void Delete()
        {
            _savingSystem.Delete(DefaultSaveFile);
        }
    }
}