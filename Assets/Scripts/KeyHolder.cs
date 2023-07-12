using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    // event to notify the key has changed
    public event EventHandler OnKeyChanged;
    
    // list of keys
    public List<KeyType> _keys;

    private void Awake()
    {
        _keys = new List<KeyType>();
    }
    
    // add a key to the list
    public void AddKey(KeyType keyType)
    {
        _keys.Add(keyType);
        OnKeyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    // remove a key from the list
    public void RemoveKey(KeyType keyType)
    {
        _keys.Remove(keyType);
        OnKeyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    // check if the key is in the list
    public bool HasKey(KeyType keyType)
    {
        return _keys.Contains(keyType);
    }
}
