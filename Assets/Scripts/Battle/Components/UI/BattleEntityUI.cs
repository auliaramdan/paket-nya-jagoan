using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(BattleEntity))]
public class BattleEntityUI : MonoBehaviour
{

    [SerializeField]
    private UIDocument uiDocument;
    [SerializeField]
    private VisualTreeAsset skillVisualAsset;
    
    private Button _attackButton, _defendButton,  _skillPanelButton;
    private ListView _skillListView;

    private BattleEntity _target;
    private BattleEntity _owner;
    private int _consecutiveTurnCount;
    private bool _targetPicked;
    private BattleActionManager _pendingAction;
    private VisualElement _rootElement;

    private void Awake()
    {
        _rootElement = uiDocument.rootVisualElement;
    }

    private void OnEnable()
    {
        _owner ??= GetComponent<BattleEntity>();
        
        _attackButton ??= uiDocument.rootVisualElement.Q<Button>("attack");
        _defendButton ??= uiDocument.rootVisualElement.Q<Button>("defend");
        _skillPanelButton ??= uiDocument.rootVisualElement.Q<Button>("skill");
        _skillListView ??= uiDocument.rootVisualElement.Q<ListView>("skill-list");

        SetupSkillListView();
        
        _rootElement.style.visibility = Visibility.Hidden;
        
        _attackButton.RegisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked, _owner.EntityData.BasicAttack);
        _defendButton.RegisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked, _owner.EntityData.BasicDefense);
        _skillPanelButton.RegisterCallback<ClickEvent>(SkillButtonClicked);
        
        _owner.OnTurnStart += OnTurnStart;
        
        PlayerTargetInput.OnPlayerTarget += OnPlayerTarget;
        BattleManager.OnBattleEnd += OnBattleEnd;
    }

    private void OnDisable()
    {
        _attackButton.UnregisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked);
        _defendButton.UnregisterCallback<ClickEvent, BattleActionManager>(ActionButtonClicked);
        _skillPanelButton.UnregisterCallback<ClickEvent>(SkillButtonClicked);
        
        _owner.OnTurnStart -= OnTurnStart;
        PlayerTargetInput.OnPlayerTarget -= OnPlayerTarget;
        BattleManager.OnBattleEnd -= OnBattleEnd;
    }

    private void OnTurnStart(TurnDetail detail)
    {
        _targetPicked = false;
        uiDocument.rootVisualElement.style.visibility = Visibility.Visible;
        _consecutiveTurnCount = detail.consecutiveTurnCount;
        _skillListView.RefreshItems();
    }

    private void OnPlayerTarget(BattleEntity obj)
    {
        _targetPicked = true;
        _target = obj;
    }
    
    private void OnBattleEnd()
    {
        _rootElement.style.display = DisplayStyle.None;
    }

    private void SkillButtonClicked(ClickEvent evt)
    {
        _skillListView.style.visibility = _skillListView.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
    }
    
    private void ActionButtonClicked(ClickEvent evt, BattleActionManager action)
    {
        if (_pendingAction == action)
            return;
        
        _skillListView.style.visibility = Visibility.Hidden;
        
        ActionSelected(action);
    }

    private void ActionSelected(BattleActionManager action)
    {
        _pendingAction = action;
        if (!action.RequireTarget)
            _targetPicked = true;
        else
            PlayerTargetInput.OnPlayerChoosingTarget?.Invoke(true);

        StartCoroutine(WaitForTarget());
    }
    
    private void SetupSkillListView()
    {
        _skillListView.makeItem = () => skillVisualAsset.Instantiate();
        _skillListView.itemsSource = _owner.EntityData.Skills;
        _skillListView.bindItem = (element, i) =>
        {
            element.enabledSelf = _owner.EntityData.Skills[i].actionManager.ActionCost <= _consecutiveTurnCount;
            Debug.Log(_consecutiveTurnCount);
            
            if (_owner.ActionPool.TryGetValue(_owner.EntityData.Skills[i].actionManager, out var cooldown))
                element.enabledSelf = cooldown.CurrentCooldown <= 0 && element.enabledSelf;
            
            element.Q<Label>().text = $"{_owner.EntityData.Skills[i].actionName} - {_owner.EntityData.Skills[i].actionManager.ActionCost} Turn";
        };
        _skillListView.itemsChosen += selected =>
        {
            ActionSelected(((EntityActions)selected.First()).actionManager);
            _skillListView.style.visibility = Visibility.Hidden;
        };
    }

    private IEnumerator WaitForTarget()
    {
        while (!_targetPicked)
            yield return null;
        
        _owner.OnActionSelected(_pendingAction, _target);
        _pendingAction = null;
        _target = null;
        _skillListView.ClearSelection();
        _skillListView.style.visibility = Visibility.Hidden;
        _rootElement.style.visibility = Visibility.Hidden;
        PlayerTargetInput.OnPlayerChoosingTarget?.Invoke(false);
    }
}
