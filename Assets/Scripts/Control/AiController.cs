using UnityEngine;

namespace Control
{
    public class AiController : MonoBehaviour
    {
        private GameObject _player;

        [SerializeField] private float chaseDistance = 5f;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (DistanceToPlayer() <= chaseDistance) print($"{gameObject.name} must chase");
        }

        private float DistanceToPlayer() => Vector3.Distance(_player.transform.position, transform.position);
    }
}