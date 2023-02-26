using System;
using Game.Scripts.Enemies;
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
        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<MBTExecutorEnhanced>().Agent;
        }

        public override NodeResult Execute()
        {
            if(targetPosition.Value != Vector3.zero)
            {
                _agent.SetDestination(targetPosition.Value);
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }
    }
}