using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxResetter : MonoBehaviour
{
    [SerializeField] private int _nbrToPlay;
    [SerializeField] private ParticleSystem _particleSystem;
    private void OnEnable()
    {
        _particleSystem.Emit(_nbrToPlay);
    }
}
