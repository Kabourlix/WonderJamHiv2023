using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreLayer : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreLayerCollision(6,6);
    }
}
