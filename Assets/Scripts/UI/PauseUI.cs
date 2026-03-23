using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Image PauseBG;
    [SerializeField] private Image PauseTitleBG;
    [SerializeField] private TextMeshProUGUI PauseTitle;
    [SerializeField] private Image PauseTextBG;
    [SerializeField] private TextMeshProUGUI PauseText;
    [SerializeField] private Image PauseControlsBG;
    [SerializeField] private TextMeshProUGUI PauseControlsText;

    private void Start()
    {
        PauseUIOff();
    }

    private void OnEnable()
    {
        PauseManager.instance.OnPause += PauseUIOn;
        PauseManager.instance.OnPauseEnd += PauseUIOff;
    }

    private void OnDisable()
    {
        PauseManager.instance.OnPause -= PauseUIOn;
        PauseManager.instance.OnPauseEnd -= PauseUIOff;
    }

    private void PauseUIOn()
    {
        PauseBG.color = new Color(1,1,1,0.19f);
        PauseTitleBG.color = Color.black;
        PauseTitle.text = "Pause Menu";
        PauseTextBG.color = Color.black;
        PauseText.text = "P to Return to Game\nL to Quit Game";
        PauseControlsBG.color = Color.black;
        PauseControlsText.text = "Move: WASD Keys / Left Stick\r\nFight: UIO Keys / Triggers and Bumpers\r\nInteract: W Key / X Button / Square Button \r\n";
    }
    private void PauseUIOff()
    {
        PauseBG.color = new Color(1, 1, 1, 0);
        PauseTitleBG.color = new Color(PauseTitleBG.color.r, PauseTitleBG.color.g, PauseTitleBG.color.b, 0);
        PauseTitle.text = "";
        PauseTextBG.color = new Color(PauseTextBG.color.r, PauseTextBG.color.g, PauseTextBG.color.b, 0);
        PauseText.text = "";
        PauseControlsBG.color = new Color(PauseControlsBG.color.r, PauseControlsBG.color.g, PauseControlsBG.color.b, 0);
        PauseControlsText.text = "";
    }
}
