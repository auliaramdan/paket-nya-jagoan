using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class InteractableEventHandler : MonoBehaviour
{
    [SerializeField]
    private InteractableEventScriptableObject interactableItem;
    
    [SerializeField]
    private bool isReceiver;

    [SerializeField, ShowIf("isReceiver")]
    private UnityEvent onReceiveInteractionEvent;
    
    [SerializeField, HideIf("isReceiver")]
    private UnityEvent onInteractionEventReplied;
    
    void OnEnable()
    {
        interactableItem.BroadcastToReceiver += InvokeEvent;
        interactableItem.ReplyFromReceiver += onInteractionEventReplied.Invoke;
    }

    void OnDisable()
    {
        interactableItem.BroadcastToReceiver -= InvokeEvent;
        interactableItem.ReplyFromReceiver -= onInteractionEventReplied.Invoke;
    }

    private void InvokeEvent()
    {
        Debug.Log("Invoke");
        onReceiveInteractionEvent.Invoke();
    }
}
