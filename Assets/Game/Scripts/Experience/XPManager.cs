using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Quests;
using UnityEngine;


public class XPManager : MonoBehaviour
{
    public int currentXP, targetXP, level;

    public static XPManager instance;

    [SerializeField] private MeshFilter currentModel;
    [SerializeField] private Mesh targetModelLVL1, targetModelLVL2;
     

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
    
    public void AddXP(int xp)
    {
        currentXP += xp;

        if (currentXP >= targetXP )
        {
            currentXP = targetXP - currentXP;
            level++;
            LevelUp();
        }
    }

        void LevelUp()
    {
        switch (level)
        {
            
            case 1:
                currentModel.mesh = targetModelLVL1;
                break;

            case 2:
                currentModel.mesh = targetModelLVL2;
                break;
        }
    }
}
