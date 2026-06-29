using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BattleEntity : MonoBehaviour
{
    public Action<TurnDetail> OnTurnStart;
    public Action<TurnDetail> OnTurnEnd;
    public Action<BattleEntity> OnEntityDied;
    
    [CreateProperty]
    public float CurrentHealth => currentHealth;
    [CreateProperty]
    public int Atk => atk;
    [CreateProperty]
    public int Def => def;
    [CreateProperty]
    public int Spd => spd;

    public Animator EntityAnimator => _animator;

    public BattleEntityScriptableObject EntityData => data;
    public Dictionary<BattleActionManager, BattleActionManager> ActionPool => _actionPool;
    
    [SerializeField]
    private BattleEntityScriptableObject data;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private int atk, def, spd;

    private Animator _animator;
    private readonly Dictionary<BattleActionManager, BattleActionManager> _actionPool = new();

    public void Initialize(BattleEntityScriptableObject newData)
    {
        data = newData;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = data.AnimatorController;
        
        currentHealth = data.MaxHp;
        atk = EntityData.DefaultAtk;
        def = EntityData.DefaultDef;
        spd = EntityData.DefaultSpd;
    }

    public void OnActionSelected(BattleActionManager requestedAction, BattleEntity target)
    {
        if (!_actionPool.TryGetValue(requestedAction, out var action))
        {
            action = Instantiate(requestedAction, transform);
            _actionPool.Add(requestedAction, action);
        }
        
        action.Initialize(this, target);
        StartCoroutine(StartActionCoroutine(action));
    }

    private IEnumerator StartActionCoroutine(BattleActionManager action)
    {
        action.StartActionSequence();

        while (!action.IsFinished)
            yield return null;
        
        OnTurnEnd?.Invoke(new TurnDetail{consecutiveTurnCount = action.ActionCost});
    }

    public void ModifyStats(float newHealth, int newAtk, int newDef, int newSpd)
    {
        currentHealth += newHealth;
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
            OnEntityDied?.Invoke(this);
            return;
        }
        
        atk += newAtk;
        def += newDef;
        spd += newSpd;
    }
}
