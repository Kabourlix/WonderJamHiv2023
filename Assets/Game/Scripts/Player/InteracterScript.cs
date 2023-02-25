using Game.Scripts.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracterScript : MonoBehaviour
{
    [SerializeField] GameObject _contextualInteractMenu;
    private int _otherCollidersCounter=0;

    public int OtherCollidersCounter { get => _otherCollidersCounter; set
        {
            _otherCollidersCounter = value;
            _contextualInteractMenu.SetActive(_otherCollidersCounter > 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>() == null) return;
        OtherCollidersCounter++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>() == null) return;
        OtherCollidersCounter--;
    }
}
