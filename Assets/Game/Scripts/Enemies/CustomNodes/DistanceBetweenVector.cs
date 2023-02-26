using Game.Scripts.Utility;
using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MBT.DistanceCondition;

namespace MBTExample
{
    [MBTNode("K/DistanceBetweenVector")]
    [AddComponentMenu("")]
    public class DistanceBetweenVector : Leaf
    {
        public Comparator comparator = Comparator.GreaterThan;
        public FloatReference distance = new FloatReference(10f);
        [Space]
        public Vector3Reference transform2;

        public override NodeResult Execute()
        {
            //calculate distance between two transforms
            Vector2 self = transform.position.xz();
            Vector2 target = transform2.Value.xz();
            float magnitude = (target - self).magnitude;
            //Debug.Log(magnitude);
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