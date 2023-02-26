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
        public Patroller patroller;

        public override NodeResult Execute()
        {
            nextPatrolPoint.Value = patroller.GetNextWaypoint();
            return NodeResult.success;
        }
    }
}
