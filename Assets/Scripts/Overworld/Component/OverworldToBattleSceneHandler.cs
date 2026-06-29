using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldToBattleSceneHandler : MonoBehaviour
{
    public static Action<List<BattleEntityScriptableObject>> AnnounceBattleParticipant;

    [SerializeField, Scene]
    private string battleSceneName;
    
    [SerializeField]
    private List<BattleEntityScriptableObject> battleParticipants = new();

    [Button]
    public void MoveToNewScene()
    {
        DontDestroyOnLoad(gameObject);

        var op = SceneManager.LoadSceneAsync(battleSceneName, LoadSceneMode.Single);

        if (op == null)
            throw new NullReferenceException("Cannot load battle scene");
        
        op.allowSceneActivation = true;
        op.completed += _ =>
        {
            StartCoroutine(OnBattleSceneLoad());
        };
    }

    private IEnumerator OnBattleSceneLoad()
    {
        yield return new WaitForSeconds(.1f);
        
        AnnounceBattleParticipant?.Invoke(battleParticipants);
        Destroy(gameObject);
    }
}
