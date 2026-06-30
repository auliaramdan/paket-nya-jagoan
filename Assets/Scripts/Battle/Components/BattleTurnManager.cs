using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    [SerializeField]
    private BattleTurnUI battleTurnUI;

    private List<BattleEntity> _battleEntities = new();
    private List<BattleEntity> _entitiesTurn = new();
    private List<BattleEntityTurn> _battleEntityTurns;

    public void Initialize(List<BattleEntity> entities)
    {
        _battleEntities = entities;
    }
    
    private IEnumerator Start()
    {
        _battleEntityTurns = _battleEntities.Select(battleEntity => new BattleEntityTurn { Entity = battleEntity, TurnTime = 1f/battleEntity.Spd * 10}).ToList();
        
        for (int i = 0; i < 10; i++)
        {
            NextTurn();
        }
        battleTurnUI.InitializeEntityList(_entitiesTurn);
        
        yield return new WaitForSeconds(2f);
        
        _entitiesTurn[0].OnTurnStart?.Invoke(new TurnDetail{consecutiveTurnCount = GetConsecutiveTurnAmount(_entitiesTurn[0])});
    }
    
    private void OnEnable()
    {
        foreach (var battleEntity in _battleEntities) 
            battleEntity.OnTurnEnd += AdvanceTurn;
    }

    private void OnDisable()
    {
        foreach (var battleEntity in _battleEntities) 
            battleEntity.OnTurnEnd -= AdvanceTurn;
    }

    private void NextTurn()
    {
        var min = _battleEntityTurns.Select(entityTurn => entityTurn.TurnTime).Min();
        var added = false;
        
        foreach (var entityTurn in _battleEntityTurns)
        {
            if (entityTurn.TurnTime - min <= 0 && !added)
            {
                added = true;
                _entitiesTurn.Add(entityTurn.Entity);
                entityTurn.TurnTime = 1f/entityTurn.Entity.Spd * 10;
                continue;
            }
            
            entityTurn.TurnTime -= min;
        }
    }

    private void AdvanceTurn(TurnDetail turnDetail)
    {
        StartCoroutine(AdvanceTurnCoroutine(turnDetail));
    }

    private IEnumerator AdvanceTurnCoroutine(TurnDetail turnDetail)
    {
        _entitiesTurn.RemoveRange(0, turnDetail.consecutiveTurnCount);

        for (int i = 0; i < turnDetail.consecutiveTurnCount; i++)
        {
            NextTurn();
        }
        
        battleTurnUI.AdvanceTurnUI(_entitiesTurn);

        yield return new WaitForSeconds(1f);
        
        _entitiesTurn[0].OnTurnStart?.Invoke(new TurnDetail{consecutiveTurnCount = GetConsecutiveTurnAmount(_entitiesTurn[0])});
    }

    public void RemoveEntity(BattleEntity entity)
    {
        _battleEntities.Remove(entity);
        _battleEntityTurns.Remove(_battleEntityTurns.FirstOrDefault(e => e.Entity == entity));
        AdvanceTurn(new TurnDetail(){consecutiveTurnCount = 10});
    }

    private int GetConsecutiveTurnAmount(BattleEntity entity)
    {
        if (_entitiesTurn[0] != entity)
            return 0;

        for (var i = 0; i < _entitiesTurn.Count; i++)
        {
            if (_entitiesTurn[i] != entity)
                return i;
        }
        
        return _entitiesTurn.Count;
    }

    private class BattleEntityTurn
    {
        public BattleEntity Entity;
        public float TurnTime;
    }
}

[Serializable]
public class TurnDetail
{
    public int consecutiveTurnCount;
}