using Game.Scripts.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    [SerializeField] private LayerMask grabableLayerMask;

    [Header("Externals")]
    [SerializeField] private GameObject _vfxHand;
    [SerializeField] private CapsuleCollider _interactCollider;


    //Components
    private Joint joint;

    private Rigidbody _currentGrabbedObject = null;
    [SerializeField] private string _grabbedObjectLayer;
    private int _grabbedObjectLayerIndex;
    private int _cachedLayerIndex;
    
    private void Awake()
    {
        joint = GetComponent<Joint>();
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
        joint.connectedBody = _currentGrabbedObject;
        _vfxHand.SetActive(true);
        _cachedLayerIndex=_currentGrabbedObject.gameObject.layer;
        _currentGrabbedObject.gameObject.layer = _grabbedObjectLayerIndex;
    }

    void UngrabObject()
    {
        _vfxHand.SetActive(false);
        joint.connectedBody = null;
        _currentGrabbedObject.gameObject.layer = _cachedLayerIndex;
        _currentGrabbedObject=null; 
    }
}
