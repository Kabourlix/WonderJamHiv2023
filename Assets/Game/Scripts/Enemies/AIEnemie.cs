using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemie : MonoBehaviour
{

    private void Start()
    {
        GetComponent<PlayerController>().IsSuspect = true;
    }
}
