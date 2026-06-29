using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableEvent", menuName = "Scriptable Objects/InteractableEvent")]
public class InteractableEventScriptableObject : ScriptableObject
{
    public Action BroadcastToReceiver;
    public Action ReplyFromReceiver;
    
    public void Broadcast()
    {
        BroadcastToReceiver?.Invoke();
    }

    public void Reply()
    {
        ReplyFromReceiver?.Invoke();
    }
}
