using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Set vector to transform")]

    public class SetVectorToTransform : Leaf
    {
        public TransformReference transformRef;
        public Vector3Reference vector3Destination;

        public override NodeResult Execute()
        {
            transformRef.Value.position = vector3Destination.Value;
            return NodeResult.success;
        }
    }
}

