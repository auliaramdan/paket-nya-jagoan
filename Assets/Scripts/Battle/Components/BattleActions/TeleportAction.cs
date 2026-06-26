using UnityEngine;

public class TeleportAction : BaseAction
{
    [SerializeField]
    private Vector3 offset;
    
    public override void StartAction()
    {
        base.StartAction();

        ownerEntity.transform.position = targetEntity.transform.position + offset;
        isFinished = true;
    }
}