using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBTExample
{
    [MBTNode("K/DeathEvent")]
    [AddComponentMenu("")]
    public class DeathEvent : Leaf
    {
        public TransformReference transform;
        private PlayerController playerControl;

        public override NodeResult Execute()
        {
            playerControl = transform.Value.GetComponent<PlayerController>();
            if (playerControl == null) { Debug.Log("No PlayerComponent FOUND"); return NodeResult.failure; }
            if (playerControl.IsSuspect)
            {
                Debug.Log("VU");
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }

    }
}