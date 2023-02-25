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
        public Vector3Reference variableToSet = new Vector3Reference(VarRefMode.DisableConstant);
        
        
        public override NodeResult Execute()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask);
            if (colliders.Length > 0)
            {
                variableToSet.Value = new Vector3(colliders[0].transform.position.x, colliders[0].transform.position.y, colliders[0].transform.position.z);
                Debug.Log(colliders[0].name);
                Destroy(colliders[0].gameObject);
                return NodeResult.success;

            }
            else
            {
                variableToSet.Value = Vector3.zero;
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