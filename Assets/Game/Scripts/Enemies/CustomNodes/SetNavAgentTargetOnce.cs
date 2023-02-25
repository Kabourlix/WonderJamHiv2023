using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [MBTNode("K/SetNavOnce")]
    [AddComponentMenu("")]
    public class SetNavAgentTargetOnce : Leaf
    {
        public TransformReference targetPosition;
        [SerializeField] private NavMeshAgent agent;
        
        public override NodeResult Execute()
        {
            if(targetPosition != null)
            {
                var tf = targetPosition.Value;
                agent.SetDestination(tf.position);
                targetPosition = null;
                Destroy(tf.gameObject);
                
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }
    }
}