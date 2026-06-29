using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public static Action<List<BattleEntity>> OnBattleStart;
    public static Action OnBattleEnd;
    
    [SerializeField]
    private BattleTurnManager battleTurnManager;

    [SerializeField]
    private List<BattleEntity> playerEntities = new();

    [SerializeField]
    private List<BattleEntity> enemyEntities = new();
    
    [SerializeField]
    private UnityEvent onBattleEndEvent, onBattleWin, onBattleLose, onWrapUp;
    
    private List<BattleEntity> _activeParticipants;

    private void Awake()
    {
        OverworldToBattleSceneHandler.AnnounceBattleParticipant += AnnounceBattleParticipant;
    }

    private void AnnounceBattleParticipant(List<BattleEntityScriptableObject> obj)
    {
        OverworldToBattleSceneHandler.AnnounceBattleParticipant -= AnnounceBattleParticipant;

        var players = obj.Where(be => be.Side == EntitySide.Ally).ToArray();
        for (int i = 0; i < players.Length; i++)
        {
            playerEntities[i].Initialize(players[i]);
            playerEntities[i].gameObject.SetActive(true);
        }
        
        var enemies = obj.Where(be => be.Side == EntitySide.Enemy).ToArray();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyEntities[i].Initialize(enemies[i]);
            enemyEntities[i].gameObject.SetActive(true);
        }

        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(.1f);

        _activeParticipants = playerEntities.Where(go => go.gameObject.activeInHierarchy)
            .Concat(enemyEntities.Where(go => go.gameObject.activeInHierarchy))
            .ToList();
        
        OnBattleStart?.Invoke(_activeParticipants);
        
        battleTurnManager.Initialize(_activeParticipants);
        battleTurnManager.gameObject.SetActive(true);
        
        foreach (var activeParticipant in _activeParticipants)
        {
            activeParticipant.OnEntityDied += OnEntityDied;
        }
    }

    private void OnEntityDied(BattleEntity battleEntity)
    {
        var remaining = _activeParticipants.Where(e => e != battleEntity);
        if (remaining.Any(e => e.EntityData.Side == battleEntity.EntityData.Side))
        {
            _activeParticipants.Remove(battleEntity);
            battleTurnManager.RemoveEntity(battleEntity);
            battleEntity.gameObject.SetActive(false);
            return;
        }
        
        onBattleEndEvent?.Invoke();
        OnBattleEnd?.Invoke();
        
        if (battleEntity.EntityData.Side == EntitySide.Enemy)
            onBattleWin?.Invoke();
        else
            onBattleLose?.Invoke();
        
        StartCoroutine(EndBattleCoroutine());
    }

    private IEnumerator EndBattleCoroutine()
    {
        yield return new WaitForSeconds(2f);
        
        onWrapUp?.Invoke();
    }
}
