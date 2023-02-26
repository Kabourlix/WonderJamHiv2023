using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    private bool _isPushed;

    private Vector3 _originalPosition;
    public Vector3 _finalPosition;
    private float _progression;
    private float _pushDuration=0.5f;
    [SerializeField] private AnimationCurve _pushTweening;

    [SerializeField] private UnityEngine.Events.UnityEvent _onPushedStart;
    [SerializeField] private UnityEngine.Events.UnityEvent _onPushedEnd;

    [HideInInspector] public bool CanBePushed=true;

    public bool IsPushed { get => _isPushed; 
        set 
        { 
            _isPushed = value;
            if (_isPushed)
            {
                _onPushedStart.Invoke();
            }
            else
            {
                _onPushedEnd.Invoke();
            }
        }
    }

    void Update()
    {
        if (!IsPushed) return;
        _progression += Time.deltaTime / _pushDuration;
        float lerp = _pushTweening.Evaluate(_progression);

        if (_progression >= 1)
        {
            lerp = 1;
            IsPushed = false;
        }
        
        transform.position = Vector3.Lerp(_originalPosition, _finalPosition, lerp);
        
    }

    public void Push(Vector3 finalPosition, float pushDuration=0.5f)
    {
        if(IsPushed || !CanBePushed) { return; }
        IsPushed = true;
        _finalPosition = finalPosition;
        _pushDuration = pushDuration;
        _progression = 0;
        _originalPosition=transform.position;
    }
    //Wankil Studio
}
