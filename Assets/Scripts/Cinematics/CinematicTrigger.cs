using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _alreadyTriggered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _alreadyTriggered) return;

            GetComponent<PlayableDirector>().Play();
            _alreadyTriggered = true;
        }
    }
}