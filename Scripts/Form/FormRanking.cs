using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormRanking : Form
{
    public void OnClickBack()
    {
        FormMgr.Ins.OpenForm_Replace<FormMain>();
    }
}
