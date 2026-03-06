using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    private InteractOverlap interactOverlap;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Image imageUI;
    [SerializeField] private TextAsset text;
    private bool IsOverlapping = false;


    private bool UsesDialogue = false;
    private bool DialogueStarted = false;
    private bool MainDialogueDone = false;
    private string[] lines;
    private int lineIndex = 0;
    private int EODIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (imageUI == null) Debug.Log("A Dialogue component tried to access the Image component on the canvas. It may not be assigned in the inspector.");
        else if (textUI == null) Debug.Log("The Dialogue component tried to find the TextMesh component on the canvas. It may not be assigned in the inspector.");
        else UsesDialogue = true;
    }

    private void Awake()
    {
        if (!TryGetComponent(out interactOverlap)) Debug.Log("A Dialogue component tried to find the InteractOverlap. This object may not have it as a component");

        if (text != null)
        {
            string lines_raw = text.text;
            lines = lines_raw.Split('\n');

            Debug.Log(lines[15] + " " + lines[15].Length);
            for (int i = 0; i < lines[15].Length; i++)
            {
                bool temp = (lines[15][i] == '\n') ? true: false;
                Debug.Log(lines[15][i] + " " + temp);
            }
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

        PlayerInteract.OnInteractPressed += DialoguePressed;
    }
    private void OnDisable()
    {
        if (interactOverlap != null)
        {
            interactOverlap.OnOverlap -= ReadyDialogue;
            interactOverlap.OnOverlapEnd -= UnreadyDialogue;
        }
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
            DialogueStarted = true;

            OpenDialogueBox();
            
            /*
            if (MainDialogueDone)
            {
                if (lineIndex == lines.Length)
                {
                    lineIndex = EODIndex + 1;
                }
            }
            */

            SendDialogue(lines[lineIndex]);
            lineIndex++;
        }
        else if (IsOverlapping && UsesDialogue && DialogueStarted)
        {
            if (MainDialogueDone && lineIndex == lines.Length)
            {
                lineIndex = EODIndex + 1;
                CloseDialogueBox();
            }
            else if (lines[lineIndex] == "EOD")
            {
                CloseDialogueBox();
                EODIndex = lineIndex;
                MainDialogueDone = true;
                lineIndex++;
            }
            else if (MainDialogueDone)
            {
                CloseDialogueBox();
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
    
}
