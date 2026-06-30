using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BattleEntity))]
public class BattleEntityAI : MonoBehaviour
{
    private BattleEntity _owner;

    private BattleEntity[] _validTargets;

    private void OnEnable()
    {
        BattleManager.OnBattleStart += OnEnemyBattleStart;
        
        _owner ??= GetComponent<BattleEntity>();
        _owner.OnTurnStart += OnTurnStart;
    }

    private void OnEnemyBattleStart(List<BattleEntity> obj)
    {
        _validTargets = obj.Where(o => o.EntityData.Side == EntitySide.Ally).ToArray();
        BattleManager.OnBattleStart -= OnEnemyBattleStart;
    }

    private void OnDisable()
    {
        _owner.OnTurnStart -= OnTurnStart;
    }

    private void OnTurnStart(TurnDetail obj)
    {
        _owner.OnActionSelected(_owner.EntityData.BasicAttack, _validTargets[Random.Range(0, _validTargets.Length - 1)]);
    }
}
