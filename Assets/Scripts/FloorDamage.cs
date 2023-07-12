using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDamage : MonoBehaviour
{
    // frequency of the damage
    [SerializeField] private float frequency = 1f;
    
    // counter time for the damage frequency
    private float counterTime = 0f;
    
    // flag to check if the damage is active
    //private bool damageActive = false;
    
    private void OnTriggerStay(Collider other)
    {
        //if (damageActive)
        {   
            counterTime -= Time.deltaTime;
            if (counterTime <= 0f)
            {
                counterTime = frequency;
                infringeDamage(other);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // activate the damage
        //damageActive = true;
        infringeDamage(other);
        // reset the counter time
        counterTime = frequency;
    }
    
    private void OnTriggerExit(Collider other)
    {
        // deactivate the damage
        //damageActive = false;
    }
    
    private void infringeDamage(Collider other)
    {
        // verify if the object hit is the player or one of his children
        if ((other.CompareTag("Player")) || (other.CompareTag("PlayerChildren")))
        {
            // get the component FloorDamageReceiver from the object collider
            IFloorDamageReceiver floorDamageReceiver = null;
            if (other.CompareTag("Player"))
            {
                floorDamageReceiver = other.GetComponent<IFloorDamageReceiver>();
            }
            else if (other.CompareTag("PlayerChildren"))
            {
                floorDamageReceiver = other.GetComponentInParent<IFloorDamageReceiver>();
            }
            // if the object hit has the component FloorDamageReceiver, call the method OnFloorDamage()
            if (floorDamageReceiver != null)
            {
                floorDamageReceiver.OnFloorDamage();
            }
        }
    }
    
    // update the counter time
    private void Update()
    {
        
    }
}
