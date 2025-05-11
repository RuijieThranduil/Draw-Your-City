using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : UnitySingleton<GameMgr>  
{
    public Camera uiCamera;
    
    public DalleAPI dalleAPI;  // The singleton pattern ensures global uniqueness 

    protected override void Awake()   // Unity's lifecycle approach
    {
        base.Awake();
        
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        FormMgr.Ins.OpenForm_Replace<FormMain>();
    }

    public void GenerateImage(string prompts, Texture2D texInputImage, Texture2D texInputMask,  Action<Texture2D> onComplete)
    {
        if (string.IsNullOrEmpty(prompts))
        {
            Debug.Log("Please enter a valid prompt.");
            return;
        }

        StartCoroutine(dalleAPI.GenerateImageWithMask(prompts, texInputImage, texInputMask, onComplete));
    }

    public void EnterARStreet()
    {
        SceneManager.LoadScene("ARScene", LoadSceneMode.Additive);
        
        FormMgr.Ins.OpenForm_Replace<FormAR>();
    }
}
