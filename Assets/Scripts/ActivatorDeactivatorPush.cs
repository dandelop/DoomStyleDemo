using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorDeactivatorPush : ActivatorDeactivator
{
    private void OnTriggerEnter(Collider other)
    {
        onActivation.Invoke();
    }
    
    private void OnTriggerExit(Collider other)
    {
        onDeactivation.Invoke();
    }
}
