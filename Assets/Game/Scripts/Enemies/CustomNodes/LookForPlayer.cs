using System;
using Game.Scripts.Utility;
using MBT;
using UnityEngine;

namespace MBTExample
{
    [MBTNode("K/LookForPlayer")]
    [AddComponentMenu("")]
    public class LookForPlayer : Leaf
    {

        public LayerMask playerLayer;
        [Min(0)]
        public float range = 5;
        
        [Range(0,180f)]
        public float angle = 45;
        public TransformReference playerSeen;

        public override NodeResult Execute()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, playerLayer);
            Debug.Log(colliders.Length);
            if (colliders.Length > 0)
            {
                Vector2 direction = colliders[0].transform.position.xz() - transform.position.xz();
                Debug.DrawLine(transform.position, colliders[0].transform.position, Color.red);
                float angleToPlayer = Vector2.Angle(direction, transform.forward.xz());
                if (angleToPlayer < angle * 0.5f)
                {
                    Debug.Log($"{colliders[0].name} in sight");
                    var dir3 = new Vector3(direction.x, 0, direction.y);
                    if (Physics.Raycast(transform.position, range*dir3.normalized, out var hit, range, playerLayer))
                    {
                        if (hit.collider.gameObject == colliders[0].gameObject)
                        {
                            Debug.Log("VU");
                            playerSeen.Value = hit.collider.gameObject.transform;
                            return NodeResult.success;
                        }
                    }
                }
            }
            playerSeen.Value = null;
            return NodeResult.failure;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward * range);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle * 0.5f, 0) * transform.forward * range);
        }
    }
}