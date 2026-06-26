using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionManager : MonoBehaviour
{
    [SerializeField]
    private List<BaseAction> actions = new();

    [SerializeField]
    private List<BaseAction> executions = new();

    [SerializeField]
    private BattleEntity owner, target;

    public void Initialize(BattleEntity ownerEntity, BattleEntity targetEntity)
    {
        owner = ownerEntity;
        target = targetEntity;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartActionSequence()
    {
        foreach (var action in actions)
        {
            action.Init(owner, target);
        }

        StartCoroutine(ExecuteActionsCoroutine());
    }

    private IEnumerator ExecuteActionsCoroutine()
    {
        foreach (var action in executions)
        {
            action.StartAction();

            if (!action.WaitUntilFinished) continue;

            while (!action.IsFinished)
            {
                yield return null;
            }
        }
        
        owner.Data.OnTurnEnd?.Invoke();
    }
}
