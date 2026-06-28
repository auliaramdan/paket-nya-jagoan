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
        
        ownerEntity.ModifyStats(health, atk, def, spd);
        isFinished = true;
    }
}
