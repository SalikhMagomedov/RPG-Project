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
                Gizmos.DrawSphere(GetWaypoint(i), WaypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(i + 1 < transform.childCount ? i + 1 : 0));
            }
        }

        private Vector3 GetWaypoint(int i) => transform.GetChild(i).position;
    }
}