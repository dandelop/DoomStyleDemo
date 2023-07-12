using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorDeactivatorAction : ActivatorDeactivator
{
    // activation event (external)
    public IActionEvent iActionEvent;

    // stay in activation area (for player idle) // DEBUG
    public bool stayInActivationArea;
    
    // last state of activation / deactivation
    private bool _lastStateActivation;
    
    // reference to materials to change state on/off
    [SerializeField] private Material _materialOn;
    [SerializeField] private Material _materialOff;
    // flag to verify if materials are set
    private bool _materialsSet = false;
    
    // reference to mesh renderer
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
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
        
        // verify references to materials
        if ((_materialOn != null) && (_materialOff != null))
        {
            _materialsSet = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) || (other.CompareTag("PlayerChildren")))
        {
            stayInActivationArea = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player")) || (other.CompareTag("PlayerChildren")))
        {
            stayInActivationArea = false;
        }
    }
    
    
    public void OnAction(object sender, EventArgs eventArgs)
    {
        // if player is in the activation area and player launch action event
        if (stayInActivationArea)
        {
            // alternate activation / deactivation state
            if (_lastStateActivation)
            {
                // launch deactivation event
                onDeactivation.Invoke();
                _lastStateActivation = false;
                // change material
                if (_materialsSet)
                {
                    _meshRenderer.material = _materialOff;
                }
            }
            else
            {
                // launch activation event
                onActivation.Invoke();
                _lastStateActivation = true;
                // change material
                if (_materialsSet)
                {
                    _meshRenderer.material = _materialOn;
                }
            }
        }
    }
}
