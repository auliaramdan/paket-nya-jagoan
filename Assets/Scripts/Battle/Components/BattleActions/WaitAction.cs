using System.Collections;
using UnityEngine;

public class WaitAction : BaseAction
{
    [SerializeField]
    private float seconds = 1f;
    
    public override void StartAction()
    {
        base.StartAction();
        
        StartCoroutine(WaitActionCoroutine());
    }

    private IEnumerator WaitActionCoroutine()
    {
        yield return new WaitForSecondsRealtime(seconds);
        isFinished = true;
    }
}
