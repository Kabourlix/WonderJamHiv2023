using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Distance Attack")]
    public class DistanceAttack : Service
    {
        public GameObjectReference gameObjectSelf = new GameObjectReference();
        public GameObjectReference gameObjectTarget = new GameObjectReference();

        public override void Task()
        {
            Debug.Log("PEWPEW");
            //Call bot distance attaque avec la position de la cible en parametre
            //gameObjectSelf.Value.GetComponent<>().
        }
    }
}
