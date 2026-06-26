using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public enum EntitySide
{
    Ally,
    Enemy
}

public class BattleEntity : MonoBehaviour
{
    public BattleEntityScriptableObject Data => data;
    
    [SerializeField]
    private BattleEntityScriptableObject data;

    [SerializeField]
    private UnityEvent onTurnStart, onTurnEnd;

    private List<BattleActionManager> _actionPool = new();

    private void OnEnable()
    {
        data.OnTurnStart += StartTurn;
        data.OnTurnEnd += EndTurn;
        data.OnActionSelected += OnActionSelected;
    }

    private void OnDisable()
    {
        data.OnTurnStart -= StartTurn;
        data.OnTurnEnd -= EndTurn;
        data.OnActionSelected -= OnActionSelected;
    }

    private void StartTurn()
    {
        onTurnStart?.Invoke();
    }

    private void EndTurn()
    {
        onTurnEnd?.Invoke();
    }
    
    private void OnActionSelected(BattleActionManager requestedAction, BattleEntity target)
    {
        var action = _actionPool.Find(x => x == requestedAction);

        if (action == null)
        {
            action = Instantiate(requestedAction, transform);
            action.Initialize(this, target);
            _actionPool.Add(action);
        }

        action.StartActionSequence();
    }
}
