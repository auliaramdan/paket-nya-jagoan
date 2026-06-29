public class ModifyTargetStatAction : BaseAction
{
    public int atk;
    public int def;
    public int spd;
    
    public override void StartAction()
    {
        base.StartAction();
        
        targetEntity.ModifyStats(0, atk, def, spd);
        isFinished = true;
    }

    public override void DurationFinished()
    {
        base.DurationFinished();
        
        targetEntity.ModifyStats(0, -atk, -def, -spd);
    }
}
