using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutorotatePlaneToPlayer : MonoBehaviour
{
    // rotate the plane to face the player on x and z axis
    private void Update()
    {
        // get the player position
        var playerPosition = GameManager.Instance.Player.transform.position;
        
        // get the direction to the player
        var direction = playerPosition - transform.position;
        
        // get the rotation to the player
        var rotation = Quaternion.LookRotation(direction);
        
        // rotate the plane to face the player
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }
    
    // draw gizmo to show the plane
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
