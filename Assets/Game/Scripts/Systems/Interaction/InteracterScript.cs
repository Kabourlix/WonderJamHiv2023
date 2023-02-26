using Game.Scripts.Interaction;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Systems.Interaction;
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
    [SerializeField] GameObject _leashPrefab;
    private int _otherCollidersCounter=0;

    public int OtherCollidersCounter { get => _otherCollidersCounter; set
        {
            _otherCollidersCounter = value< 0 ? 0 : value;
            _contextualInteractMenu.SetActive(_otherCollidersCounter > 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Interactable>() == null) return;
        OtherCollidersCounter++;
    }

    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() == null) return;
        OtherCollidersCounter--;
    }

    public LeashFollowScript SpawnNewLeash(Rigidbody attachedRigidbody)
    {
        GameObject go=Instantiate(_leashPrefab);
        go.transform.position = transform.position;
        attachedRigidbody.position=go.transform.position;
        go.GetComponent<LeashFollowScript>().Init(gameObject, attachedRigidbody);

        return go.GetComponent<LeashFollowScript>();
    }
}
