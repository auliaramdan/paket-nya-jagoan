using UnityEngine;

public class ModifyOwnStatAction : BaseAction
{
    [SerializeField]
    private float health;

    [SerializeField]
    private int atk, def, spd;
    
    public override void StartAction()
    {
        base.StartAction();
        
        ownerEntity.ModifyStats(0, atk, def, spd);
        isFinished = true;
    }

    public override void DurationFinished()
    {
        base.DurationFinished();
        
        ownerEntity.ModifyStats(0, -atk, -def, -spd);
    }
}
