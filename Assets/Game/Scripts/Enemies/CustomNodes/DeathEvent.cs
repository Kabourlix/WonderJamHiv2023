using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace MBTExample
{
    [MBTNode("K/DeathEvent")]
    [AddComponentMenu("")]
    public class DeathEvent : Leaf
    {
        private PlayerController playerControl;
        private void Start()
        {
            playerControl = GameManager.Instance.Player.GetComponent<PlayerController>();
        }
        public override NodeResult Execute()
        {
            if (playerControl == null) { throw new System.Exception("No playerController found"); }
            if (playerControl.IsSuspect)
            {
                GameManager.Instance.ChangeState(GameState.CaughtState);
                //Debug.Log("<color=red>Player has been caught</color>");
                return NodeResult.success;
            }
            else
            {
                return NodeResult.failure;
            }
        }

    }
}