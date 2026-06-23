using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationHandler : MonoBehaviour
{
    [SerializeField]
    private string idleAnimationName = "idle", runAnimationName = "run";
    
    private Animator _animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveAnimationCallback(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            _animator.Play(idleAnimationName);
        }
        else if (ctx.performed)
        {
            _animator.Play(runAnimationName);
            var dir = ctx.ReadValue<Vector2>();

            if (dir.x == 0)
                return;
            
            var scale = transform.localScale;
            scale.x = dir.x > 0 ? 1 : -1;
            
            transform.localScale = scale;
        }
    }
}
