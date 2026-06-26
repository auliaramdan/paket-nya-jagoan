using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleEntityUI : MonoBehaviour
{
    [SerializeField]
    private BattleEntityScriptableObject data;

    [SerializeField]
    private Canvas entityCanvas;
    
    [SerializeField]
    private Button attackButton, defendButton;
    [SerializeField]
    private Toggle skillPanelToggle;

    [SerializeField]
    private Transform skillPanel;

    [SerializeField]
    private BattleEntity target;

    private void Start()
    {
        attackButton.onClick.AddListener(() => {data.OnActionSelected?.Invoke(data.BasicAttack, target);});
    }

    private void OnEnable()
    {
        data.OnTurnStart += OnTurnStart;
        data.OnTurnEnd += OnTurnEnd;
    }
    
    private void OnDisable()
    {
        data.OnTurnStart += OnTurnStart;
        data.OnTurnEnd += OnTurnEnd;
    }

    private void OnTurnStart()
    {
        entityCanvas.enabled = true;
    }
    
    private void OnTurnEnd()
    {
        entityCanvas.enabled = false;
    }
}
