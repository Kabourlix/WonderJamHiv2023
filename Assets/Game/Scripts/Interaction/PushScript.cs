using Game.Scripts.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushScript : MonoBehaviour
{
    #region Singleton pattern  
    public static PushScript Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    [SerializeField] GameObject _contextualPushMenu;
    [SerializeField] GameObject _leashPrefab;
    private int _pushableOtherCollidersCounter;

    public int PushableOtherCollidersCounter
    {
        get => _pushableOtherCollidersCounter;set
        {
            _pushableOtherCollidersCounter = value;
            _contextualPushMenu.SetActive(_pushableOtherCollidersCounter > 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Pushable>() == null) return;
        PushableOtherCollidersCounter++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Pushable>() == null) return;
        PushableOtherCollidersCounter--;
    }
}
