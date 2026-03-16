using UnityEngine;

public class GSM_DialogueDone : GinoStartMovement
{
    [SerializeField] private Dialogue dialogue;

    private void OnEnable()
    {
        dialogue.OnDialogueDone += GSMInvoke;
    }
    private void OnDisable()
    {
        dialogue.OnDialogueDone -= GSMInvoke;
    }
}
