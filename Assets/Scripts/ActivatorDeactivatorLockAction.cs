using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorDeactivatorLockAction : ActivatorDeactivator
{
    // key type to unlock
    [SerializeField] private KeyType _keyType;
    // is locked
    private bool _isLocked;
    
    // reference to player key holder
    private KeyHolder _keyHolder;

    // activation event (external)
    public IActionEvent iActionEvent;

    // stay in activation area (for player idle)
    public bool stayInActivationArea;
    
    // last state of activation / deactivation
    private bool _lastStateActivation;
    
    private void Start()
    {
        _isLocked = true;
        // TO-DO: implement list of keyholders (player, enemy, etc.)
        // get reference to player
        _keyHolder = GameManager.Instance.Player.GetComponent<KeyHolder>();
        
        // TO-DO: implement list of emissor (player, enemy, etc.)
        // get reference to the event emissor (Player)
        iActionEvent = GameManager.Instance.Player.GetComponent<IActionEvent>();
        
        // subscribe to the event
        if (iActionEvent != null)
        {
            iActionEvent.OnActionEvent += OnAction;
        }
        
        // set stay in activation area to false
        stayInActivationArea = false;
        
        // set last state
        _lastStateActivation = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) || (other.CompareTag("PlayerChildren")))
        {
            stayInActivationArea = true;
            if (_isLocked)
            {
                if (_keyHolder != null)
                {
                    // unlock if player has the key
                    if (_keyHolder.HasKey(_keyType))
                    {
                        _isLocked = false;
                        _keyHolder.RemoveKey(_keyType);
                    }
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        stayInActivationArea = false;
    }
    
    
    public void OnAction(object sender, EventArgs eventArgs)
    {
        if (_isLocked)
        {
            return;
        }
            
        // if player is in the activation area and player launch action event
        if (stayInActivationArea)
        {
            // alternate activation / deactivation state
            if (_lastStateActivation)
            {
                // launch deactivation event
                onDeactivation.Invoke();
                _lastStateActivation = false;
            }
            else
            {
                // launch activation event
                onActivation.Invoke();
                _lastStateActivation = true;
            }
        }
    }

}
