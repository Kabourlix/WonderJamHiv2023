using Game.Scripts.Utility;
using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MBT.DistanceCondition;

namespace MBTExample
{
    [MBTNode("K/DistanceBetweenMe")]
    [AddComponentMenu("")]
    public class DistanceBetween : Leaf
    {
        public Comparator comparator = Comparator.GreaterThan;
        public FloatReference distance = new FloatReference(10f);
        [Space]
        public TransformReference transform2;

        public override NodeResult Execute()
        {
            //calculate distance between two transforms
            Vector2 self = transform.position.xz();
            Vector2 target = transform2.Value.position.xz();
            float magnitude = (target - self).magnitude;
            float dist = distance.Value;
            if (comparator == Comparator.GreaterThan)
            {
                if (magnitude > dist)
                {
                    return NodeResult.success;
                }
                else
                {
                    return NodeResult.failure;
                }
            }
            else
            {
                if (magnitude < dist)
                {
                    return NodeResult.success;
                }
                else
                {
                    return NodeResult.failure;
                }
            }
        }
    }
}