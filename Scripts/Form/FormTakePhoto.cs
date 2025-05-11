using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FormTakePhoto : Form
{
    public GameObject panel_TakePhoto;
    public GameObject panel_Confirm;
    public GameObject panel_PaintMask;
    public GameObject panel_Photo;

    public RawImage img_CameraShow;
    public RawImage img_Photo;
    
    public DrawImage drawImage_SimplePaint;
    public DrawImage drawImage_PaintMask;
    
    public TakePhotoCamera takePhotoCamera;
    
    protected override void OnReset()
    {
        panel_TakePhoto.SetActive(true);
        panel_Confirm.SetActive(false);
        panel_PaintMask.SetActive(false);
        panel_Photo.SetActive(false);

        drawImage_SimplePaint.SetCanDraw(false);
        drawImage_PaintMask.SetCanDraw(false);
        
        drawImage_SimplePaint.Clear();
        drawImage_PaintMask.Clear();
    }

    // PanelTakePhoto ===============================
    public void OnClickTakePhoto()
    {
        Texture2D texPhoto = takePhotoCamera.TakePhoto();
        //img_Photo.rectTransform.localEulerAngles = takePhotoCamera.rawImage.rectTransform.localEulerAngles;
        img_Photo.texture = texPhoto;
        //img_Photo.texture = img_CameraShow.texture;
        
        panel_TakePhoto.SetActive(false);
        panel_Confirm.SetActive(true);
        panel_Photo.SetActive(true);
    }

    // PanelConfirm==================================
    public void OnClickRetake()
    {
        panel_TakePhoto.SetActive(true);
        panel_Confirm.SetActive(false);
        panel_Photo.SetActive(false);
    }
    
    public void OnClickFinish_TakePhoto()
    {
        panel_Confirm.SetActive(false);
        panel_PaintMask.SetActive(true);
    }

    // PanelPaintMask==================================
    public void OnClickSimplePaint()
    {
        drawImage_SimplePaint.SetCanDraw(true);
        drawImage_PaintMask.SetCanDraw(false);
    }

    public void OnClickUndo()
    {
        drawImage_SimplePaint.Clear();
        drawImage_PaintMask.Clear();
    }

    public void OnClickPaintMask()
    {
        drawImage_SimplePaint.SetCanDraw(false);
        drawImage_PaintMask.SetCanDraw(true);
    }

    public void OnClickDelete()
    {
        OnReset();
    }
    
    public void OnClickFinish_PaintMask()
    {
        Texture2D texMask = drawImage_PaintMask.CropCurrentTexture_InvertMask();
        FormMgr.Ins.pushPopForm<FormPrompts>(img_Photo.texture, texMask);
    }

    public void OnClickBack()
    {
        FormMgr.Ins.OpenForm_Replace<FormMain>();
    }
}
