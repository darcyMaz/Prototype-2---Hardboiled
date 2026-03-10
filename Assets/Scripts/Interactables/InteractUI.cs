using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    // private SpriteRenderer ui;
    private bool UseUI = false;
    private InteractOverlap InteractOverlap;

    [SerializeField] private Image uiBG;
    [SerializeField] private TextMeshProUGUI uiText;

    private void OnEnable()
    {
        if (!TryGetComponent(out InteractOverlap))
        {
            Debug.Log("A DoorUI object tried to get a DoorOverlap component from the same GameObject.");
        }
        else
        {
            InteractOverlap.OnOverlap += EnableUI;
            InteractOverlap.OnOverlapEnd += DisableUI;
        }
    }

    private void OnDisable()
    {
        if (InteractOverlap != null)
        {
            InteractOverlap.OnOverlap -= EnableUI;
            InteractOverlap.OnOverlapEnd -= DisableUI;
        }
    }

    private void Start()
    {        
        /*
        // Cycle through the child GameObjects to find the one that has the InteractPrompt tag.
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "InteractPrompt")
            {
                // Make sure the right component exists at that GameObject.
                if (!transform.GetChild(i).TryGetComponent(out ui)) Debug.Log("A door could not find its corresponding UI gameobject so it will not appear.");
                else UseUI = true;

                // End the loop after you've found it.
                break;
            }
        }
        if (UseUI) ui.enabled = false;
        */

        if (uiBG == null || uiText == null) Debug.Log("Could not find one or both UI elements for the InteractUI component.");
        else UseUI = true;
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

}
