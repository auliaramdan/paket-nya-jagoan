using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleEntityScriptableObject", menuName = "Scriptable Objects/BattleEntityScriptableObject")]
public class BattleEntityScriptableObject : ScriptableObject
{
    public Action OnTurnStart, OnTurnEnd;
    public Action<BattleActionManager, BattleEntity> OnActionSelected;
    
    public float Spd => spd;
    public Sprite Sprite => sprite;
    public EntitySide Side => side;
    public BattleActionManager BasicAttack => basicAttack;
    public BattleActionManager BasicDefense => basicDefense;
    public List<EntityActions> Skills => skills;
    
    [SerializeField]
    private float spd;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private EntitySide side;

    [SerializeField]
    private BattleActionManager basicAttack, basicDefense;
    [SerializeField]
    private List<EntityActions> skills;
}

[Serializable]
public class EntityActions
{
    public string actionName;
    public BattleActionManager actionManager;
}
