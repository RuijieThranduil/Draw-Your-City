using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormPrompts : Form
{
    public TMP_InputField inputField;
    public GameObject panel_Generating;
    public GameObject panel_Confirm;
    public RawImage img_Result;
    public Transform transRoot_PromptsToggle;
    
    public Texture2D texInputImage;
    public Texture2D texInputMask;
    
    List<Toggle> listSelectedToggles = new List<Toggle>();
    
    Texture2D curResultTex;

    private void Awake()
    {
        transRoot_PromptsToggle.GetComponentsInChildren<Toggle>(listSelectedToggles);
    }

    protected override void OnShow()
    {
        texInputImage = (Texture2D)param[0];
        texInputMask = (Texture2D)param[1];
    }

    protected override void OnReset()
    {
        panel_Generating.gameObject.SetActive(false);
        panel_Confirm.gameObject.SetActive(false);
    }

    public void OnClickGenerate()
    {
        var listSelectedPrompts_isOn = listSelectedToggles
            .Where(_ => _.isOn);
        
        string selectedPrompts = string.Empty;
        if (listSelectedPrompts_isOn.Any())
        {
            var listSelectedPrompts = listSelectedPrompts_isOn
                .Select(_ => _.GetComponentInChildren<TMP_Text>().text)
                .ToList();
            
            selectedPrompts = string.Join(" ", listSelectedPrompts);
        }
        
        string prompts = inputField.text + " " + selectedPrompts;
        
        prompts = prompts.Trim();
        
        Debug.Log("Input Prompts:" + prompts);
        
        panel_Generating.gameObject.SetActive(true);
        GameMgr.Ins.GenerateImage(prompts, texInputImage, texInputMask, _ =>
        {
            curResultTex = _;
            img_Result.texture = curResultTex;
            
            panel_Generating.gameObject.SetActive(false);
            panel_Confirm.gameObject.SetActive(true);
        });
    }

    // Generating ================================
    public void OnClickCancel()
    {
        
    }

    // Confirm ================================
    public void OnClickTryAgain()
    {
        OnReset();
    }
    
    public void OnClickAR()
    {
        if (curResultTex != null)
        {
            string strNow = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string filePath = $"{Application.persistentDataPath}/Output/{strNow}.png";
            GTools.SaveTextureToFile(curResultTex, filePath);
            Debug.Log(filePath);
        }
        
        GameMgr.Ins.EnterARStreet();
    }

    public void OnClickBack()
    {
        FormMgr.Ins.OpenForm_Replace<FormMain>();
    }
}
