using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Quests;
using Game.Scripts.UI;
using UnityEngine;
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
       
       AddXP(10); 
       Debug.Log("You gained 10xp !");

   }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _level < 2)
        {
             AddXP(10); 
             Debug.Log("You gained 10xp !");   
        }

    }
    public void AddXP(int xp)
    {
        _currentXp += xp;
        _hud.UpdateExpBar(Mathf.InverseLerp(0, targetXp, _currentXp));

        if (_currentXp < targetXp) return;
        
        _currentXp = targetXp - _currentXp; // Better to reset to 0 ?
        _level++;
        Evolve();
    }

    private void Evolve()
    {
        _playerController.CurrentForm = _level switch
        {
            1 => PlayerController.Forms.spider,
            2 => PlayerController.Forms.humanoid,
            _ => _playerController.CurrentForm
        };
    }
}
