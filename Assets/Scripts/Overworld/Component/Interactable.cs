using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onInteract, onDeclaredClosest, onDeclaredNotClosest;

    private void Start()
    {
        gameObject.tag = "Interactable";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            InteractableMessageHub.OnInteractableFoundPlayer?.Invoke(this);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            InteractableMessageHub.OnInteractableLostPlayer?.Invoke(this);
    }

    private void OnDisable()
    {
        InteractableMessageHub.OnInteractableLostPlayer?.Invoke(this);
    }

    public void Interact()
    {
        onInteract?.Invoke();
    }

    public void DeclareClosest(bool isClosest)
    {
        if (isClosest)
            onDeclaredClosest?.Invoke();
        else
            onDeclaredNotClosest?.Invoke();
    }
}
