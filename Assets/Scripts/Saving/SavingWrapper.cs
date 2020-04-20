using UnityEngine;

namespace RPG.Saving
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
            if (Input.GetKeyDown(KeyCode.S)) _savingSystem.Save(DefaultSaveFile);
            if (Input.GetKeyDown(KeyCode.L)) _savingSystem.Load(DefaultSaveFile);
        }
    }
}