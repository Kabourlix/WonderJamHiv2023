using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemie : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Blackboard IAvariables; 
    private Transform _tf;

    private void Start()
    {
        _tf = transform;
        //IAvariables.GetVariable<BoolVariable>("HearSomething").Value = true;
    }
}
