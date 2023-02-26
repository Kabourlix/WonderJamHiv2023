using System;
using Game.Scripts.Enemies;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [MBTNode("Example/Set Patrol Point")]
    [AddComponentMenu("")]
    public class SetPatrolPoint : Leaf
    {
        public TransformReference nextPatrolPoint = new TransformReference(VarRefMode.DisableConstant);
        private Patroller _patroller;

        private void Start()
        {
            _patroller = GetComponent<MBTExecutorEnhanced>().Patroller;
        }

        public override NodeResult Execute()
        {
            nextPatrolPoint.Value = _patroller.GetNextWaypoint();
            return NodeResult.success;
        }
    }
}
