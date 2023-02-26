using System;
using System.Collections;
using System.Collections.Generic;
using Aurore.MainMenu;
using Game.Scripts.Quests;
using Game.Scripts.Systems.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InputManager = Game.Inputs.InputManager;

namespace Game.Scripts.UI
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance;
        private AudioSource audioSrc;
        
        private void Awake()
        {
            if(Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
            //DontDestroyOnLoad(gameObject); // Keep it alive through levels
            
            _experienceTween = experienceBar.GetComponent<ApparitionTween>();
            _questTween = questPanel.GetComponent<ApparitionTween>();
            
        }

        private void Start()
        {
            InputManager.OnShowHideQuestEvent += () => ShowQuestPanel(!_questTween.IsShown);
            QuestManager.Instance.OnQuestCompleted += (q) => ShowQuestCompleted(q);
            
            ShowQuestPanel(false);
            _experienceTween.Disappear();
            
            gameoverScreen.SetActive(false);
            winEvilScreen.SetActive(false);
            winGoodScreen.SetActive(false);
            audioSrc = GetComponent<AudioSource>();
            
        }

        

        [SerializeField] private GameObject questPanel;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Slider experienceBar;
        [SerializeField] private Slider suspiciousBar;
        [SerializeField] private Image isVisibleObj;
        [SerializeField] private Image isSuspectObj;

        //image to set
        [SerializeField] private Sprite visibleImg;
        [SerializeField] private Sprite hideImg;
        [SerializeField] private Sprite caughtImg;
        [SerializeField] private Sprite suspectImg;

        //khadidja updates
        [SerializeField] private GameObject winEvilScreen;
        [SerializeField] private GameObject winGoodScreen;
        [SerializeField] private GameObject gameoverScreen;

        //Son
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip evolveSound;
        [SerializeField] private AudioClip gainXpSound;
        [SerializeField] private AudioClip questCompletedSound;


        #region Tweeners

        private ApparitionTween _questTween;
        private ApparitionTween _experienceTween;
        

        #endregion
        
        private Dictionary<Quest,QuestUI> _questUIs = new Dictionary<Quest, QuestUI>();


        #region Quests

        public void ShowQuestPanel(bool b)
        {
            if(b)
                _questTween.Appear();
            else
                _questTween.Disappear();
        }


        public void AddQuest(Quest quest)
        {
            var questGO = Instantiate(questPrefab, questPanel.transform);
            var ui = questGO.GetComponent<QuestUI>();
            ui.Init(quest);
            _questUIs.Add(quest,ui);
        }

        #endregion

        [SerializeField] private float expBarDuration = 2f;
        public void UpdateExpBar(float value)
        {
            PlayGainExp();
            Debug.Log("<color=red>UpdateExpBar</color>");
            value = Mathf.Clamp01(value);
            
            Action c = () => LeanTween.value(experienceBar.gameObject, experienceBar.value, value, 1f)
                .setOnUpdate((float val) =>
                {
                    experienceBar.value = val;
                }).setOnComplete(() =>
                    {
                        LeanTween.value(gameObject, 0, 1, expBarDuration).setOnComplete(() =>
                        {
                            _experienceTween.Disappear();
                        });

                    });
            
            _experienceTween.Appear(c);
        }

        public void SetVisible()
        {
            isVisibleObj.sprite = visibleImg;
            isSuspectObj.gameObject.SetActive(false);
        }
        public void SetHide()
        {
            isVisibleObj.sprite = hideImg;
            isSuspectObj.gameObject.SetActive(false);
        }
        public void SetCaught()
        {
            isVisibleObj.sprite = caughtImg;
            isSuspectObj.gameObject.SetActive(true);
        }
        public void isSuspicious(bool state)
        {
            isSuspectObj.gameObject.SetActive(state);
        }
        
        public void UpdateSuspiciousBar(float value) => suspiciousBar.value = Mathf.Clamp01(value);
        
        #region Panel Auto Display

        public void ShowQuestTemp(Quest q)
        {
            var ui = _questUIs[q];
            if(!_questTween.IsShown)
                ShowQuestPanel(true);
            Action c  = () => ShowQuestPanel(false);
            ui.Highlight(c);
        }

        public void ShowQuestCompleted(Quest q)
        {
            var ui = _questUIs[q];
            if(!_questTween.IsShown)
                ShowQuestPanel(true);
            Action c = () =>
            {
                ShowQuestPanel(false);
                _questUIs[q].QuestCompleted();
            };
            ui.Highlight(c);
            PlayquestComplete();
        }

        #endregion

        public void ShowGameOver()
        {
            //TODO
            PlayGameOver();
            gameoverScreen.SetActive(true);
        }

        public void ShowWinEvil()
        {
            //TODO
            PlayWinGame();
            winEvilScreen.SetActive(true);
        }
        public void ShowWinGood()
        {
            //TODO
            PlayWinGame();
            winGoodScreen.SetActive(true);
        }

        public void ShowPause()
        {
            //TODO
            PauseManager.Instance.EnablePause();
        }

        #region PlayAudio
        private void PlayGameOver()
        {
            audioSrc.PlayOneShot(gameOverSound);
        }
        private void PlayWinGame()
        {
            audioSrc.PlayOneShot(winSound);
        }
        private void PlayGainExp()
        {
            audioSrc.PlayOneShot(gainXpSound);
        }
        private void PlayquestComplete()
        {
            audioSrc.PlayOneShot(questCompletedSound);
        }
        #endregion

        public void Home()
        {
            //Go back to menu scene
            SceneManager.LoadScene("MainMenu");
        }

        public void Retry()
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
