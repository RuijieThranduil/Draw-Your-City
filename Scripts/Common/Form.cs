using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Form : DirtyNode
{
    public GameObject goPanelBg;
    public List<Form> mListChildForm;
    public List<ScrollRect> childScrollRects;
    private bool mIsPopForm = false;

    [NonSerialized]
    public object[] param;                          //参数
    //UI上的监听事件列表
    protected Dictionary<int, Callback<object[]>> eventDic = null;
    public virtual Dictionary<int, Callback<object[]>> CtorEvent()
    {
        if (eventDic == null)
            eventDic = new Dictionary<int, Callback<object[]>>();
        return eventDic;
    }
    public bool isPopForm
    {
        set
        {
            mIsPopForm = value;
        }
        get
        {
            return mIsPopForm;
        }
    }
    
    public virtual void Show(bool ishsow)
    {
        if (ishsow == IsShow())
            return;

        if (ishsow)
        {
            gameObject.SetActive(true);
            OnShow();
            //GameSoundMgr.Ins.PlaySound(4);
            Dirty(true);
            ResetScroolRects();
        }
        else
        {
            OnClose();
            gameObject.SetActive(false);
        }
    }

    protected void ResetScroolRects()
    {
        childScrollRects.ForEach(_ =>
        {
            if (_ != null && _.vertical)
                _.verticalNormalizedPosition = 1;
        });
    }

    public virtual bool IsShow()
    {
        return /*enabled && */gameObject.activeSelf;
    }

    protected virtual void OnShow()
    {
        
    }

    protected virtual void OnClose()
    {
        
    }

    Tween tweenClose;
    public virtual void OnClickClose()
    {
        if (tweenClose != null && tweenClose.IsPlaying())
            return;
    }

    protected void CloseSelf()
    {
        if (isPopForm)
            FormMgr.Ins.PopForm(GetType());
        else
            FormMgr.Ins.PopForm();
    }
}

public enum eFormOpenAnim
{
    None,
    Scale,
    Move,
    Half,
}
