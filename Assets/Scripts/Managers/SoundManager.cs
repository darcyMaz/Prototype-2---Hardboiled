using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip outsideBG;
    [SerializeField] private AudioClip barBG;

    [SerializeField] private AudioClip PlayerHit;
    [SerializeField] private AudioClip EnemyHit;
    [SerializeField] private AudioClip EnemyHit2;

    [SerializeField] private AudioSource BG;
    private bool HasBGMusic = false;

    [SerializeField] private AudioSource SFX;
    private bool HasSFX = false;

    [SerializeField] private PlayerPunchTrigger PlayerPunchTrigger;
    private bool CanHitPlayer = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        if (BG == null) Debug.Log("The SoundManager could not find its Background music AudioSource.");
        else HasBGMusic = true;

        if (SFX == null) Debug.Log("The SoundManager could not find its SFX AudioSource.");
        else HasSFX = true;

        if (PlayerPunchTrigger == null) Debug.Log("The SoundManager could not find its PlayerHitTrigger.");
        else CanHitPlayer = true;
    }

    private void OnEnable()
    {
        if (HasBGMusic)
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string shortenedName = sceneName.Split(" ")[0];

            if (shortenedName == "Outside" || shortenedName == "Factory")
            {
                BG.Stop();
                BG.clip = outsideBG;
                BG.Play();
            }
            else if (shortenedName == "Bar")
            {
                BG.Stop();
                BG.clip = barBG;
                BG.Play();
            }
            else
            {
                Debug.Log("A scene with no specified Background Music has started. This may be intentional.");
                BG.Stop();
            }
        }
        
        if (HasSFX)
        {
            // No EPTs added if there are none.
            foreach (EnemyPunchTrigger ept in EnemyPunchTrigger.GetEPTs())
            {
                ept.OnPlayerHit += PlayerHitSound;
            }
            if (CanHitPlayer) PlayerPunchTrigger.OnEnemyHit += EnemyHitSound;
        }
        


    }

    private void OnDisable()
    {
        // if (HasBGMusic) UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SceneChanged;

        if (HasSFX)
        {
            foreach (EnemyPunchTrigger ept in EnemyPunchTrigger.GetEPTs())
            {
                ept.OnPlayerHit -= PlayerHitSound;
            }
            if (CanHitPlayer) PlayerPunchTrigger.OnEnemyHit -= EnemyHitSound;
        }
    }

    private void PlayerHitSound()
    {
        SFX.clip = PlayerHit;
        SFX.Play();
    }
    private void EnemyHitSound()
    {
        // Get a random int
        System.Random rand = new System.Random();
        int random_num = rand.Next();

        // If it's even play the first one, if it's odd play the second one.
        if (random_num % 2 == 0) SFX.clip = EnemyHit;
        else SFX.clip = EnemyHit2;

        SFX.Play();
    }
}
