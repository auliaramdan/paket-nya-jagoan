using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BattleEntity))]
public class PlayerTargetInput : MonoBehaviour
{
    public static Action<BattleEntity> OnPlayerTarget;
    public static Action<bool> OnPlayerChoosingTarget;

    [SerializeField]
    private UnityEvent<bool> onPlayerChoosingTarget;
    
    private BattleEntity _battleEntity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _battleEntity = GetComponent<BattleEntity>();
    }

    private void OnEnable()
    {
        OnPlayerChoosingTarget += OnPlayerChoosingTargetEvt;
    }

    private void OnDisable()
    {
        OnPlayerChoosingTarget -= OnPlayerChoosingTargetEvt;
    }

    private void OnPlayerChoosingTargetEvt(bool obj)
    {
        onPlayerChoosingTarget?.Invoke(obj);
        Debug.Log("Choose target");
    }

    public void Targeted()
    {
        OnPlayerTarget?.Invoke(_battleEntity);
    }
}
