using System;
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

        public override NodeResult Execute()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, playerLayer, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                Vector3 direction = colliders[0].transform.position - transform.position;
                float angleToPlayer = Vector3.Angle(direction, transform.forward);
                if (angleToPlayer < angle * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 100, playerLayer))
                    {
                        if (hit.collider.gameObject == colliders[0].gameObject)
                        {
                            return NodeResult.success;
                        }
                    }
                }
            }
            return NodeResult.failure;
        }
    }
}