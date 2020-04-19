using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";
        
        private SavingSystem _savingSystem;
        private Fader _fader;
        
        [SerializeField] private float fadeInTime = 2f;

        private void Awake()
        {
            _fader = FindObjectOfType<Fader>();
            _savingSystem = GetComponent<SavingSystem>();
        }

        private IEnumerator Start()
        {
            _fader.FadeOutImmediate();
            
            yield return _savingSystem.LoadLastScene(DefaultSaveFile);
            yield return _fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) Load();
            if (Input.GetKeyDown(KeyCode.S)) Save();
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFile);
        }
    }
}