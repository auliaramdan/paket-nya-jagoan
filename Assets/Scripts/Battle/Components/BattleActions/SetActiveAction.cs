using UnityEngine;

public class SetActiveAction : BaseAction
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private bool active;

    public override void StartAction()
    {
        base.StartAction();

        target.SetActive(active);
        isFinished = true;
    }
}
