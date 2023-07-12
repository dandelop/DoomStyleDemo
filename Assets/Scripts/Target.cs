using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IPlayerShootReceiver
{
    // reference to particle system
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        // get reference to the particle system
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void OnShoot()
    {
        // play particle system
        _particleSystem.Play();
        Debug.Log("Target destroyed");
        // destroy the target
        //Destroy(gameObject);
    }
}