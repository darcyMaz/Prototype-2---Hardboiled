using System;
using UnityEngine;
using System.Collections.Generic;
public class PlayerHealth : MonoBehaviour 
{ 
    private SpriteRenderer sr; 
    private bool UsesSR = false; 
    // private bool CanTakeDamage = false; 
    [SerializeField] private static float MaxHealth = 5; 
    [SerializeField] private float health = MaxHealth; 
    [SerializeField] private int hitHealth = 1; 
    private Color hitColor = new Color(0.862f, 0.517f, 0.517f); 
    public event Action OnPlayerDeath; 
    public event Action <int> OnPlayerHit;

    private void Awake()
    {
        if (!TryGetComponent(out sr)) Debug.Log("The PlayerHealth script could not find the Player's SpriteRenderer component.");
        else UsesSR = true;
    }

    private void OnEnable() 
    { 
        OnPlayerHit += PlayerHit; 
    } 
    private void OnDisable() 
    { 
        OnPlayerHit -= PlayerHit; 
    } 
    private void PlayerHit(int damage) 
    { 
        health -= damage; 
        if (health <= 0) 
        { 
            PlayerDeath(); return; 
        } 
        else if (UsesSR) 
        { 
            sr.color = hitColor; // sr.sprite = hitSprite;
        }
        Invoke("PlayerHitDone", 0.6f);     
    }     
    private void PlayerHitDone()     
    {         
        if (UsesSR) sr.color = Color.white; // sr.sprite = readySprite;
    }      
    private void PlayerDeath()     
    {   
        // Dead enemy sprite: for now just the normal hit sprite
        if (UsesSR)
        {
              sr.color = hitColor; // sr.sprite = hitSprite;
        }
        // Invoke("DestroyPlayer", 0.7f);
        OnPlayerDeath?.Invoke();
    }     
    private void DestroyPlayer()     
    {         
        try         
        {             
            OnPlayerDeath.Invoke();         
        }         
        catch (NullReferenceException e)         
        {             
            Debug.Log("An Enemy died without subscribers to its OnEnemyDead event. This behaviour may be normal.\n" + e.Message);         
        }                  
        Destroy(gameObject);     
    }     
    public void PlayerHitTriggered()     
    {         
        OnPlayerHit.Invoke(hitHealth);     
    } 
}