using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class isWalking : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //check if AI navmeshAgent is walking
        if (agent.velocity.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}
