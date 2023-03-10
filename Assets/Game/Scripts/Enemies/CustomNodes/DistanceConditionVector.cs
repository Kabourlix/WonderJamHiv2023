using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Conditions/Distance Condition Vector")]
    public class DistanceConditionVector : Condition
    {
        public Comparator comparator = Comparator.GreaterThan;
        public FloatReference distance = new FloatReference(10f);
        [Space]
        public TransformReference transform1;
        public Vector3Reference transform2;

        public override bool Check()
        {
            // Squared magnitude is enough to compare distances
            float sqrMagnitude = (transform1.Value.position - transform2.Value).sqrMagnitude;
            float dist = distance.Value;
            if (comparator == Comparator.GreaterThan)
            {
                return sqrMagnitude > dist * dist;
            }
            else
            {
                return sqrMagnitude < dist * dist;
            }
        }

        public enum Comparator
        {
            GreaterThan, LessThan
        }
    }
}
