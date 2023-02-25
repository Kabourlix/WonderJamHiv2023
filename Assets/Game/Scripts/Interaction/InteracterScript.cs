using Game.Scripts.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracterScript : MonoBehaviour
{
    #region Singleton pattern  
    public static InteracterScript Instance;
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

    [SerializeField] GameObject _contextualInteractMenu;
    [SerializeField] GameObject _contextualPushMenu;
    [SerializeField] GameObject _leashPrefab;
    private int _otherCollidersCounter=0;
    private int _pushableOtherCollidersCounter;

    public int OtherCollidersCounter { get => _otherCollidersCounter; set
        {
            _otherCollidersCounter = value;
            _contextualInteractMenu.SetActive(_otherCollidersCounter > 0);
        }
    }

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
        if(other.gameObject.GetComponent<Pushable>() != null)
        {
            PushableOtherCollidersCounter++;
        }
        if (other.gameObject.GetComponent<IInteractable>() == null) return;
        OtherCollidersCounter++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Pushable>() != null)
        {
            PushableOtherCollidersCounter--;
        }
        if (other.gameObject.GetComponent<IInteractable>() == null) return;
        OtherCollidersCounter--;
    }

    public LeashFollowScript SpawnNewLeash(Rigidbody attachedRigidbody)
    {
        GameObject go=Instantiate(_leashPrefab);
        go.GetComponent<LeashFollowScript>().Init(gameObject, attachedRigidbody);

        return go.GetComponent<LeashFollowScript>();
    }
}
