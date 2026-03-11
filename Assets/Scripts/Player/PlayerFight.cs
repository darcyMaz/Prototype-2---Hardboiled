using Unity.VisualScripting;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool CanUseSR = false;
    [SerializeField] private Sprite idleFightSprite;
    [SerializeField] private Sprite walkSprite;

    private void Awake()
    {
        if (!TryGetComponent(out sr)) Debug.Log("The PlayerFight component cannot find the SpriteRenderer on the player.");
        else CanUseSR = true;
    }
    private void OnEnable()
    {
        FightManager.instance.OnFightStarted += FightStart;
        FightManager.instance.OnFightCompleted += FightEnd;
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightStarted -= FightStart;
        FightManager.instance.OnFightCompleted -= FightEnd;
    }


    private void FightStart()
    {
        if (CanUseSR)
        {
            sr.sprite = idleFightSprite;
        }
    }
    private void FightEnd()
    {
        if (CanUseSR)
        {
            sr.sprite = walkSprite;
        }
    }
}
