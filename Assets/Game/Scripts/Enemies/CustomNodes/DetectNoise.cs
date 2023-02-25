using System;
using MBT;
using UnityEngine;

namespace MBTExample
{
    [MBTNode("K/DetectNoise")]
    [AddComponentMenu("")]
    public class DetectNoise : Leaf
    {
        public LayerMask mask = -1;
        public float range = 5;
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        
        
        public override NodeResult Execute()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                variableToSet.Value = colliders[0].transform;
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.color = Color.white;
        }
    }
}