using System;
using UnityEditor;
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

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying || string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");

            if (!string.IsNullOrEmpty(property.stringValue)) return;
            
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}