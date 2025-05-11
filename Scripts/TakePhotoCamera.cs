using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class TakePhotoCamera : MonoBehaviour
{
    public RawImage rawImage;
    
    private WebCamTexture webCamTexture;
    
    void OnEnable()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            OpenCamera();
        }
        else
        {
            OpenCameraPermission();
        }
    }

    private void OnDisable()
    {
        CloseCamera();
    }

    public void OpenCamera()
    {
        StartCoroutine(OpenCameraAsync());
    }
    
    public void CloseCamera()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            Destroy(webCamTexture);
            webCamTexture = null;
        }
    }

    IEnumerator OpenCameraAsync()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] webCamDevices = WebCamTexture.devices;
            if (webCamDevices != null && webCamDevices.Length > 0)
            {
                string webCamName = webCamDevices[0].name;
                webCamTexture = new WebCamTexture(webCamName, Screen.width, Screen.height);
                webCamTexture.Play();
                    
                Debug.Log($"WebCamTexture: Name:{webCamTexture.name} Width:{webCamTexture.width} Height:{webCamTexture.height}");
                
                rawImage.texture = webCamTexture;
                rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);
               
                
                // if (webCamTexture.videoRotationAngle != 0)
                // {
                //     rawImage.rectTransform.sizeDelta = new Vector2(webCamTexture.height, webCamTexture.width);
                // }
                // else
                {
                    rawImage.rectTransform.sizeDelta = new Vector2(webCamTexture.width, webCamTexture.height);
                }
                
                // var arf = rawImage.GetOrAddComponent<AspectRatioFitter>();
                // arf.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                // arf.aspectRatio = webCamTexture.width / (float)webCamTexture.height;
            }
        }
    }
    
    private void OpenCameraPermission()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            return;
        }
        
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
        callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
    }
 
    Texture2D RotateTexture_Positive(Texture2D texture)
    {
        // 获取原始纹理的像素数据
        Color[] originalPixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;

        // 创建一个新的纹理对象，大小交换，宽高互换
        Texture2D rotatedTexture = new Texture2D(height, width);

        // 旋转像素
        Color[] rotatedPixels = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 旋转90度，x -> y，y -> width - 1 - x
                rotatedPixels[x * height + (height - 1 - y)] = originalPixels[y * width + x];
            }
        }

        // 将旋转后的像素设置到新的纹理中
        rotatedTexture.SetPixels(rotatedPixels);
        rotatedTexture.Apply();

        return rotatedTexture;
    }
    
    Texture2D RotateTexture_Negative(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        Texture2D rotatedTexture = new Texture2D(height, width);

        // 遍历原始纹理的每个像素并根据旋转规则放置到新纹理中
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                // 逆时针旋转90度后的新坐标
                rotatedTexture.SetPixel(y, width - 1 - x, pixelColor);
            }
        }

        rotatedTexture.Apply();  // 应用更改
        return rotatedTexture;
    }
    
    void PermissionCallbacks_PermissionDenied(string PermissionName)
    {
        Debug.Log($"PermissionCallbacks_PermissionDenied[{PermissionName}]");
    }
 
    //本次允许
    void PermissionCallbacks_PermissionGranted(string PermissionName)
    {
        Debug.Log($"PermissionCallbacks_PermissionGranted[{PermissionName}]");
        OpenCamera();
    }
    void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string PermissionName)
    {
        Debug.Log($"PermissionCallbacks_PermissionDeniedAndDontAskAgain[{PermissionName}]");
    }

    public Texture2D TakePhoto()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying)
        {
            return null;
        }
        
        // if (blockWidth == -1 || blockHeight == -1)
        //     texture2D.SetPixels(webCamTexture.GetPixels());    
        // else
        //1:1
        
        // #if UNITY_EDITOR
        //     int blockWidth = webCamTexture.width;
        //     int blockHeight = webCamTexture.width;
        // #else
            int blockWidth = webCamTexture.height;
            int blockHeight = webCamTexture.height;
        //#endif
        
        var pixels = webCamTexture.GetPixels(webCamTexture.width / 2 - blockWidth / 2,
            webCamTexture.height / 2 - blockHeight / 2, blockWidth, blockHeight);
            
        Texture2D texture2D = new Texture2D(blockWidth, blockHeight, TextureFormat.RGBA32, false);
        texture2D.SetPixels(pixels);
        texture2D.Apply();

        Debug.Log("webCamTexture.videoRotationAngle: " + webCamTexture.videoRotationAngle);
        Texture2D texRotated = RotateTexture_Negative(texture2D);
        return texRotated;
        // if (webCamTexture.videoRotationAngle == -90)
        // {
        //     Texture2D texRotated = RotateTexture_Positive(texture2D);
        //     return texRotated;
        // }
        //
        // if (webCamTexture.videoRotationAngle == 90)
        // {
        //     Texture2D texRotated = RotateTexture_Negative(texture2D);
        //     return texRotated;
        // }

        return texture2D;
    }

    public void SaveImage(Texture2D tex)
    {
        byte[] imageBytes = tex.EncodeToJPG();

        if (imageBytes != null && imageBytes.Length > 0)
        {
            string savePath;

            string platformPath = Application.persistentDataPath + "/MyTempPhotos";

            if (!Directory.Exists(platformPath))
            {
                Directory.CreateDirectory(platformPath);
            }

            savePath = platformPath + "/" + Time.deltaTime + ".jpg";

            File.WriteAllBytes(savePath, imageBytes);
        }
    }
}