using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FormMain : Form
{
    protected override void OnShow()
    {
        
    }

    public void OnClickOpenFormTakePhoto()
    {
        FormMgr.Ins.OpenForm_Replace<FormTakePhoto>();
    }

    public void OnClickOpenARStreet()
    {
        GameMgr.Ins.EnterARStreet();
    }

    public void OnClickSeeRanking()
    {
        FormMgr.Ins.OpenForm_Replace<FormRanking>();
    }

    public void OnClickTrendingPrompts()
    {
        FormMgr.Ins.OpenForm_Replace<FormTrendingPrompts>();
    }
}
