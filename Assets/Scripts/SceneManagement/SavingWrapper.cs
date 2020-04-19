using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";
        
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
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