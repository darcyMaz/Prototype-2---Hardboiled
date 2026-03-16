using UnityEngine;

public class GSM_FightEnd : GinoStartMovement
{
    private void OnEnable()
    {
        FightManager.instance.OnFightCompleted += GSMInvoke;
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightCompleted -= GSMInvoke;
    }
}
