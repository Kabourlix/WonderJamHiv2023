using Game.Scripts.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform questIndicatorObject;
    private LayerMask maskCollide;
    private bool isActivated = true;
    private void Awake()
    {
        maskCollide = LayerMask.NameToLayer("Interactable");
    }

    private void Desactivate()
    {
        isActivated = false;
        questIndicatorObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;
        
        if (cam == null) return;
        questIndicatorObject.LookAt(cam.transform.position);
            
        if(maskCollide != gameObject.layer)
            Desactivate();
    }
}
