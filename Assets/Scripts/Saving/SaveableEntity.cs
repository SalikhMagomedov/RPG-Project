using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        
        private NavMeshAgent _navMeshAgent;
        private ActionScheduler _actionScheduler;

        public string UniqueIdentifier => uniqueIdentifier;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public object CaptureState() => new SerializableVector3(transform.position);

        public void RestoreState(object state)
        {
            _navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3) state).ToVector3();
            _navMeshAgent.enabled = true;
            _actionScheduler.CancelCurrentAction();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying || string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");

            if (!string.IsNullOrEmpty(property.stringValue)) return;
            
            property.stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}