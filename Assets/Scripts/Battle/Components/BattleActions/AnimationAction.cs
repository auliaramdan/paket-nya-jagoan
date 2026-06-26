using UnityEngine;

public class AnimationAction : BaseAction
{
    [SerializeField]
    private string targetAnimationName;
    
    public override void StartAction()
    {        
        base.StartAction();

        var animator = ownerEntity.GetComponent<Animator>();
        animator.Play(targetAnimationName);
        isFinished = true;
    }
}