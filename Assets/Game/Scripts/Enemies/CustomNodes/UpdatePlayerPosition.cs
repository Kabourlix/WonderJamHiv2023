using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBTExample
{
    [MBTNode("K/Update player position")]
    [AddComponentMenu("")]
    public class UpdatePlayerPosition : Service
    {
        public TransformReference transformPlayer;
        public override void Task()
        {
            transformPlayer.Value = GameManager.Instance.Player;
        }
    }
}