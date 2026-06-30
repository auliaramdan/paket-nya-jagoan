using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionManager : MonoBehaviour
{
    public bool IsFinished { get; private set; }
    public int ActionCost => actionCost;
    public int CurrentCooldown => _currentCooldown;
    public bool RequireTarget => requireTarget;

    [SerializeField]
    private int actionCost = 1, actionDuration, actionCooldown;
    [SerializeField]
    private bool requireTarget = true;
    
    [SerializeField]
    private List<BaseAction> actions = new();

    [SerializeField]
    private List<BaseAction> executions = new();

    [SerializeField]
    private BattleEntity owner, target;
    
    private int _currentDuration, _currentCooldown;

    public void Initialize(BattleEntity ownerEntity, BattleEntity targetEntity)
    {
        owner = ownerEntity;
        target = targetEntity;
        
        owner.OnTurnEnd += OnTurnEnd;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartActionSequence()
    {
        _currentCooldown = actionCooldown;
        _currentDuration = actionDuration;
        
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
    
    private void OnTurnEnd(TurnDetail obj)
    {
        if (_currentDuration == 1)
        {
            foreach (var action in actions)
            {
                action.DurationFinished();
            }
        }
        
        _currentCooldown -= obj.consecutiveTurnCount;
        _currentDuration -= obj.consecutiveTurnCount;
        
        if (_currentCooldown <= 0) _currentCooldown = 0;
        if (_currentDuration <= 0) _currentDuration = 0;
    }
}
