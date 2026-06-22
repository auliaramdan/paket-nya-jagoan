using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInteractHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onInteractableFound, onInteractableLost;
    
    private Vector3 _lastPlayerPosition;

    private List<Interactable> _registeredInteractables = new();
    private Interactable _closestInteractable;
    
    private const float LastPlayerPositionThreshold = 0.2f;
    private const float InteractableMaxDistanceThreshold = 2f;

    private void Start()
    {
        _lastPlayerPosition = transform.position;
    }
    
    private void OnEnable()
    {
        InteractableMessageHub.OnInteractableFoundPlayer += RegisterInteractable;
        InteractableMessageHub.OnInteractableLostPlayer += UnregisterInteractable;
    }

    private void OnDisable()
    {
        InteractableMessageHub.OnInteractableFoundPlayer -= RegisterInteractable;
        InteractableMessageHub.OnInteractableLostPlayer -= UnregisterInteractable;
    }

    private void Update()
    {
        if((transform.position - _lastPlayerPosition).sqrMagnitude < LastPlayerPositionThreshold)
            return;

        _lastPlayerPosition = transform.position;

        if (_registeredInteractables.Count <= 0)
            return;
        
        var newClosest = GetClosestInteractable();
        if (newClosest == _closestInteractable)
            return;
        
        if (_closestInteractable)
            _closestInteractable.DeclareClosest(false);
        
        newClosest.DeclareClosest(true);
        _closestInteractable = newClosest;
    }

    public void InteractCallback(InputAction.CallbackContext obj)
    {
        if (obj.canceled)
            return;
        
        if (_registeredInteractables.Count <= 0)
            return;
        
        GetClosestInteractable().Interact();
    }

    private void RegisterInteractable(Interactable interactable)
    {
        if (Vector3.Distance(transform.position, interactable.transform.position) > InteractableMaxDistanceThreshold)
            return;
        
        _registeredInteractables.Add(interactable);
        
        onInteractableFound?.Invoke();
    }

    private void UnregisterInteractable(Interactable interactable)
    {
        _registeredInteractables.Remove(interactable);
        
        if (_registeredInteractables.Count > 0) 
            return;
        
        _closestInteractable = null;
        onInteractableLost?.Invoke();
    }

    private Interactable GetClosestInteractable()
    {
        var closest = _registeredInteractables[0];

        for (int i = 1; i < _registeredInteractables.Count; i++)
        {
            if ((transform.position - _registeredInteractables[i].transform.position).sqrMagnitude 
                < 
                (transform.position - closest.transform.position).sqrMagnitude)
            {
                closest = _registeredInteractables[i];
            }
        }
        
        return closest;
    }
}
