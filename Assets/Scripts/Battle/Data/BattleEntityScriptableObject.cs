using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleEntityScriptableObject", menuName = "Scriptable Objects/BattleEntityScriptableObject")]
public class BattleEntityScriptableObject : ScriptableObject
{
    public int DefaultSpd => defaultSpd;
    public int DefaultAtk => defaultAtk;
    public int DefaultDef => defaultDef;
    public float MaxHp => maxHp;
    public Sprite Sprite => sprite;
    public EntitySide Side => side;
    public RuntimeAnimatorController AnimatorController => animatorController;
    public BattleActionManager BasicAttack => basicAttack;
    public BattleActionManager BasicDefense => basicDefense;
    public List<EntityActions> Skills => skills;

    [SerializeField]
    private float maxHp;

    [SerializeField]
    private int defaultAtk, defaultDef, defaultSpd;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private EntitySide side;
    [SerializeField]
    private RuntimeAnimatorController animatorController;

    [SerializeField]
    private BattleActionManager basicAttack, basicDefense;
    [SerializeField]
    private List<EntityActions> skills;
}

public enum EntitySide
{
    Ally,
    Enemy
}

[Serializable]
public class EntityActions
{
    public string actionName;
    public BattleActionManager actionManager;
}