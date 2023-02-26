using Game.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    #region Singleton pattern
    private static DialogueSystem _instance;
    public static DialogueSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DialogueSystem>();
            }
            return _instance;
        }
    }
    #endregion

    private void Start()
    {
        InputManager.OnSkipDialogueEvent += DeleteFirstMessage;
        Instance._dialogueWindow.SetActive(Instance._messageCount > 0);
    }

    [SerializeField] private GameObject _dialogueWindow;
    [SerializeField]private  VerticalLayoutGroup _backlog;
    [SerializeField]private GameObject _messagePrefab;
    private int _messageCount = 0;
    [SerializeField]private int _messageMax = 5;

    public static void AddMessage(string content, float durationInSeconds)
    {
        if(Instance==null)
        {
            Debug.LogError("No instance of dialogue system where found !");
            return;
        }
        GameObject newGo = GameObject.Instantiate(Instance._messagePrefab,Instance._backlog.transform);
        TMP_Text dialogueField=newGo.GetComponentInChildren<TMP_Text>();
        dialogueField.text = content;
        Instance.StartCoroutine(Instance.TimeDelete(newGo,durationInSeconds));
        Instance._messageCount++;
        Instance._dialogueWindow.SetActive(true);
        while (Instance._messageCount >= Instance._messageMax)
        {
            Instance.DeleteFirstMessage();
        }
    }

    public void DeleteFirstMessage()
    {
        DeleteMessage(Instance._backlog.transform.GetChild(0).gameObject);
    }

    public static void DeleteMessage(GameObject objectGameObject)
    {
        if (objectGameObject == null) return;
        DestroyImmediate(objectGameObject);
        Instance._messageCount--;
        Instance._dialogueWindow.SetActive(Instance._messageCount>0);
    }

    private IEnumerator TimeDelete(GameObject go,float time)
    {
        yield return new WaitForSecondsRealtime(time);
        DeleteMessage(go);
    }

    internal static void AddMessage(string completionMessage)
    {
        throw new NotImplementedException();
    }
}
