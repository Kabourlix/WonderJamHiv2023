using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemies;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    private MBTExecutorEnhanced[] _allIa;
    
    public static IAManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
        _allIa = FindObjectsOfType<MBTExecutorEnhanced>();
        Debug.Log($"We have {_allIa.Length} IA in the scene");
    }
    
    public void StartAllIa()
    {
        foreach (var ia in _allIa)
        {
            ia.StartLogic();
        }
    }
    
    public void FreezeAllIa()
    {
        foreach (var ia in _allIa)
        {
            ia.Freeze();
        }
    }
    
    public void UnfreezeAllIa()
    {
        foreach (var ia in _allIa)
        {
            ia.Unfreeze();
        }
    }
    
    
}
