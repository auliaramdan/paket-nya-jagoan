using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionManager : MonoBehaviour
{
    public bool IsFinished { get; private set; }
    public int ActionCost => actionCost;
    public bool RequireTarget => requireTarget;

    [SerializeField]
    private int actionCost = 1;
    [SerializeField]
    private bool requireTarget = true;
    
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
        IsFinished = false;
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
        IsFinished = true;
    }
}
