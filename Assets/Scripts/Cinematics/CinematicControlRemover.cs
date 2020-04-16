using UnityEngine;
using UnityEngine.Playables;

namespace Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            print("DisableControl");
        }

        private void EnableControl(PlayableDirector pd)
        {
            print("EnableControl");
        }
    }
}