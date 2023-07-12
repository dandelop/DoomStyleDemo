using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // activation event (optional)
    // Version 01: single event emitter
    //[SerializeField] private ActivatorDeactivator _activationEvent;
    // Version 02: multiple event emitters
    [SerializeField] private List<ActivatorDeactivator> _activationEvents;
    
    // open direction
    [SerializeField] private Vector3 _openDirection = Vector3.left;
    private Vector3 _closeDirection;
    
    // door state
    private bool _isOpen;
    private bool _isClose;
    private bool _isOpening;
    private bool _isClosing;
    
    // original position of the door
    private Vector3 _position0;
    
    // temporal distance to consider the door is closed
    private float _lastDistance;
    
    
    private void Start()
    {
        // subscribe to activation event
        // Version 01: single event emitter
        /*
        if (_activationEvent != null)
        {
            _activationEvent.onActivation.AddListener(Open);
            _activationEvent.onDeactivation.AddListener(Close);
        }
        */
        // Version 02: multiple event emitters
        if (_activationEvents != null)
        {
            foreach (ActivatorDeactivator activationEvent in _activationEvents)
            {
                activationEvent.onActivation.AddListener(Open);
                activationEvent.onDeactivation.AddListener(Close);
            }
        }
        // set position 0
        _position0 = transform.position;
        // set close direction
        _closeDirection = -_openDirection;
        // set initial state
        _isOpen = false;
        _isClose = true;
        _isOpening = false;
        _isClosing = false;
        // set minimum distance
        _lastDistance = float.MaxValue;
    }

    public void Open()
    {
        // Open the door
        if (_isOpen || _isOpening)
        {
            return;
        }
        _isOpening = true;
        _isClosing = false;
        _isOpen = false;
        _isClose = false;
    }

    public void Close()
    {
        // Close the door
        if (_isClose || _isClosing)
        {
            return;
        }
        _isOpening = false;
        _isClosing = true;
        _isOpen = false;
        _isClose = false;
    }
    
    private void Update()
    {
        if (_isOpen || _isClose)
        {
            return;
        }
        if (_isOpening)
        {
            // Open the door
            transform.Translate(_openDirection * Time.deltaTime);
            if ((transform.position - _position0).magnitude >= _openDirection.magnitude)
            {
                _isOpening = false;
                _isClosing = false;
                _isOpen = true;
                _isClose = false;
            }
        }
        else if (_isClosing)
        {
            // Close the door
            transform.Translate(_closeDirection * Time.deltaTime);
            // when magnitude is greater than last magnitude, the door is closed
            float distance = (transform.position - _position0).magnitude;
            if (distance >= _lastDistance)
            {
                _isOpening = false;
                _isClosing = false;
                _isOpen = false;
                _isClose = true;
                // reset original position
                transform.position = _position0;
                _lastDistance = float.MaxValue;
            }
            else
            {
                _lastDistance = distance;
            }
        }
    }
}
