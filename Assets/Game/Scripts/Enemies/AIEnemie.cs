using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemie : MonoBehaviour
{
    [SerializeField] private MBTExecutor mBT;
    [SerializeField] private NavMeshAgent agent;
    private void Start()
    {
        mBT = transform.GetChild(0).GetComponent<MBTExecutor>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void Freeze(bool state)
    {
        mBT.freez = state;
        agent.isStopped = state;
    }
    public IEnumerator FreezCoroutine(float freezTime)
    {
        Freeze(true);
        yield return new WaitForSeconds(freezTime);
        Freeze(false);
    }
}
