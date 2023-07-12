using UnityEngine;
using UnityEngine.Events;

public abstract class ActivatorDeactivator : MonoBehaviour
{
    public UnityEvent onActivation = new UnityEvent();
    public UnityEvent onDeactivation = new UnityEvent();
}