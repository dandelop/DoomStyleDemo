using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorDeactivatorLock : ActivatorDeactivator
{
    // key type to unlock
    [SerializeField] private KeyType _keyType;
    // is locked
    private bool _isLocked;
    
    // reference to player key holder
    private KeyHolder _keyHolder;

    private void Start()
    {
        _isLocked = true;
        // get reference to player
        _keyHolder = GameManager.Instance.Player.GetComponent<KeyHolder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) || (other.CompareTag("PlayerChildren")))
        {
            if (_isLocked)
            {
                if (_keyHolder != null)
                {
                    // unlock if player has the key
                    if (_keyHolder.HasKey(_keyType))
                    {
                        _isLocked = false;
                        _keyHolder.RemoveKey(_keyType);
                        onActivation.Invoke();
                    }
                }
            }
            else
            {
                onActivation.Invoke();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        onDeactivation.Invoke();
    }
    
}
