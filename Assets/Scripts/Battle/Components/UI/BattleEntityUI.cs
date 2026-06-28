using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(BattleEntity))]
public class BattleEntityUI : MonoBehaviour
{

    [SerializeField]
    private UIDocument uiDocument;
    
    private Button _attackButton, _defendButton;
    private Toggle _skillPanelToggle;

    [SerializeField]
    private Transform skillPanel;

    private BattleEntity _target;
    private BattleEntity _owner;
    private int _consecutiveTurnCount;
    private bool _targetPicked;
    private BattleActionManager _pendingAction;

    private void OnEnable()
    {
        _owner ??= GetComponent<BattleEntity>();
        
        _attackButton ??= uiDocument.rootVisualElement.Q<Button>("attack");
        _defendButton ??= uiDocument.rootVisualElement.Q<Button>("defend");
        
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        
        _attackButton.RegisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked, _owner.EntityData.BasicAttack);
        _defendButton.RegisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked, _owner.EntityData.BasicDefense);
        
        _owner.OnTurnStart += OnTurnStart;
        
        PlayerTargetInput.OnPlayerTarget += OnPlayerTarget;
    }

    private void OnDisable()
    {
        _attackButton.UnregisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked);
        _defendButton.UnregisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked);
        
        _owner.OnTurnStart -= OnTurnStart;
        PlayerTargetInput.OnPlayerTarget -= OnPlayerTarget;
    }

    private void OnTurnStart(TurnDetail detail)
    {
        _targetPicked = false;
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        _consecutiveTurnCount = detail.consecutiveTurnCount;

        _attackButton.SetEnabled(_owner.EntityData.BasicAttack.ActionCost <= _consecutiveTurnCount);
    }

    private void OnPlayerTarget(BattleEntity obj)
    {
        _targetPicked = true;
        _target = obj;
    }
    
    private void ActionButtonClicked(ClickEvent evt, BattleActionManager action)
    {
        _pendingAction = action;
        if (!action.RequireTarget)
            _targetPicked = true;

        StartCoroutine(WaitForTarget());
    }

    private IEnumerator WaitForTarget()
    {
        while (!_targetPicked)
            yield return null;
        
        _owner.OnActionSelected(_pendingAction, _target);
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
    }
}
