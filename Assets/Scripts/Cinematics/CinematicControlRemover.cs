using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject _player;
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            
            _player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            _playableDirector.played -= DisableControl;
            _playableDirector.stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}