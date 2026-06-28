using UnityEngine;

public class AnimationAction : BaseAction
{
    [SerializeField]
    private string targetAnimationName;
    
    public override void StartAction()
    {        
        base.StartAction();

        ownerEntity.EntityAnimator.Play(targetAnimationName);
        isFinished = true;
    }
}