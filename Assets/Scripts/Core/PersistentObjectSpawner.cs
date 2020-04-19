using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        private static bool _hasSpawned;

        [SerializeField] private GameObject persistentObjectPrefab;

        private void Awake()
        {
            if (_hasSpawned) return;

            SpawnPersistentObjects();
            _hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            var persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}