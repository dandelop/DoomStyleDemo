using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // radius of the circle the enemy can move in
    [SerializeField] private float _radius = 3f;
    
    // speed of the enemy
    [SerializeField] private float _speed = 1f;
    
    [SerializeField] private float _loopingPositionAnimationDuration = 2f;
    
    // time of life of the enemy
    private float _timeOfLife;
    
    // reference to "parent" rigidbody
    private Rigidbody _rigidbody;
    
    // center of the circle
    private Vector3 _center;
    
    private void Start()
    {
        // start life counter
        _timeOfLife = 0f;
        // get reference to the rigidbody
        _rigidbody = transform.parent.GetComponent<Rigidbody>();
        
        // calculate the center point of the circle: position of the enemy plus the radius in forward direction
        _center = transform.position + transform.forward * _radius;
        // start the looping position animation
        StartCoroutine(LoopPosition());
    }

    private void Update()
    {
        // increment the time of life
        _timeOfLife += Time.deltaTime;
    }

    // make a loop relative position animation
    private IEnumerator LoopPosition()
    {
        float angle = 0f;
        float lastTime = Time.time;
        float initialX = transform.position.x;
        float initialZ = transform.position.z;
        float lastX = initialX;
        float lastZ = initialZ;
        float newPosX = 0f;
        float newPosZ = 0f;
        while (true)
        {
            for (float t = 0f; t < _loopingPositionAnimationDuration;)
            {
                t += (Time.time - lastTime);
                lastTime = Time.time;
                
                angle = t * (1 / _loopingPositionAnimationDuration) * 2 * Mathf.PI;
                //Debug.Log("Angle Loop (angle): " + angle);
                newPosX = initialX + _radius * Mathf.Sin(angle);
                newPosZ = (initialZ + _radius) - _radius * Mathf.Cos(angle);
                // move the enemy rigidbody
                _rigidbody.MovePosition(new Vector3(newPosX, transform.position.y, newPosZ));
                // rotate the enemy rigidbody to look forward
                _rigidbody.MoveRotation(Quaternion.LookRotation(new Vector3(newPosX - lastX, 0, newPosZ - lastZ)));
                lastX = newPosX;
                lastZ = newPosZ;
                //transform.rotation = Quaternion.Euler(0, 0, t * (1 / _loopingPositionAnimationDuration) * 360);
                //yield return null;
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
    
    
    // draw gizmo to show the circle
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_center, _radius);
        
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, _radius);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, _center);
    }

}
