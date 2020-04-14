using UnityEngine;

namespace Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WaypointGizmoRadius = .3f;

        private void OnDrawGizmos()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, WaypointGizmoRadius);
            }
        }
    }
}