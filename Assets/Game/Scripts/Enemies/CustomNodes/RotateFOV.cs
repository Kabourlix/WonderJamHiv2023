using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Rotate FOV")]
    public class RotateFOV : Leaf
    {
        public GameObjectReference SelfObj;
        [Tooltip("Angle to rotate in degrees")]
        [Range(5, 180)]
        public float RotateAngle;

        public override NodeResult Execute()
        {
            //rotate self to look around for player in FOV
            StartCoroutine(RotateAnim());
            return NodeResult.success;
        }
        private IEnumerator RotateAnim()
        {
            //rotate self to look around for player in FOV
            //calcule theta angle
            float theta = RotateAngle/2 * Mathf.Deg2Rad;
            //rotate in one side
            SelfObj.Value.transform.Rotate(SelfObj.Value.transform.GetChild(0).transform.position - SelfObj.Value.transform.position, theta);
            yield return new WaitForSeconds(1.5f);
            SelfObj.Value.transform.Rotate(SelfObj.Value.transform.GetChild(0).transform.position - SelfObj.Value.transform.position, -theta);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
