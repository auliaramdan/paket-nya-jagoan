using System;
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
    [SerializeField]
    private BattleEntityScriptableObject data;

    [SerializeField]
    private UnityEvent onTurnStart, onTurnEnd;

    private void OnEnable()
    {
        data.OnTurnStart += StartTurn;
    }
    
    private void OnDisable()
    {
        data.OnTurnStart -= StartTurn;
    }

    private void StartTurn()
    {
        onTurnStart?.Invoke();
    }

    [Button]
    private void FinishTurn()
    {
        onTurnEnd?.Invoke();
        data.OnTurnEnd?.Invoke();
    }
}
