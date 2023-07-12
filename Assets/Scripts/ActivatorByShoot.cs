using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorByShoot : ActivatorDeactivator, IPlayerShootReceiver
{
    public void OnShoot()
    {
        // launch activation event
        onActivation.Invoke();
        
        // Destroy gameObject
        Destroy(gameObject);
    }
    
    
}
