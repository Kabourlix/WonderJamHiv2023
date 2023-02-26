using System;
using Game.Scripts.Enemies;
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

        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<MBTExecutorEnhanced>().Agent;
        }

        public override NodeResult Execute()
        {
            if(targetPosition != null)
            {
                var tf = targetPosition.Value;
                _agent.SetDestination(tf.position);
                
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }
    }
}