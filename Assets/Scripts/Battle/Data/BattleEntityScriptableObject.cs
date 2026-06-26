using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleEntityScriptableObject", menuName = "Scriptable Objects/BattleEntityScriptableObject")]
public class BattleEntityScriptableObject : ScriptableObject
{
    public Action OnTurnStart, OnTurnEnd;
    
    public float Spd => spd;
    public Sprite Sprite => sprite;
    public EntitySide Side => side;
    
    [SerializeField]
    private float spd;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private EntitySide side;
}
