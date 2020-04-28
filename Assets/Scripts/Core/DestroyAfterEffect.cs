using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy;
        
        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(targetToDestroy != null ? targetToDestroy : gameObject);
            }
        }
    }
}