using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovementHandler : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    
    private Vector2 _moveDirection = Vector2.zero;
    private CharacterController _characterController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void UpdateMoveDirection(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            var direction = ctx.ReadValue<Vector2>();
            _moveDirection = direction.normalized;
        }
        else if (ctx.canceled)
        {
            _moveDirection = Vector3.zero;
        }
    }

    private void Update()
    {
        // if (_moveDirection.x != 0 || _moveDirection.z != 0)
            _characterController.Move(_moveDirection * (moveSpeed * Time.deltaTime));
    }
}
