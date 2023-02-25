using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Cac Attack")]

    public class CacAttack : Service
    {
        public GameObjectReference gameObjectSelf = new GameObjectReference();
        public GameObjectReference gameObjectTarget = new GameObjectReference();

        public override void Task()
        {
            Debug.Log("PAF");
            //animation

            //infliger les dégats à la cible.
                //Note : GetCompenent du player pour appeler fonction dommage
            //gameObjectTarget.Value.GetComponent
        }
    }
}
