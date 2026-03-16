using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth ph;
    private bool UsesUI = false;

    private RectTransform rectTransform;
    private Slider slider;

    private void Awake()
    {
        if (!TryGetComponent(out rectTransform)) Debug.Log("The HealthUI could not access its RectTransform.");
        else if (!TryGetComponent(out slider)) Debug.Log("The HealthUI could not access its Slider.");
        else if (ph != null)
        {
            UsesUI = true;
        }
    }

    private void OnEnable()
    {
        FightManager.instance.OnFightStarted += OpenUI;
        FightManager.instance.OnFightCompleted += CloseUI;
        ph.OnPlayerHit += DecreaseUI;
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightStarted -= OpenUI;
        FightManager.instance.OnFightCompleted -= CloseUI;
        ph.OnPlayerHit -= DecreaseUI;
    }

    private void OpenUI()
    {
        if (UsesUI)
        {
            rectTransform.anchoredPosition = new Vector3(-192, -79, 0);
        }
    }
    private void CloseUI()
    {
        if (UsesUI)
        {
            rectTransform.anchoredPosition = new Vector3(-192, 100, 0);
        }
    }

    private void DecreaseUI(int damage)
    {
        // decrease ui to a minimum of zero
        if (UsesUI) slider.value = (slider.value - damage < 0) ? 0: slider.value - damage;
    }

}
