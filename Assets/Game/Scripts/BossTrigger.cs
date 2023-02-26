using Game.Inputs;
using Game.Scripts.Enemies;
using Game.Scripts.Quests;
using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BossTrigger : MonoBehaviour
{
    private bool goodQuestCompleted = false;
    private bool playerInZone = false;
    [SerializeField] private List<string> startMessages;
    // Start is called before the first frame update
    void Start()
    {
        QuestManager.Instance.OnAllGoodQuestCompleted += GoodQuestCompleted;
        InputManager.OnTest4Event += GoodQuestCompleted;
    }

    [SerializeField] private GameObject bossGo;

    [SerializeField] private GameObject bossPosition;

    [SerializeField] private List<GameObject> bossItems;

    [SerializeField] private float timeToSurvive;

    [SerializeField] private UnityEvent _onWin;

    public void GoodQuestCompleted()
    {
        goodQuestCompleted = true;
        TryToSpawnBoss();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            TryToSpawnBoss();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    public void TryToSpawnBoss()
    {
        if (!playerInZone || !goodQuestCompleted) return;
        SpawnBoss();
    }

    public void SpawnBoss()
    {
        StartCoroutine(Dialogues());
        StartCoroutine(TeleportBoss());
        StartCoroutine(Win());
    }

    public IEnumerator Dialogues()
    {
        foreach(var item in startMessages)
        {
            DialogueSystem.AddMessage(item,8);
            yield return new WaitForSeconds(4);
        }
    }

    public IEnumerator TeleportBoss()
    {
        yield return new WaitForSeconds(3);
         
        bossGo.GetComponentInChildren<NavMeshAgent>().enabled = false;
        bossGo.GetComponentInChildren<Animator>().SetTrigger("BossFight");
        bossGo.transform.position = bossPosition.transform.position;

        foreach (var item in bossItems)
        {
            item.SetActive(true);
        }

        GetComponentInChildren<Animator>().SetTrigger("Repair");

        Destroy(bossGo.GetComponentInChildren<MBTExecutorEnhanced>().gameObject);
    }

    public IEnumerator Win()
    {
        yield return new WaitForSeconds(timeToSurvive);
        _onWin.Invoke();
        GameManager.Instance.ChangeState(GameState.EndLevel);
    }
}
