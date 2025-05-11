using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARClickObj : MonoBehaviour
{
    public Texture2D texImage;
    public ARButtonType buttonType;
    
    private void OnMouseUpAsButton()
    {
        FormMgr.Ins.SendUIEvent(GameMsg.FormAR_OpenImage, texImage);  // button store the images 
    }

    private void Awake()
    {
        var ps = GetComponentInChildren<ParticleSystem>(); // Subaggregate(Child): different color stand for different type of the AR dots 
        ps.GetComponent<Renderer>().sharedMaterial = Resources.Load<Material>("ARButton_" + buttonType);
    }
}

public enum ARButtonType
{
    Red,
    Yellow,
    Blue,
    Green,
}  //  red = atmosphere , yellow = building , blue = details , green = facade or greenery