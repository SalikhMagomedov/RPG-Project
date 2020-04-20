using UnityEngine;

namespace RPG.Saving
{
    public class SaveableEntity : MonoBehaviour
    {
        public string UniqueIdentifier => "";

        public object CaptureState() => null;

        public void RestoreState(object state)
        {
        }
    }
}