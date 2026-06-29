using System.Collections.Generic;
using UnityEngine;

public class BootstrapBattle : MonoBehaviour
{
    [SerializeField]
    private List<BattleEntityScriptableObject> battleParticipants = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OverworldToBattleSceneHandler.AnnounceBattleParticipant?.Invoke(battleParticipants);
    }
}
