using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [MBTNode("K/SetNavOnceVector")]
    [AddComponentMenu("")]
    public class SetNavAgentTargetOnceVector : Leaf
    {
        public Vector3Reference targetPosition;
        [SerializeField] private NavMeshAgent agent;
        
        public override NodeResult Execute()
        {
            if(targetPosition.Value != Vector3.zero)
            {
                agent.SetDestination(targetPosition.Value);
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }
    }
}