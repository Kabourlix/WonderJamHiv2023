using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Reset transform value")]
    public class ResetVariable : Leaf
    {
        public TransformReference transformReference;
        public override NodeResult Execute()
        {
            transformReference.Value = null;
            return NodeResult.success;
        }
    }
}