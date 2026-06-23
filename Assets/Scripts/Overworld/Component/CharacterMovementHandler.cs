using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovementHandler : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    
    private Vector2 _moveDirection = Vector2.zero;
    private Rigidbody2D _rb2D;
    
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
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
            _moveDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        _rb2D.MovePosition(_rb2D.position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));
    }
}
