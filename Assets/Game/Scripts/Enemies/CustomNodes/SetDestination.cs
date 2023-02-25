using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Set destination")]
    public class SetDestination : Service
    {
        public TransformReference destination;
        public NavMeshAgent agent;
        public override void Task()
        {
            agent.isStopped = false;
            agent.SetDestination(destination.Value.position);
        }
    }
}