using System;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    // turns off sprite at the end of the animation

    private SpriteRenderer sr;
    private bool UseSR = false;
    [SerializeField] private bool LastEnemy = false;
    [SerializeField] private SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent(out sr)) Debug.Log("An Enemy could not find its SpriteRenderer component.");
        else UseSR = true;
    }

    private void EnemyAnimOver()
    {
        if (UseSR) sr.enabled = false;
        if (LastEnemy) playerSprite.enabled = true;
    }
}
