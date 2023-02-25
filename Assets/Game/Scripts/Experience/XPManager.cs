using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Quests;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;


public class XPManager : MonoBehaviour
{
    private int _currentXp, _level;
    [SerializeField] private int targetXp = 30;
    
    private PlayerController _playerController;
    private QuestManager _questManager;
    private HUDManager _hud;
    
    
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _questManager??= QuestManager.Instance;
        _hud??= HUDManager.Instance;
        _questManager.OnQuestCompleted += UpdateXPCallback;
        PlayerController.OnEvolveEnd += Evolve;
    }

    private void OnEnable()
    {
        if(_questManager != null)
            _questManager.OnQuestCompleted += UpdateXPCallback;
    }

    private void OnDisable()
    {
        _questManager.OnQuestCompleted -= UpdateXPCallback;
    }

   private void UpdateXPCallback(Quest quest)
   {
       if (_level >= 3 || quest.QuestType != QuestType.Evil) return;
       
       AddXP(quest.RewardXp); 
       Debug.Log($"You gained {quest.RewardXp} xp !");

   }
   
    private void AddXP(int xp)
    {
        _currentXp += xp;
        _hud.UpdateExpBar(Mathf.InverseLerp(0, targetXp, _currentXp));

        if (_currentXp < targetXp) return;
        
        _currentXp = targetXp - _currentXp; // Better to reset to 0 ?
        _level++;
        _playerController.EvolveBegin();
    }

   
    public void Evolve()
    {
        _playerController.CurrentForm = _level switch
        {
            1 => PlayerController.Forms.spider,
            2 => PlayerController.Forms.humanoid,
            _ => _playerController.CurrentForm
        };
    }
}
