using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private KeyType _keyType;
    public KeyType KeyType => _keyType;
    
    public static Color GetColor(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Red:
                return Color.red;
            case KeyType.Green:
                return Color.green;
            case KeyType.Blue:
                return Color.blue;
            default:
                return Color.white;
        }
    }
    
    // on trigger enter -> get key
    private void OnTriggerEnter(Collider other)
    {
        KeyHolder keyHolder = null;
        if (other.CompareTag("Player"))
        {
            keyHolder = other.GetComponent<KeyHolder>();
        }
        else if (other.CompareTag("PlayerChildren"))
        {
            keyHolder = other.GetComponentInParent<KeyHolder>();
        }
        if (keyHolder != null)
        {
            keyHolder.AddKey(_keyType);
            Destroy(gameObject);
        }
    }
    
}
