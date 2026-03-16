using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dialogue is a class allowing the player to interact with and initiate a conversation with a BoxCollider2D represented by NPCs or doors.
/// This class should be generalized so that it does not only activate on Interact.
/// 
/// Dialogue should be the super or abstract class of DialogueOnInteract, DialogueOnFightEnd, DialogueOnFightStart, etc.
///
/// </summary>
public class Dialogue : MonoBehaviour
{
    private InteractOverlap interactOverlap;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Image imageUI;
    private bool IsOverlapping = false;

    [SerializeField] private TextAsset text;
    private bool UsesDialogue = false;
    private bool DialogueStarted = false;
    private bool MainDialogueDone = false;
    private string[] lines;
    private int lineIndex = 0;
    private int EODIndex = 0;

    // This event plays when the main dialogue is done.
    public event System.Action OnDialogueDone;
    // This event plays when a dialogue box opens.
    public event System.Action OnDialogueStarted;
    // This event plays when a dialogue box closes.
    public event System.Action OnDialogueClosed;

    private static List<Dialogue> dialogues = new List<Dialogue>();

    private LockDialogue lockDialogue;
    private bool CanLock = false;

    private void Awake()
    {
        dialogues.Add(this);

        if (imageUI == null) Debug.Log("A Dialogue component tried to access the Image component on the canvas. It may not be assigned in the inspector.");
        else if (textUI == null) Debug.Log("The Dialogue component tried to find the TextMesh component on the canvas. It may not be assigned in the inspector.");
        else UsesDialogue = true;

        if (!TryGetComponent(out lockDialogue)) Debug.Log("A Dialogue Component could not find its LockDialogue. It may not have one.");
        else CanLock = true;

        if (!TryGetComponent(out interactOverlap)) Debug.Log("A Dialogue component tried to find the InteractOverlap. This object may not have it as a component");

        if (text != null)
        {
            string lines_raw = text.text;
            lines = lines_raw.Split('\n');
        }
        else
        {
            lines = new string[3];
            lines[0] = "The text for this dialogue could not be found.";
            lines[1] = "EOD";
            lines[2] = "The text for this dialogue could not be found.";
        }
    }

    private void OnEnable()
    {
        if (interactOverlap != null)
        {
            interactOverlap.OnOverlap += ReadyDialogue;
            interactOverlap.OnOverlapEnd += UnreadyDialogue;
        }

        if (CanLock)
        {
            lockDialogue.OnFlipLockDialogue += FlipDialogueLock;
        }

        PlayerInteract.OnInteractPressed += DialoguePressed;

    }
    private void OnDisable()
    {
        if (interactOverlap != null)
        {
            interactOverlap.OnOverlap -= ReadyDialogue;
            interactOverlap.OnOverlapEnd -= UnreadyDialogue;
        }

        if (CanLock)
        {
            lockDialogue.OnFlipLockDialogue += FlipDialogueLock;
        }

        PlayerInteract.OnInteractPressed -= DialoguePressed;

        dialogues.Remove(this);
    }

    private void ReadyDialogue()
    {
        IsOverlapping = true;
    }
    private void UnreadyDialogue()
    {
        IsOverlapping = false;
    }
    private void DialoguePressed()
    {
        // This has to do two things:
        // 1) Start dialogue
        // 2) Continue dialogue

        // 1) Start
        //    Check if we can start (not started already, isoverlapping, usesdialogue)
        //    DialogueStarted = true;
        //    Make image alpha = 255
        //    Make text the first line
        //    Ooooh also lock player movement!
        // 2) Continue Dialogue
        //    if dialoguestarted == true
        //    if that was the last line then
        //          make text nothing
        //          make image alpha = 0
        //          dialoguestarted = false
        //    else Make text the next line
        

        if (IsOverlapping && UsesDialogue && !DialogueStarted)
        {
            
            // If the EOD (End Of Dialogue) line is the final line, this if statement will run and won't let any dialogue appear.
            if (MainDialogueDone)
            {
                if (lineIndex == lines.Length)
                {
                    return ;
                }
            }


            OnDialogueStarted?.Invoke();
            DialogueStarted = true;
            OpenDialogueBox();
            SendDialogue(lines[lineIndex]);
            lineIndex++;
        }
        else if (IsOverlapping && UsesDialogue && DialogueStarted)
        {
            if (MainDialogueDone && lineIndex == lines.Length)
            {
                lineIndex = EODIndex + 1;
                CloseDialogueBox();
                OnDialogueClosed?.Invoke();
            }
            else if (lines[lineIndex] == "EOD")
            {
                CloseDialogueBox();
                EODIndex = lineIndex;
                MainDialogueDone = true;
                lineIndex++;

                if (lineIndex == lines.Length)
                {
                    OnDialogueDone.Invoke();
                }
                else OnDialogueDone?.Invoke();
                OnDialogueClosed?.Invoke();
            }
            else if (MainDialogueDone)
            {
                CloseDialogueBox();
                OnDialogueClosed?.Invoke();

            }
            else 
            {
                SendDialogue(lines[lineIndex]);
                lineIndex++;
            }
        }
    }

    
    private void SendDialogue(string line)
    {
        textUI.text = line;
    }

    private void OpenDialogueBox()
    {
        // Dialogue box made visible by adjusting alpha.
        imageUI.color = new Color(imageUI.color.r, imageUI.color.g, imageUI.color.b, 255);
    }

    private void CloseDialogueBox()
    {
        imageUI.color = new Color(imageUI.color.r, imageUI.color.g, imageUI.color.b, 0);
        SendDialogue("");
        DialogueStarted = false;
    }

    private void FlipDialogueLock()
    {
        UsesDialogue = (UsesDialogue) ? false : true;
    }

    public static IEnumerable<Dialogue> GetEnabledDialogues()
    {
        foreach (var item in dialogues)
        {
            yield return item;
        }
    }
}
