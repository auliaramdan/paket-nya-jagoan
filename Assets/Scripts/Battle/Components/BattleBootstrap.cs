using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    public static Action<BattleEntity[]> OnBattleStart;
    
    [SerializeField]
    private BattleTurnManager battleTurnManager;

    [SerializeField]
    private List<BattleEntity> playerEntities = new();

    [SerializeField]
    private List<BattleEntity> enemyEntities = new();

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
        for (int i = 0; i < players.Length; i++)
        {
            enemyEntities[i].Initialize(enemies[i]);
            enemyEntities[i].gameObject.SetActive(true);
        }

        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(.1f);

        var activeParticipant = playerEntities.Where(go => go.gameObject.activeInHierarchy)
            .Concat(enemyEntities.Where(go => go.gameObject.activeInHierarchy))
            .ToArray();
        
        OnBattleStart?.Invoke(activeParticipant);
        
        battleTurnManager.Initialize(activeParticipant);
        battleTurnManager.gameObject.SetActive(true);
    }
}
