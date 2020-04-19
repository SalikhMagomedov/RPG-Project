using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal() => FindObjectsOfType<Portal>()
            .FirstOrDefault(portal => portal != this && portal.destination == destination);

        private enum DestinationIdentifier
        {
            A,
            B,
            C,
            D,
            E
        }
    }
}