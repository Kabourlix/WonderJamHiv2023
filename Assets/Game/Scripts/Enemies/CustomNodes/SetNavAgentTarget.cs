using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [MBTNode("K/SetNav")]
    [AddComponentMenu("")]
    public class SetNavAgentTarget : Leaf
    {
        public TransformReference targetPosition;
        [SerializeField] private NavMeshAgent agent;
        
        public override NodeResult Execute()
        {
            if(targetPosition != null)
            {
                var tf = targetPosition.Value;
                agent.SetDestination(tf.position);
                
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }
    }
}