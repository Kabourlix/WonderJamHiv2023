using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Quests;
using UnityEngine;


public class XPManager : MonoBehaviour
{
    public int currentXP, targetXP, level;
    public static XPManager instance;


    private void Awake()
    {
        if (instance == null)
            instance = this; 
        else
            Destroy(gameObject);
    } 

    void Start()
    {
        level = 0;
        currentXP = 0;
        targetXP = 30;
    }
    
/* 
    private void OnEnable()
    {
        QuestManager.Instance.OnQuestCompleted += UpdateXP;
    }

    private void OnDisable()
    {
        QuestManager.Instance.OnQuestCompleted -= UpdateXP;
    }

   void UpdateXP(Quest quest)
    {
        if (level < 3 && quest.QuestType == QuestType.Evil)
        { 
            XPManager.instance.AddXP(10); 
            Debug.Log("You gained 10xp !");  
        }

    }
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && level < 2)
        {
             XPManager.instance.AddXP(10); 
             Debug.Log("You gained 10xp !");   
        }

    }
    public void AddXP(int xp)
    {
        currentXP += xp;

        if (currentXP >= targetXP )
        {
            currentXP = targetXP - currentXP;
            level++;
            NewSkin();
        }
    }

    private void NewSkin()
    {
        switch (level)
        {
            case 1:
                GetComponent<PlayerController>().CurrentForm = PlayerController.Forms.spider;
                break;
            case 2:
                GetComponent<PlayerController>().CurrentForm = PlayerController.Forms.humanoid;
                break;
        }
    }
}
