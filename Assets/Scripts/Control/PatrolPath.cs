using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WaypointGizmoRadius = .3f;

        private void OnDrawGizmos()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), WaypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int i) => i + 1 < transform.childCount ? i + 1 : 0;

        public Vector3 GetWaypoint(int i) => transform.GetChild(i).position;
    }
}