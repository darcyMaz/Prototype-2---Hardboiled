using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    
    private InteractOverlap InteractOverlap;
    private bool UseUI = false;

    [SerializeField] private Image uiBG;
    [SerializeField] private TextMeshProUGUI uiText;

    // This bool is seperate from UseUI because not all InteractUIs will have Dialogues accompanying them.
    private Dialogue dialogue;
    private bool HasDialogue = false;

    private LockDialogue lockDialogue;
    private bool CanLockUI;

    private void Awake()
    {
        if (!TryGetComponent(out InteractOverlap)) Debug.Log("A DoorUI object tried to get a DoorOverlap component from the same GameObject.");
        else if (uiBG == null) Debug.Log("Could not find the background Sprite UI element for the InteractUI component.");
        else if (uiText == null) Debug.Log("Could not find the TextMeshProUGUI UI element for the InteractUI component.");
        else UseUI = true;

        // No Debug.Log warning because to have no Dialogue is expected behaviour.
        if (TryGetComponent(out dialogue)) HasDialogue = true;

        if (!TryGetComponent(out lockDialogue)) Debug.Log("An InteractUI component could not find a LockDialogue. It may not have intentionally.");
        else CanLockUI = true;
    }

    private void OnEnable()
    {
        if (UseUI)
        {
            InteractOverlap.OnOverlap += EnableUI;
            InteractOverlap.OnOverlapEnd += DisableUI;

            if (HasDialogue)
            {
                dialogue.OnDialogueDone += LockUI;
            }
        }

        if (CanLockUI)
        {
            lockDialogue.OnFlipLockDialogue += LockUI;
        }
    }

    private void OnDisable()
    {
        if (UseUI)
        {
            InteractOverlap.OnOverlap -= EnableUI;
            InteractOverlap.OnOverlapEnd -= DisableUI;

            if (HasDialogue)
            {
                dialogue.OnDialogueDone -= LockUI;
            }
        }

        if (CanLockUI)
        {
            lockDialogue.OnFlipLockDialogue -= LockUI;
        }
    }
   
    private void EnableUI()
    {
        if (UseUI)
        {
            uiText.text = "Press W to Interact";
            uiBG.color = Color.black;
        }
    }
    private void DisableUI()
    {
        if (UseUI)
        {
            uiText.text = "";
            uiBG.color = new Color(0,0,0,0);
        }
    }

    private void LockUI()
    {
        if (UseUI) DisableUI();
        UseUI = (UseUI) ? false : true;
    }

}
