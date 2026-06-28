using UnityEngine;

public class DamageTargetAction : BaseAction
{
    [SerializeField]
    private float baseDamage;
    
    public override void StartAction()
    {
        base.StartAction();
        
        targetEntity.ModifyStats(-(ownerEntity.Atk * baseDamage - targetEntity.Def), 0, 0, 0);
        isFinished = false;
    }
}
