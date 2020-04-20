using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

        public string UniqueIdentifier => uniqueIdentifier;

        public object CaptureState() => null;

        public void RestoreState(object state)
        {
        }

        private void Update()
        {
            if (Application.isPlaying) return;
            
            print("Editing;");
        }
    }
}