using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Detect Enemy GameObject Service")]
    public class DetectEnemyGameObjectService : Service
    {
        public LayerMask mask = -1;
        [Tooltip("Sphere radius")]
        public float range = 5;
        public GameObjectReference variableToSet = new GameObjectReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                //Debug.Log("ATTACK");
                variableToSet.Value = colliders[0].gameObject;
            }
            else
            {
                variableToSet.Value = null;
            }
        }
    }
}