using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public bool IsFinished => isFinished;
    public bool WaitUntilFinished => waitUntilFinished;
    
    [SerializeField]
    protected bool waitUntilFinished;
    
    protected bool isFinished;

    protected BattleEntity ownerEntity, targetEntity;

    public virtual void Init(BattleEntity owner, BattleEntity target)
    {
        ownerEntity = owner;
        targetEntity = target;
    }

    public virtual void StartAction()
    {
        isFinished = false;
    }
}