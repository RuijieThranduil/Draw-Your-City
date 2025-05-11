using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormAR : Form
{
    // ImageDetail ==================================
    public GameObject Panel_ImageDetail;
    public Slider slider_Like;
    public RawImage img_ImageDetail;

    private void Awake()
    {
        CtorEvent()[(int)GameMsg.FormAR_OpenImage] = obj =>
        {
            Texture2D texImage = (Texture2D)obj[0];
            img_ImageDetail.texture = texImage;
            OpenPanel_ImageDetail(true);
        };
    }

    protected override void OnShow()
    {
        slider_Like.value = 0.5f;
        
        Panel_ImageDetail.SetActive(false);
    }

    public void OnClickBack()
    {
        CloseSelf();
    }

    protected override void OnClose()
    {
        base.OnClose();

        Scene ARScene = SceneManager.GetSceneByName("ARScene");
        if (ARScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(ARScene);
        }

        FormMgr.Ins.OpenForm_Replace<FormMain>();
    }

    void OpenPanel_ImageDetail(bool bOpen)
    {
        if (bOpen)
        {
            Panel_ImageDetail.SetActive(true);
            slider_Like.value = 0.5f;
        }
        else
        {
            Panel_ImageDetail.SetActive(false);
        }
    }

    public void OnClickOpenFormRanking()
    {
        FormMgr.Ins.OpenForm_Replace<FormRanking>();
    }

    public void OnClickCloseImageDetail()
    {
        OpenPanel_ImageDetail(false);
    }
}
