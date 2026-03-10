using System;
using UnityEngine;

public class PagerLockListener : MonoBehaviour
{
    [SerializeField] private Dialogue bartender;
    private void OnEnable()
    {
        if (bartender != null)
        {
            bartender.OnDialogueDone += SwitchToPagerScene;
        }
        else Debug.Log("The Dialogue component from the bartender could not be found. Connect it from the inspector if you have not laready.");
    }
    private void OnDisable()
    {
        if (bartender != null) bartender.OnDialogueDone -= SwitchToPagerScene;
    }

    private void SwitchToPagerScene()
    {
        try
        {
            SceneManager.Instance.BufferSceneChange("Pager - Act 4A");
        }
        catch (Exception e)
        { 
            Debug.Log("PagerLockListener could not find the SceneManager's static instance. Scene change failed.");
            Debug.Log(e.Message);
        }
        
    }
}
