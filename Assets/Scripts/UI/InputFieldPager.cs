using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldPager : MonoBehaviour
{
    [SerializeField] private string solution;
    private TMP_InputField tmp;
    private Image img;
    private string current_str;
    private Color correctColor = new Color(0.4745f, 0.851f, 0.49f, 1);
    private float ChangeSceneTimer = 0;
    private float ChangeSceneTime = 0.7f;
    private bool CanSceneChange = false;

    // Having many different solutions is important as the pager codes have duplicates for a given number.
    // Small mistakes should be fine.
    private string[] solutions =
    {
        "test",
        "meet in alley by factory now"
    };

    private void Awake()
    {
        if (!TryGetComponent(out img)) Debug.Log("The InputFieldPager script could not find its ImageUI component.");
        if (!TryGetComponent(out tmp)) Debug.Log("The InputFieldPager script could not find its TMP_InputField component.");
    }

    private void Update()
    {
        if (ChangeSceneTimer <= 0f && CanSceneChange)
        {
            SceneManager.Instance.BufferSceneChange("Bar - Act4B");

            // We only want to buffer the scene once so make sure to make this bool false.
            CanSceneChange = false;
        }
        ChangeSceneTimer = (ChangeSceneTime <= 0f) ? 0: ChangeSceneTimer - Time.deltaTime;
    }

    public void CompareToSolution()
    {
        if (tmp != null) current_str = tmp.text;

        bool correct = false;
        for (int i=0; i<solutions.Length; i++)
        {
            // Go through each possible solution, if the input is equal to one then the puzzle is solved.
            if (current_str.ToLower() == solutions[i].ToLower()) { correct = true; break; }
        }
        if (correct) CompletePuzzle();
    }

    private void CompletePuzzle()
    {
        img.color = correctColor;
        CanSceneChange = true;
        ChangeSceneTimer = ChangeSceneTime;   
    }
}
