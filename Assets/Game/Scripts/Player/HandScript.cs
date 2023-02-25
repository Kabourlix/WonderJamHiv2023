using Game.Scripts.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.XR;

public class HandScript : MonoBehaviour
{
    [SerializeField] private LayerMask grabableLayerMask;

    [Header("Externals")]
    [SerializeField] private GameObject _vfxHand;
    [SerializeField] private CapsuleCollider _interactCollider;

    private Rigidbody _currentGrabbedObject = null;
    [SerializeField] private string _grabbedObjectLayer;
    private int _grabbedObjectLayerIndex;
    private int _cachedLayerIndex;
    
    private void Awake()
    {
        _grabbedObjectLayerIndex=LayerMask.NameToLayer(_grabbedObjectLayer);
    }

    void Start()
    {
        InputManager.OnGrabEvent += OnGrab;
    }

    void OnGrab() 
    {
        if(_currentGrabbedObject!=null)
        {
            UngrabObject();
        }
        else
        {
            GrabObject();
        }
    }

    void GrabObject()
    {
        var bounds = _interactCollider.bounds;
        var colliders = Physics.OverlapCapsule(bounds.center, bounds.center + new Vector3(0, _interactCollider.height, 0), _interactCollider.radius, grabableLayerMask);
        //We'll keep only the closest to us
        var closest = colliders.Where(x => x.GetComponent<Rigidbody>() != null).OrderBy(x => Vector3.SqrMagnitude(x.transform.position - bounds.center)).FirstOrDefault();
        if (closest == null) return;
        _currentGrabbedObject = closest.GetComponent<Rigidbody>(); ;
        
        _vfxHand.SetActive(true);
        _cachedLayerIndex=_currentGrabbedObject.gameObject.layer;
        _currentGrabbedObject.transform.SetParent(transform);
        _currentGrabbedObject.transform.localPosition = Vector3.zero;

        var children = GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = _grabbedObjectLayerIndex;
        }
    }

    void UngrabObject()
    {
        _vfxHand.SetActive(false);

        var children = GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = _cachedLayerIndex;
        }

        _currentGrabbedObject=null; 
    }
}
