using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/check is in FOV")]
    public class CheckIsInFOV : Service
    {
        public LayerMask mask = -1;
        public GameObjectReference SelfObj;
        public TransformReference TargetObj;
        public BoolReference variableToSet = new BoolReference();
        public float FOVAngle;

        public override void Task()
        {
            //calculate if target is in FOV
            Vector3 direction = TargetObj.Value.position - SelfObj.Value.transform.position;
            float angle = Vector3.Angle(direction, (SelfObj.Value.transform.GetChild(0).transform.position - SelfObj.Value.transform.position));
            if (angle < FOVAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(SelfObj.Value.transform.position + SelfObj.Value.transform.up, direction.normalized, out hit, 100, mask))
                {
                    if (hit.collider.gameObject == TargetObj.Value.gameObject)
                    {
                        variableToSet.Value = true;
                    }
                    else
                    {
                        variableToSet.Value = false;
                    }
                }
            }
            else
            {
                variableToSet.Value = false;
            }
        }
    }
}