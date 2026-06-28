using System;
using UnityEngine;

[RequireComponent(typeof(BattleEntity))]
public class PlayerTargetInput : MonoBehaviour
{
    public static Action<BattleEntity> OnPlayerTarget;

    private BattleEntity _battleEntity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _battleEntity = GetComponent<BattleEntity>();
    }

    public void Targeted()
    {
        OnPlayerTarget?.Invoke(_battleEntity);
    }
}
