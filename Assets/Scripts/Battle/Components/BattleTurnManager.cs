using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    [SerializeField]
    private BattleTurnUI battleTurnUI;
    
    [SerializeField]
    private List<BattleEntityScriptableObject> battleEntities = new();
    
    [SerializeField]
    private List<BattleEntityScriptableObject> entitiesTurn = new();

    private List<BattleEntityTurn> _battleEntityTurns;
    
    private IEnumerator Start()
    {
        _battleEntityTurns = battleEntities.Select(battleEntity => new BattleEntityTurn { Entity = battleEntity, TurnTime = battleEntity.Spd }).ToList();
        
        for (int i = 0; i < 10; i++)
        {
            NextTurn();
        }
        battleTurnUI.InitializeEntityList(entitiesTurn);
        
        yield return new WaitForSeconds(2f);
        
        entitiesTurn[0].OnTurnStart?.Invoke();
    }
    
    private void OnEnable()
    {
        foreach (var battleEntity in battleEntities) 
            battleEntity.OnTurnEnd += AdvanceTurn;
    }

    private void OnDisable()
    {
        foreach (var battleEntity in battleEntities) 
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
                entitiesTurn.Add(entityTurn.Entity);
                entityTurn.TurnTime = entityTurn.Entity.Spd;
                continue;
            }
            
            entityTurn.TurnTime -= min;
        }
    }

    private void AdvanceTurn()
    {
        StartCoroutine(AdvanceTurnCoroutine());
    }

    private IEnumerator AdvanceTurnCoroutine()
    {
        entitiesTurn.Remove(entitiesTurn[0]);
        NextTurn();
        battleTurnUI.AdvanceTurnUI(entitiesTurn);

        yield return new WaitForSeconds(1f);
        
        entitiesTurn[0].OnTurnStart?.Invoke();
    }

    private class BattleEntityTurn
    {
        public BattleEntityScriptableObject Entity;
        public float TurnTime;
    }
}
