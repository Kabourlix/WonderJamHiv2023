using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [MBTNode("K/NavMoveTowards")]
    [AddComponentMenu("")]
    public class NavMoveTowards : Leaf
    {
        public Vector3Reference targetPosition;
        [SerializeField] private NavMeshAgent agent;
        
        public override NodeResult Execute()
        {
            Vector3 target = targetPosition.Value;
            // Move as long as distance is greater than min. distance
            float dist = Vector3.Distance(target, agent.transform.position);
            if (dist > agent.stoppingDistance)
            {
                // Move towards target
                agent.SetDestination(target);
                return NodeResult.running;
            }
            else
            {
                return NodeResult.success;
            }
        }
    }
}