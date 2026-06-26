using System;
using UnityEngine;

public class ReturnToInitialAction : BaseAction
{
    private Vector3 _initialPosition;

    public override void Init(BattleEntity owner, BattleEntity target)
    {
        base.Init(owner, target);
        
        _initialPosition = ownerEntity.transform.position;
    }

    public override void StartAction()
    {
        ownerEntity.transform.position = _initialPosition;
        isFinished = true;
    }
}
